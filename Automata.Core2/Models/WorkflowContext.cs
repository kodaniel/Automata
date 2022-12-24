using Automata.Core.Helpers;
using System.Collections.ObjectModel;

namespace Automata.Core.Models;
public class WorkflowContext
{
    private readonly Dictionary<string, object> _values;

    public WorkflowContext()
        : this(new Dictionary<string, object>())
    {
    }

    public WorkflowContext(Dictionary<string, object> initialValues)
    {
        _values = initialValues;
    }

    public object this[string field]
    {
        get => this.Read<object>(field);
        set => this.Write(field, value);
    }

    public bool Handled { get; set; }

    public int Count => _values.Count;

    internal Dictionary<string, object> Values => _values;

    public IReadOnlyDictionary<string, object> GetValues()
    {
        return new ReadOnlyDictionary<string, object>(_values);
    }
}
