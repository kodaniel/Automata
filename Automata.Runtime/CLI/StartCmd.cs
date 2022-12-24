using Automata.Runtime.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Automata.Runtime.CLI;

[Command("start", Description = "Start runtime.")]
class StartCmd : AutomataCmdBase
{
    private readonly IWorkflowService _workflowService;

    [Required]
    [Argument(0, "WORKFLOW", "Workflow id.")]
    public string? WorkflowId { get; }

    public StartCmd(IWorkflowService workflowService)
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
            workflow.IsEnabled = true;
            Console.WriteLine("Workflow started.");
            return Task.FromResult(0);
        }
    }
}
