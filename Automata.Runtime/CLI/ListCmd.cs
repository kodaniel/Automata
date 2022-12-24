using Automata.Runtime.Contracts;
using Automata.Runtime.Helpers;

namespace Automata.Runtime.CLI;

[Command("list", "ls", Description = "List workflows.")]
class ListCmd : AutomataCmdBase
{
    private readonly IWorkflowService _workflowService;

    public ListCmd(IWorkflowService workflowService)
	{
        _workflowService = workflowService;
    }

    protected override Task<int> OnExecute(CommandLineApplication app)
    {
        var tableBuilder = new ConsoleTableBuilder()
            .AddColumn("Id")
            .AddColumn("Name")
            .AddColumn("Enabled")
            .AddColumn("Last triggered");

        foreach (var wf in _workflowService.Workflows)
        {
            tableBuilder.AddRow(wf.Id.LeadingSegment(), wf.Name, wf.IsEnabled.ToString(), wf.LastTriggered.ToString());
        }

        tableBuilder.Write();

        return base.OnExecute(app);
    }
}
