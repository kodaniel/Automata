using Automata.Runtime.Models;
using Automata.Runtime.Services;

namespace Automata.CLI.Commands;

[Command("plugins", Description = "List plugins.")]
class PluginsCmd : AutomataCmdBase
{
    private readonly AutomataController automataController;

    [Option("--components", "List components.", CommandOptionType.NoValue)]
    public bool ListComponents { get; set; }

    [Option("--verbose", "Show details.", CommandOptionType.NoValue)]
    public bool Verbose { get; set; }

    public PluginsCmd(AutomataController automataController)
    {
        this.automataController = automataController;
    }

    protected override Task<int> OnExecute(CommandLineApplication app)
    {
        if (!automataController.Plugins.Any())
        {
            Console.WriteLine("There are no plugins added.");
        }
        else
        {
            if (ListComponents)
            {
                automataController.Plugins.SelectMany(plugin => plugin.Blocks).ToList().ForEach(i => Console.WriteLine(i.Name));
            }
            else
            {
                if (Verbose)
                    PrintPluginsDetails(automataController.Plugins);
                else
                    PrintPlugins(automataController.Plugins);
            }
        }

        return base.OnExecute(app);
    }

    private void PrintPlugins(IEnumerable<PluginModel> plugins)
    {
        foreach (var plugin in plugins)
        {
            Console.WriteLine("{0} {1}", plugin.Name, plugin.Version);
        }
    }

    private void PrintPluginsDetails(IEnumerable<PluginModel> plugins)
    {
        var tableBuilder = new ConsoleTableBuilder()
            .AddColumn("Name", 30)
            .AddColumn("Version")
            .AddColumn("Description");

        foreach (var plugin in plugins)
        {
            tableBuilder.AddRow(plugin.Name, plugin.Version, plugin.Description);
        }

        tableBuilder.Write();
    }
}