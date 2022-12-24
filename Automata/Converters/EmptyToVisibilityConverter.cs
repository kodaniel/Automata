using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Automata.Converters;

public class EmptyToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return string.IsNullOrEmpty(value?.ToString()) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility visibility && visibility == Visibility.Visible;
    }
}
