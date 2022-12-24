using Automata.CLI.Services;

namespace Automata.CLI.Commands;

[Command("list", "ls", Description = "List workflows.")]
class ListCmd : AutomataCmdBase
{
    private readonly DaemonClientService _client;

    public ListCmd(DaemonClientService client)
	{
        _client = client;
    }

    protected override async Task<int> OnExecute(CommandLineApplication app)
    {
        var workflows = _client.ListWorkflowsAsync();

        var tableBuilder = new ConsoleTableBuilder()
            .AddColumn("Id")
            .AddColumn("Name")
            .AddColumn("Enabled")
            .AddColumn("Last triggered");

        await foreach (var wf in workflows)
        {
            tableBuilder.AddRow(wf.Id.LeadingSegment(), wf.Name, wf.IsEnabled.ToString(), wf.LastTriggered.ToString());
        }

        tableBuilder.Write();

        return 0;
    }
}
