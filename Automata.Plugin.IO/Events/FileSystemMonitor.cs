using Automata.Core.Attributes;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;
using Automata.Core.Models;
using System.ComponentModel;
using System.IO;

namespace Automata.Plugin.IO.Events;

public class FileSystemMonitor : BaseEventArgs
{
    private FileSystemWatcher? watcher;

    [FieldOptions(Bindable = true, AllowExpression = false)]
    public FieldArgs<string> Folder { get; set; }

    [FieldOptions(Bindable = true, AllowExpression = false)]
    public FieldArgs<string> Filter { get; set; }

    [DisplayName("Sub-directories")]
    [Description("Watch for sub-directories too.")]
    public FieldArgs<bool> SubDirectories { get; set; }

    public FieldArgs<bool> EnableCreated { get; set; }

    public FieldArgs<bool> EnableDeleted { get; set; }

    public FieldArgs<bool> EnableChanged { get; set; }

    public FieldArgs<bool> EnableRenamed { get; set; }

    public FileSystemMonitor()
    {
        Folder = new FieldArgs<string>();
        Filter = new FieldArgs<string>("*.*");
        SubDirectories = new FieldArgs<bool>(false);
        EnableCreated = new FieldArgs<bool>(true);
        EnableDeleted = new FieldArgs<bool>(true);
        EnableChanged = new FieldArgs<bool>(true);
        EnableRenamed = new FieldArgs<bool>(true);
    }

    public override void StartListener(WorkflowEventCallback callback)
    {
        base.StartListener(callback);

        if (!Directory.Exists(Filter.Value))
            return;

        watcher = new FileSystemWatcher();
        watcher.Path = Folder.Value;
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;
        watcher.IncludeSubdirectories = SubDirectories.Value;

        if (EnableCreated.Value)
            watcher.Created += FileSystemWatcherCallback;
        if (EnableDeleted.Value)
            watcher.Deleted += FileSystemWatcherCallback;
        if (EnableChanged.Value)
            watcher.Changed += FileSystemWatcherCallback;
        if (EnableRenamed.Value)
            watcher.Renamed += FileSystemWatcherCallback;

        watcher.Filter = Filter.Value;
        watcher.EnableRaisingEvents = true;
    }

    public override void StopListener()
    {
        base.StopListener();

        watcher?.Dispose();
    }

    private void FileSystemWatcherCallback(object sender, FileSystemEventArgs e)
    {
        var context = new WorkflowContext();
        context.Write("filename", e.Name);
        context.Write("filepath", e.FullPath);
        context.Write("changetype", e.ChangeType);

        workflowEventCallback?.Invoke(context);
    }

    public override object Clone()
    {
        var clone = (FileSystemMonitor)MemberwiseClone();
        clone.Folder = (FieldArgs<string>)Folder.Clone();
        clone.Filter = (FieldArgs<string>)Filter.Clone();
        clone.EnableCreated = (FieldArgs<bool>)EnableCreated.Clone();

        return clone;
    }
}
