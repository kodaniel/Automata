using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;

namespace Automata.Models;

public class WorkflowArgs : BaseArgs
{
    public string Name { get; set; } = default!;

    public bool IsEnabled { get; set; }

    public DateTime? LastTriggered { get; set; }

    public BaseEventArgs? Event { get; set; }

    public List<BaseActionArgs> Actions { get; set; } = new List<BaseActionArgs>();

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
        var clone = (WorkflowArgs)MemberwiseClone();
        clone.Event = (BaseEventArgs?)Event?.Clone();
        clone.Actions = Actions.Select(x => (BaseActionArgs)x.Clone()).ToList();
        return clone;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
