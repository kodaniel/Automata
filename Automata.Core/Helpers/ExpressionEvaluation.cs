using Automata.Core.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace Automata.Core.Helpers;

#nullable enable

internal static class ExpressionEvaluation
{
    public static T? Evaluate<T>(string? expression, WorkflowContext context)
    {
        try
        {
            if (expression == null)
                return default;

            var sb = new StringBuilder(expression);
            var matches = Regex.Matches(expression, @"\{([a-zA-Z_][a-zA-Z0-9._]*)\}");
            foreach (Match match in matches)
            {
                var newValue = string.Empty;
                if (context.TryRead<T>(match.Groups[1].Value.Trim(), out var ctxValue) && ctxValue != null)
                    newValue = ctxValue.ToString();

                sb.Replace(match.Groups[0].Value, newValue);
            }

            return (T)Convert.ChangeType(sb.ToString(), typeof(T));
        }
        catch
        {
            return default;
        }
    }
}