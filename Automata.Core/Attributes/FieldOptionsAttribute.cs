namespace Automata.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FieldOptionsAttribute : Attribute
{
    private bool _bindable;
    private bool _allowExpression;

    public FieldOptionsAttribute()
    {
        _bindable = false;
        _allowExpression = false;
    }

    public bool Bindable
    {
        get => _bindable;
        set => _bindable = value;
    }

    public bool AllowExpression
    {
        get => _allowExpression;
        set => _allowExpression = value;
    }
}
