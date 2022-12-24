using Automata.Runtime.Services;
using System.Reflection;

namespace Automata.CLI.Commands;

[Command("autom", OptionsComparison = StringComparison.InvariantCultureIgnoreCase)]
[VersionOptionFromMember("-v|--version", MemberName = nameof(GetVersion))]
[Subcommand(typeof(StartCmd))]
[Subcommand(typeof(StopCmd))]
[Subcommand(typeof(ListCmd))]
[Subcommand(typeof(PluginsCmd))]
[Subcommand(typeof(EnableWorkflowCmd))]
class AutomataCmd : AutomataCmdBase
{
    [Option("-C <path>")]
    [FileExists]
    public string? ConfigFile { get; set; }

    protected override Task<int> OnExecute(CommandLineApplication app)
    {
        app.ShowHelp();
        return base.OnExecute(app);
    }

    private static string GetVersion() =>
        typeof(AutomataCmd).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
}
