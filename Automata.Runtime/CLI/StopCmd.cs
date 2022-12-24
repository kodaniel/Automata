using Automata.Runtime.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Automata.Runtime.CLI;

[Command("stop", Description = "Stop the runtime.")]
class StopCmd : AutomataCmdBase
{
    private readonly IWorkflowService _workflowService;

    [Required]
    [Argument(0, "WORKFLOW", "Workflow id.")]
    public string? WorkflowId { get; }

    public StopCmd(IWorkflowService workflowService)
	{
        _workflowService = workflowService;
    }

    protected override Task<int> OnExecute(CommandLineApplication app)
    {
        var workflow = _workflowService.Get(WorkflowId!);
        if (workflow is null)
        {
            Console.WriteLine("Invalid workflow id.");
            return Task.FromResult(1);
        }
        else
        {
            Console.WriteLine(workflow.GetHashCode());
            _workflowService.Workflows.ToList().ForEach(wf => Console.WriteLine(wf.GetHashCode()));
            workflow.IsEnabled = false;
            Console.WriteLine("Workflow stopped.");
            return Task.FromResult(0);
        }
    }
}
