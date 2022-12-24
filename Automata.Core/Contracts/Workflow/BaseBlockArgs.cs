using System.Runtime.Serialization;

namespace Automata.Core.Contracts.Workflow;
public abstract class BaseBlockArgs : BaseArgs, IWorkflowBlock
{
    /// <inheritdoc/>
    [DataMember(IsRequired = true)]
    public string UniqueId { get; set; }

    /// <inheritdoc/>
    public bool IsEquals(IWorkflowBlock other)
    {
        return UniqueId == other.UniqueId;
    }

    //public virtual IValidator SetupValidator()
    //{
    //    return NullValidator.Instance;
    //}
}
