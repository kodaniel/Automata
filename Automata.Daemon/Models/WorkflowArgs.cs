using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;

namespace Automata.Daemon.Models;

public class WorkflowArgs : BaseArgs, IDisposable
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    public DateTime? LastTriggered { get; set; }

    public BaseEventArgs? Event { get; set; }

    public List<BaseActionArgs> Actions { get; set; } = new List<BaseActionArgs>();

    public WorkflowArgs()
    {
        
    }

    public virtual void StartListening(Action<WorkflowArgs, WorkflowContext> callback)
    {
        Event?.StartListener(ctx => callback(this, ctx));
    }

    public virtual void StopListening()
    {
        Event?.StopListener();
    }

    public override object Clone()
    {
        throw new InvalidOperationException("Workflow can't be cloned.");
        //var clone = (WorkflowArgs)MemberwiseClone();
        //clone.Id = Guid.NewGuid();
        //clone.Event = (BaseEventArgs?)Event?.Clone();
        //clone.Actions = Actions.Select(x => (BaseActionArgs)x.Clone()).ToList();
        //return clone;
    }

    public void Dispose()
    {
        StopListening();
    }
}
