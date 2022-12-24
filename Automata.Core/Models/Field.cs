using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;

namespace Automata.Core.Models;

#nullable enable

public abstract class FieldArgs : BaseArgs
{
    public object? Value { get; set; }

    public string? ContextId { get; set; }

    public string? Expression { get; set; }

    public bool IsExpression { get; set; }

    public object? Evaluate(WorkflowContext context)
    {
        if (!IsExpression)
            return Value;

        return ExpressionEvaluation.Evaluate<object>(Expression, context);
    }
}

public class FieldArgs<T> : FieldArgs
{
    public FieldArgs()
    {
    }

    public FieldArgs(T initialValue)
    {
        Value = initialValue;
    }

    public new T? Value
    {
        get => base.Value is null ? default : (T)base.Value;
        set => base.Value = value;
    }

    public new T? Evaluate(WorkflowContext context)
    {
        if (!IsExpression)
            return Value;

        return ExpressionEvaluation.Evaluate<T>(Expression, context);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}
