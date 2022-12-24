using Automata.Plugins;
using Automata.Services;
using Automata.ViewModels.Base;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace Automata.ViewModels;

public class PluginsViewModel : ViewModelBase
{
    private readonly AutomataController _controller;

    public PluginsViewModel(AutomataController controller)
    {
        _controller = controller;
    }

    public ObservableCollection<PluginViewModel> Plugins =>
        _controller.Plugins.Select(plugin => new PluginViewModel(plugin)).ToObservableCollection();
}

public class PluginViewModel : ViewModelBase
{
    private readonly PluginModel _plugin;

    public PluginViewModel(PluginModel plugin)
    {
        _plugin = plugin;
    }

    public string? Name => _plugin.Name;

    public string? Description => _plugin.Description;

    public string? Version => _plugin.Version;

    public IEnumerable<BlockViewModel> Events => 
        _plugin.Blocks.Where(block => block.BlockType == BlockType.Event).Select(m => new BlockViewModel(m));

    public IEnumerable<BlockViewModel> Actions =>
        _plugin.Blocks.Where(block => block.BlockType == BlockType.Action).Select(m => new BlockViewModel(m));
}

public class BlockViewModel : ViewModelBase
{
    private readonly Block _block;

    public BlockViewModel(Block block)
    {
        _block = block;
    }

    public string Name => _block.Name;

    public string? Description => _block.Description;
}