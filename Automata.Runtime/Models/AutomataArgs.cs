using System.Runtime.Serialization;
using Automata.Core.Contracts.Workflow;

namespace Automata.Runtime.Models;

[Serializable]
public class AutomataArgs : BaseArgs
{
    [DataMember]
    public List<WorkflowArgs> Workflows
    {
        get; set;
    }

    public AutomataArgs()
    {
        Workflows = new List<WorkflowArgs>();
    }

    public override object Clone()
    {
        var clone = (AutomataArgs)MemberwiseClone();
        clone.Workflows = Workflows.Select(x => (WorkflowArgs)x.Clone()).ToList();
        return clone;
    }
}
