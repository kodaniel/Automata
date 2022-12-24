namespace Automata.Core.Contracts.Workflow;

public interface IWorkflowBlock
{
    /// <summary>
    /// A globally unique id which helps to identify the object during serialization.
    /// Do not modify, otherwise the serialization will be corrupted.
    /// </summary>
    string UniqueId { get; }

    /// <summary>
    /// Compares two <see cref="IWorkflowBlock"/> by their <see cref="UniqueId"/>.
    /// </summary>
    /// <param name="other">The other object.</param>
    /// <returns><see langword="true"/> if same, <see langword="false"/> otherwise.</returns>
    bool IsEquals(IWorkflowBlock other);

    //IValidator SetupValidator();
}