using Automata.Core.Models;
using Automata.ViewModels.Base;
using Automata.ViewModels.Validation;

namespace Automata.ViewModels;

public class BindableExprFieldViewModel<T> : ValidatableViewModelBase
{
    private readonly FieldArgs<T> _bindable;

    public BindableExprFieldViewModel(FieldArgs<T> bindable)
    {
        _bindable = bindable;
        _validation.Validator = SetupValidation();
    }

    public string? ContextKey
    {
        get => _bindable.ContextId;
        set => SetProperty(_bindable.ContextId, value, _bindable, (m, x) => m.ContextId = x);
    }

    public T? Value
    {
        get => _bindable.Value;
        set => SetProperty(_bindable.Value, value, _bindable, (m, x) => m.Value = x);
    }

    public string? Expression
    {
        get => _bindable.Expression;
        set => SetProperty(_bindable.Expression, value, _bindable, (m, x) => m.Expression = x);
    }

    private IValidator SetupValidation()
    {
        var ruleValidator = new RuleValidator();
        ruleValidator.AddStringLengthRule<BindableExprFieldViewModel<T>>(nameof(ContextKey), m => m.ContextKey, 0, 100, "Field name cannot exceed 100 chars.");
        ruleValidator.AddRegexRule<BindableExprFieldViewModel<T>>(nameof(ContextKey), m => m.ContextKey, "[a-zA-Z]", "Invalid format");

        return ruleValidator;
    }
}
