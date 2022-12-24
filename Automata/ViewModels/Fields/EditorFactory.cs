using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;
using System.Reflection;

namespace Automata.ViewModels.Fields;

public static class EditorFactory
{
    public static IEnumerable<Editor> CreateFields<T>(T instance)
        where T : IWorkflowBlock
    {
        var editors = new List<Editor>();
        var props = GetBrowsableProperties(instance.GetType());

        foreach (var property in props)
        {
            var field = (FieldArgs)property.GetValue(instance, null)!;
            var editor = CreatePropertyFieldViewModel(property, field);
            if (editor != null)
            {
                editors.Add(editor);
            }
        }

        return editors;
    }

    private static IEnumerable<PropertyInfo> GetBrowsableProperties(Type type)
    {
        var properties = type.GetProperties().Where(p => typeof(FieldArgs).IsAssignableFrom(p.PropertyType));

        return properties;
    }

    private static Editor? CreatePropertyFieldViewModel(PropertyInfo property, FieldArgs args)
    {
        if (property.PropertyType == typeof(FieldArgs<string>))
            return new StringEditor(property, (FieldArgs<string>)args);
        if (property.PropertyType == typeof(FieldArgs<int>))
            return new IntegerEditor(property, (FieldArgs<int>)args);
        if (property.PropertyType == typeof(FieldArgs<double>))
            return new DoubleEditor(property, (FieldArgs<double>)args);
        if (property.PropertyType == typeof(FieldArgs<bool>))
            return new BoolEditor(property, (FieldArgs<bool>)args);

        return null;
    }
}
