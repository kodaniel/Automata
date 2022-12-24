using Automata.Runtime.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Automata.CLI.Commands;

[Command("enable", Description = "Enable workflow.")]
class EnableWorkflowCmd : AutomataCmdBase
{
    private readonly IWorkflowService workflowService;

    [Required]
    [Argument(0, Description = "Workflow id.")]
    public string WorkflowId { get; set; }

    public EnableWorkflowCmd(IWorkflowService workflowService)
    {
        this.workflowService = workflowService;
    }

    protected override Task<int> OnExecute(CommandLineApplication app)
    {
        var workflow = workflowService.Get(WorkflowId);
        workflow.IsEnabled = true;

        return base.OnExecute(app);
    }
}
