using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Automata.Core.Models;
using Automata.Daemon.Contracts;
using Automata.Daemon.Helpers;
using Automata.Daemon.Models;

namespace Automata.Daemon.Services;

public class WorkflowService : IWorkflowService
{
    private readonly Collection<WorkflowArgs> _workflows = new();
    private readonly ConcurrentQueue<(WorkflowArgs, WorkflowContext)> _triggerQueue = new();
    private readonly ILogger<WorkflowService> _logger;

    private CancellationTokenSource? _cancellationTokenSource;

    public bool IsRunning { get; private set; }

    public WorkflowService(ILogger<WorkflowService> logger)
    {
        _logger = logger;
    }

    public IReadOnlyCollection<WorkflowArgs> Workflows => _workflows;

    public Task StartAsync()
    {
        _logger.LogInformation("Starting workflow service");

        if (!IsRunning)
        {
            IsRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            lock (_workflows)
            {
                foreach (var workflow in _workflows)
                    workflow.StartListening(OnWorkflowTriggered);
            }
        }

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping workflow service");

        _cancellationTokenSource?.Cancel();

        if (IsRunning)
        {
            lock (_workflows)
            {
                foreach (var workflow in _workflows)
                    workflow.StopListening();

                IsRunning = false;
            }
        }

        return Task.CompletedTask;
    }

    public WorkflowArgs? Get(Guid id)
    {
        return _workflows.FirstOrDefault(wf => wf.Id == id);
    }

    public WorkflowArgs? Get(string id)
    {
        var founds = _workflows.Where(wf => wf.Id.ToString().StartsWith(id)).ToList();
        return founds.Count != 1 ? null : Get(founds[0].Id);
    }

    public Task AddWorkflowAsync(WorkflowArgs workflow)
    {
        lock (_workflows)
        {
            _workflows.Add(workflow);

            if (IsRunning)
            {
                workflow.StartListening(OnWorkflowTriggered);
            }
        }

        return Task.CompletedTask;
    }

    public Task<bool> RemoveWorkflowAsync(Guid workflowId)
    {
        lock (_workflows)
        {
            if (_workflows.FirstOrDefault(wf => wf.Id == workflowId) is WorkflowArgs workflow)
            {
                workflow.Dispose();

                return Task.FromResult(_workflows.Remove(workflow));
            }
        }

        return Task.FromResult(false);
    }

    private void OnWorkflowTriggered(WorkflowArgs workflow, WorkflowContext context)
    {
        if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
            return;

        _logger.LogInformation($"Workflow {workflow.Name} has been triggered, queueing the actions.");

        // Add event to the queue
        _triggerQueue.Enqueue((workflow, context));

        // Start executing queue
        ExecuteQueue(_cancellationTokenSource.Token);
    }

    private bool _running = false;
    private async void ExecuteQueue(CancellationToken cancellationToken)
    {
        if (_running)
            return;

        _running = true;

        while (_triggerQueue.TryDequeue(out (WorkflowArgs Workflow, WorkflowContext Context) q))
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            _logger.LogInformation($"Start executing the next queued workflow {q.Workflow.Name}");

            // Create a new Task for every rule execution
            await Task.Factory.StartNew(async () =>
            {
                await ExecuteWorkflowAsync(q.Workflow, q.Context, cancellationToken);
            }, cancellationToken).ContinueWith(_ => { });
        }

        _running = false;
    }

    private async Task ExecuteWorkflowAsync(WorkflowArgs workflow, WorkflowContext context, CancellationToken cancellationToken)
    {
        try
        {
            workflow.Event!.WritePropertiesToContext(context);

            foreach (var action in workflow.Actions)
            {
                if (context.Handled)
                    break;

                _logger.LogInformation($"Executing action {action.GetType().Name}");

                await action.Execute(context, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation($"Executed action {action.GetType().Name}");

                action.WritePropertiesToContext(context);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Workflow has been cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unhandled exception: {ex.Message}");
        }

        //_messenger.Publish(new WorkflowHistory(startTime, workflow, context));
    }
}
