namespace Automata.Runtime.CLI;

[HelpOption("--help")]
abstract class AutomataCmdBase
{
    protected virtual Task<int> OnExecute(CommandLineApplication app)
    {
        return Task.FromResult(0);
    }
}