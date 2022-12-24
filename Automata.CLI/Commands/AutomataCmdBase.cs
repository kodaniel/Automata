namespace Automata.CLI.Commands;

[HelpOption("--help")]
abstract class AutomataCmdBase
{
    protected virtual Task<int> OnExecute(CommandLineApplication app)
    {
        return Task.FromResult(0);
    }
}