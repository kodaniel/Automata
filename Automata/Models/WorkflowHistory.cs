using Automata.Core.Contracts.EventAggregator;
using Automata.Core.Models;

namespace Automata.Models;

public record WorkflowHistory(DateTime When, WorkflowArgs Workflow, WorkflowContext Context) : IMessage
{
}
