using Automata.Daemon.Models;

namespace Automata.Daemon.Contracts;

public interface IWorkflowService
{
    bool IsRunning { get; }

    IReadOnlyCollection<WorkflowArgs> Workflows { get; }

    Task StartAsync();
    Task StopAsync();
    WorkflowArgs? Get(string id);
    Task AddWorkflowAsync(WorkflowArgs workflow);
    Task<bool> RemoveWorkflowAsync(Guid workflowId);
}
