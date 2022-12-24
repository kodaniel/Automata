using Automata.Daemon.Models;

namespace Automata.Daemon.Contracts;

public interface IWorkflowService
{
    bool IsRunning { get; }

    IReadOnlyCollection<WorkflowArgs> Workflows { get; }

    void Start();
    void Stop();
    WorkflowArgs? Get(string id);
    void AddWorkflow(WorkflowArgs workflow);
    bool RemoveWorkflow(Guid workflowId);
}
