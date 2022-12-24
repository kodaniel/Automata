using System.Text.RegularExpressions;

namespace Automata.Helpers;
public static class StringExtensions
{
    public static string ToTitleCase(this string value)
    {
        var words = Regex.Split(value, @"(?<!^)(?=[A-Z])");
        return string.Join(" ", words.Select(w => w.CapitalizeFirstLetter()));
    }

    public static string CapitalizeFirstLetter(this string value)
    {
        if (value.Length == 0)
            return value;
        else if (value.Length == 1)
            return char.ToUpper(value[0]).ToString();
        else
            return char.ToUpper(value[0]) + value.Substring(1);
    }
}
