using System.Collections.ObjectModel;
using Automata.Contracts.Services;
using Automata.Core.Contracts.Workflow;
using Automata.Models;
using Automata.Plugins;

namespace Automata.Services;

public class AutomataController
{
    private const string AutomataArgsKey = "AppArgsRoot";

    private readonly PluginCollection _plugins;
    private readonly ILocalSettingsService _localSettingsService;

    public IEnumerable<PluginModel> Plugins => _plugins;
    public IEnumerable<Block> AllBlocks => _plugins.AllBlocks;
    public IEnumerable<Block> AllEvents => _plugins.AllEvents;
    public IEnumerable<Block> AllActions => _plugins.AllActions;

    public AutomataArgs Args { get; private set; } = new AutomataArgs();

    public AutomataController(IWorkflowService workflowService, ILocalSettingsService localSettingsService)
    {
        _plugins = new PluginCollection();
        _localSettingsService = localSettingsService;
    }

    public async Task SaveAsync()
    {
        await SaveArgsInSettingsAsync(Args);
    }

    public async Task InitializeAsync()
    {
        Args = await LoadArgsFromSettingsAsync();
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
        if (args != null)
        {
            return args;
        }

        return SampleAutomataData.GetSampleAutomata(); // new AutomataArgs();
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
