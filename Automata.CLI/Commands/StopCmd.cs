using Automata.CLI.Services;
using System.ComponentModel.DataAnnotations;

namespace Automata.CLI.Commands;

[Command("stop", Description = "Stop the runtime.")]
class StopCmd : AutomataCmdBase
{
    private readonly DaemonClientService _client;

    [Required]
    [Argument(0, "WORKFLOW", "Workflow id.")]
    public string? WorkflowId { get; }

    public StopCmd(DaemonClientService client)
	{
        _client = client;
    }

    protected override async Task<int> OnExecute(CommandLineApplication app)
    {
        var success = await _client.StopAsync(WorkflowId!);

        if (success)
        {
            Console.WriteLine("Workflow stopped.");
            return 0;
        }
        else
        {
            Console.WriteLine("Invalid workflow id.");
            return 1;
        }
    }
}
