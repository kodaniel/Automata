using Automata.CLI.Services;
using System.ComponentModel.DataAnnotations;

namespace Automata.CLI.Commands;

[Command("start", Description = "Start runtime.")]
class StartCmd : AutomataCmdBase
{
    private readonly DaemonClientService _client;

    [Required]
    [Argument(0, "WORKFLOW", "Workflow id.")]
    public string? WorkflowId { get; }

    public StartCmd(DaemonClientService client)
    {
        _client = client;
    }

    protected override async Task<int> OnExecute(CommandLineApplication app)
    {
        var success = await _client.StartAsync(WorkflowId!);
        if (success)
        {
            Console.WriteLine("Workflow started.");
            return 0;
        }
        else
        {
            Console.WriteLine("Invalid workflow id.");
            return 1;
        }
    }
}
