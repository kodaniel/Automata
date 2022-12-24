using Automata.Core.Attributes;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;
using Automata.Core.Models;
using System.Reflection;

namespace Automata.Runtime.Helpers;

public static class WorkflowContextHelper
{
    public static void WritePropertiesToContext(this IWorkflowBlock instance, WorkflowContext context)
    {
        var type = instance.GetType();
        var properties = type.GetProperties();

        foreach (var propertyInfo in properties)
        {
            var field = propertyInfo.GetValue(instance, null) as FieldArgs;
            if (field != null)
            {
                var fieldOptions = propertyInfo.GetCustomAttribute<FieldOptionsAttribute>();
                var bindable = fieldOptions?.Bindable ?? false;
                var allowExpression = fieldOptions?.AllowExpression ?? false;

                if (bindable && !string.IsNullOrWhiteSpace(field.ContextId))
                {
                    var value = allowExpression && field.IsExpression ? field.Evaluate(context) : field.Value;
                    context.Write(field.ContextId, value);
                }
            }
        }
    }
}
