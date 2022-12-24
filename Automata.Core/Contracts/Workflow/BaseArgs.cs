namespace Automata.Core.Contracts.Workflow;
public abstract class BaseArgs : ICloneable
{
    // Parameterless ctor is required for serialization
    protected BaseArgs()
    {

    }

    public abstract object Clone();
}
