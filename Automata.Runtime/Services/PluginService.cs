using System.ComponentModel;
using System.Reflection;
using System.Runtime.Loader;
using Automata.Core.Contracts;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;
using Automata.Runtime.Models;

namespace Automata.Runtime.Services;

public sealed class PluginService
{
    public IEnumerable<PluginModel> DiscoverPluginsInFolder(string pluginFolderPath)
    {
        Guard.AgainstEmpty(pluginFolderPath);

        foreach (var dllFile in Directory.GetFiles(pluginFolderPath, "Automata.Plugin.*.dll"))
        {
            AssemblyLoadContext assemblyLoadContext = new AssemblyLoadContext(dllFile);
            Assembly assembly = assemblyLoadContext.LoadFromAssemblyPath(dllFile);
            PluginModel? plugin = GetPluginFromAssembly(assembly);
            
            if (plugin is not null)
                yield return plugin;
        }
    }

    public PluginModel? GetPluginFromAssembly(Assembly assembly)
    {
        var plugin = CreatePluginModel(assembly);

        if (plugin == null)
            return null;

        plugin.Blocks = GetBuildingBlocks(assembly);

        return plugin;
    }

    private PluginModel? CreatePluginModel(Assembly assembly)
    {
        var pluginType = assembly.GetTypes().FirstOrDefault(x => typeof(IPlugin).IsAssignableFrom(x));

        if (pluginType == null)
            return null;

        var plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
        plugin.OnInitializing();
        
        var name = pluginType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        var description = pluginType.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return new PluginModel
        {
            Name = name ?? assembly.GetName().Name!,
            Description = description ?? assembly.FullName!,
            Version = assembly.GetName().Version?.ToString() ?? "1.0.0.0",
        };
    }

    private IEnumerable<Block> GetBuildingBlocks(Assembly assembly)
    {
        var buildingBlockTypes = new List<Type>();
        buildingBlockTypes.AddRange(GetBuildingBlockTypes<BaseEventArgs>(assembly));
        buildingBlockTypes.AddRange(GetBuildingBlockTypes<BaseActionArgs>(assembly));

        return buildingBlockTypes.Select(t => CreateBuildingBlock(t));
    }

    private Block CreateBuildingBlock(Type type) => new(type);

    private IEnumerable<Type> GetBuildingBlockTypes<T>(Assembly assembly) =>
        assembly.GetExportedTypes()
            .Where(t => typeof(T).IsAssignableFrom(t))
            .Where(t => t.IsPublic && t.IsClass && !t.IsAbstract);
}
