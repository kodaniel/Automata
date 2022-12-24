using Automata.Core.Models;

namespace Automata.Core.Helpers;
public static class WorkflowContextExtensions
{
    /// <summary>
    /// Read the value with the given field name from the context.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <param name="context">Workflow context.</param>
    /// <param name="field">Field name.</param>
    /// <returns>Returns the value with the given type, or a default value if couldn't convert.</returns>
    public static T Read<T>(this WorkflowContext context, string field)
    {
        if (context.TryConvertInternal(field, out T value))
            return value;

        return default;
    }

    public static bool TryRead<T>(this WorkflowContext context, string field, out T value)
    {
        value = default;

        if (context.Is<T>(field))
            return context.TryConvertInternal(field, out value);

        return false;
    }

    /// <summary>
    /// Write value to context.
    /// </summary>
    /// <param name="context">Workflow context.</param>
    /// <param name="field">Field name.</param>
    /// <param name="value">Value.</param>
    public static void Write(this WorkflowContext context, string field, object value)
    {
        Guard.AgainstEmpty(field);

        context.Values[field] = value;
    }

    /// <summary>
    /// Write value to context only if the field value is null.
    /// </summary>
    /// <param name="context">Workflow context.</param>
    /// <param name="field">Field name.</param>
    /// <param name="value">Value.</param>
    public static void WriteOnlyIfNull(this WorkflowContext context, string field, object value)
    {
        Guard.AgainstEmpty(field);

        context.Values[field] ??= value;
    }

    /// <summary>
    /// Remove field from the context.
    /// </summary>
    /// <param name="context">Workflow context.</param>
    /// <param name="field">Field name.</param>
    public static void Remove(this WorkflowContext context, string field)
    {
        context.Values.Remove(field);
    }

    /// <summary>
    /// Checks a field's value in the context.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="context">Workflow context.</param>
    /// <param name="field">Field name.</param>
    /// <returns><see cref="true"/> if the value type is the same as <typeparamref name="T"/>, <see cref="false"/> otherwise.</returns>
    public static bool Is<T>(this WorkflowContext context, string field)
    {
        if (!context.Values.ContainsKey(field))
            return false;

        return context.Values[field] is T;
    }

    private static bool TryConvertInternal<T>(this WorkflowContext context, string field, out T value)
    {
        value = default;

        try
        {
            if (context.Values.ContainsKey(field))
            {
                if (typeof(T) == typeof(object))
                    value = (T)context.Values[field];
                else
                    value = (T)Convert.ChangeType(context.Values[field], typeof(T));
            }

            return true;
        }
        catch
        {
        }

        return false;
    }
}
