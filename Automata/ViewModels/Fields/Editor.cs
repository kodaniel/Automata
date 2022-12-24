using Automata.Core.Attributes;
using Automata.Core.Models;
using Automata.ViewModels.Base;
using System.ComponentModel;
using System.Reflection;

namespace Automata.ViewModels.Fields;

public abstract class Editor<T> : Editor
{
    protected Editor(PropertyInfo propertyInfo, FieldArgs<T> fieldArgs) : base(propertyInfo, fieldArgs)
    {
    }

    public new T? Value
    {
        get => base.Value is null ? default : (T)base.Value;
        set => base.Value = value;
    }
}

public abstract class Editor : ViewModelBase
{
    protected readonly PropertyInfo _propertyInfo;
    protected readonly FieldArgs _fieldArgs;

    public Editor(PropertyInfo propertyInfo, FieldArgs fieldArgs)
    {
        _propertyInfo = propertyInfo;
        _fieldArgs = fieldArgs;

        var fieldOptions = propertyInfo.GetCustomAttribute<FieldOptionsAttribute>();

        IsReadOnly = propertyInfo.GetCustomAttribute<ReadOnlyAttribute>() != null;
        Name = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? propertyInfo.Name.ToTitleCase();
        Tooltip = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
        Bindable = fieldOptions?.Bindable ?? false;
        AllowExpression = fieldOptions?.AllowExpression ?? false;
    }

    public object? Value
    {
        get => _fieldArgs.Value;
        set => SetProperty(_fieldArgs.Value, value, _fieldArgs, (m, x) => m.Value = x);
    }

    public string? ContextId
    {
        get => _fieldArgs.ContextId;
        set => SetProperty(_fieldArgs.ContextId, value, _fieldArgs, (m, x) => m.ContextId = x);
    }

    public string? Expression
    {
        get => _fieldArgs.Expression;
        set => SetProperty(_fieldArgs.Expression, value, _fieldArgs, (m, x) => m.Expression = x);
    }

    public bool IsExpression
    {
        get => _fieldArgs.IsExpression;
        set => SetProperty(_fieldArgs.IsExpression, value, _fieldArgs, (m, x) => m.IsExpression = x);
    }

    public string? Name { get; }

    public string? Tooltip { get; }

    public bool IsReadOnly { get; }

    public bool Bindable { get; }

    public bool AllowExpression { get; }
}

public sealed class StringEditor : Editor<string>
{
    public StringEditor(PropertyInfo propertyInfo, FieldArgs<string> fieldArgs) : base(propertyInfo, fieldArgs)
    {
    }
}

public sealed class IntegerEditor : Editor<int>
{
    public IntegerEditor(PropertyInfo propertyInfo, FieldArgs<int> fieldArgs) : base(propertyInfo, fieldArgs)
    {
    }
}

public sealed class DoubleEditor : Editor<double>
{
    public DoubleEditor(PropertyInfo propertyInfo, FieldArgs<double> fieldArgs) : base(propertyInfo, fieldArgs)
    {
    }
}

public sealed class BoolEditor : Editor<bool>
{
    public BoolEditor(PropertyInfo propertyInfo, FieldArgs<bool> fieldArgs) : base(propertyInfo, fieldArgs)
    {
    }
}