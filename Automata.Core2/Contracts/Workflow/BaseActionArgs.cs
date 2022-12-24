using Automata.Core.Models;

namespace Automata.Core.Contracts.Workflow;
public abstract class BaseActionArgs : BaseBlockArgs
{
    public abstract Task Execute(WorkflowContext context, CancellationToken cancellationToken);
}
