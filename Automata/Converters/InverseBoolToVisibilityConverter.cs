using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Automata.Converters;

public class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is bool bValue && bValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility visibility && visibility == Visibility.Collapsed;
    }
}
