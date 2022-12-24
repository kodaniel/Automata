#nullable enable

using Automata.Core.Models;

namespace Automata.Core.Contracts.Workflow;

public abstract class BaseEventArgs : BaseBlockArgs, IDisposable
{
    public delegate void WorkflowEventCallback(WorkflowContext context);

    protected WorkflowEventCallback? workflowEventCallback;

    public virtual void StartListener(WorkflowEventCallback callback)
    {
        workflowEventCallback = callback;
    }
    
    public virtual void StopListener()
    {
        workflowEventCallback = null;
    }
    
    public virtual void Dispose()
    {
        StopListener();
    }
}
