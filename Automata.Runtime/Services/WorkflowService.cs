using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Automata.Core.Contracts.EventAggregator;
using Automata.Core.Models;
using Automata.Runtime.Contracts;
using Automata.Runtime.Helpers;
using Automata.Runtime.Models;

namespace Automata.Runtime.Services;
public class WorkflowService : IWorkflowService
{
    private readonly Collection<WorkflowArgs> _workflows = new();
    private readonly ConcurrentQueue<(WorkflowArgs, WorkflowContext)> _triggerQueue = new();
    private readonly IMessenger _messenger;

    private CancellationTokenSource? _cancellationTokenSource;

    public bool IsRunning { get; private set; }

    public WorkflowService(/*IMessenger messenger*/)
    {
        //_messenger = messenger;
    }

    public IReadOnlyCollection<WorkflowArgs> Workflows => _workflows;

    public void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        StartInternal();
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();

        StopInternal();
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

    private void StartInternal()
    {
        if (IsRunning)
            return;

        IsRunning = true;

        lock (_workflows)
        {
            foreach (var workflow in _workflows)
            {
                workflow.StartListening(OnWorkflowTriggered);
            }
        }
    }

    private void StopInternal()
    {
        if (!IsRunning)
            return;

        lock (_workflows)
        {
            foreach (var workflow in _workflows)
            {
                workflow.StopListening();
            }

            IsRunning = false;
        }
    }

    public void AddWorkflow(WorkflowArgs workflow)
    {
        lock (_workflows)
        {
            _workflows.Add(workflow);

            if (IsRunning)
            {
                workflow.StartListening(OnWorkflowTriggered);
            }
        }
    }

    public bool RemoveWorkflow(Guid workflowId)
    {
        lock (_workflows)
        {
            if (_workflows.FirstOrDefault(wf => wf.Id == workflowId) is WorkflowArgs workflow)
            {
                if (IsRunning)
                {
                    workflow.Event.StopListener();
                }

                return _workflows.Remove(workflow);
            }
        }

        return false;
    }

    private void OnWorkflowTriggered(WorkflowArgs workflow, WorkflowContext context)
    {
        if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
            return;

        // Add event to the queue
        Debug.WriteLine($"Workflow has been triggered, queueing the actions.");
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

            // Create a new Task for every rule execution
            Debug.WriteLine("Start executing the next queued workflow");
            await Task.Factory.StartNew(async () =>
            {
                await ExecuteWorkflowAsync(q.Workflow, q.Context, cancellationToken);
            }, cancellationToken).ContinueWith(_ => { });
        }

        _running = false;
    }

    private async Task ExecuteWorkflowAsync(WorkflowArgs workflow, WorkflowContext context, CancellationToken cancellationToken)
    {
        var startTime = DateTime.Now;

        workflow.Event.WritePropertiesToContext(context);

        foreach (var action in workflow.Actions)
        {
            if (context.Handled)
                break;

            cancellationToken.ThrowIfCancellationRequested();

            await action.Execute(context, cancellationToken);

            action.WritePropertiesToContext(context);
        }

        //_messenger.Publish(new WorkflowHistory(startTime, workflow, context));
    }
}
