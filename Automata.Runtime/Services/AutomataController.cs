using System.Collections.ObjectModel;
using Automata.Core.Contracts.Workflow;
using Automata.Runtime.Contracts;
using Automata.Runtime.Models;

namespace Automata.Runtime.Services;

public class AutomataController
{
    private const string AutomataArgsKey = "AppArgsRoot";

    private readonly PluginCollection _plugins = new PluginCollection();
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IWorkflowService _workflowService;
    private readonly PluginService _pluginService;

    public IEnumerable<PluginModel> Plugins => _plugins;
    public IEnumerable<Block> AllBlocks => _plugins.AllBlocks;
    public IEnumerable<Block> AllEvents => _plugins.AllEvents;
    public IEnumerable<Block> AllActions => _plugins.AllActions;

    public AutomataArgs Args { get; private set; } = new AutomataArgs();

    public AutomataController(IWorkflowService workflowService, PluginService pluginService, ILocalSettingsService localSettingsService)
    {
        _workflowService = workflowService;
        _pluginService = pluginService;
        _localSettingsService = localSettingsService;
    }

    public async Task SaveAsync()
    {
        await SaveArgsInSettingsAsync(Args);
    }

    public async Task InitializeAsync()
    {
        foreach (var plugin in _pluginService.DiscoverPluginsInFolder(AppDomain.CurrentDomain.BaseDirectory))
        {
            AddPlugin(plugin);
        }

        Args = await LoadArgsFromSettingsAsync();

        foreach (var workflow in Args.Workflows)
        {
            _workflowService.AddWorkflow(workflow);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Add a plug-in to the Automata
    /// </summary>
    /// <param name="plugin">Plug-in</param>
    public void AddPlugin(PluginModel plugin)
    {
        _plugins.Add(plugin);
    }

    private async Task SaveArgsInSettingsAsync(AutomataArgs args)
    {
        await _localSettingsService.SaveSettingAsync(AutomataArgsKey, Args);
    }

    private async Task<AutomataArgs> LoadArgsFromSettingsAsync()
    {
        var args = await _localSettingsService.ReadSettingAsync<AutomataArgs>(AutomataArgsKey);
        if (args is not null)
        {
            return args;
        }

        return SampleAutomataData.GetSampleAutomata(); //new AutomataArgs();
    }

    #region PluginCollection

    private sealed class PluginCollection : Collection<PluginModel>
    {
        public IEnumerable<Block> AllBlocks => this.SelectMany(plugin => plugin.Blocks);

        public IEnumerable<Block> AllEvents =>
            AllBlocks.Where(block => typeof(BaseEventArgs).IsAssignableFrom(block.Type));

        public IEnumerable<Block> AllActions =>
            AllBlocks.Where(block => typeof(BaseActionArgs).IsAssignableFrom(block.Type));
    }

    #endregion
}
