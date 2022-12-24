using Automata.Models;

namespace Automata.Contracts.Services;
public interface IWorkflowService
{
    bool IsRunning { get; }

    void Start();
    void Stop();
    void AddWorkflow(WorkflowArgs workflow);
    bool RemoveWorkflow(WorkflowArgs workflow);
}
