using Automata.ViewModels.Fields;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Automata.Views;

public class EditorTemplateSelector : DataTemplateSelector
{
    public DataTemplate StringEditorTemplate { get; set; }
    public DataTemplate IntegerEditorTemplate { get; set; }
    public DataTemplate DoubleEditorTemplate { get; set; }
    public DataTemplate BoolEditorTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        var selectedTemplate = base.SelectTemplateCore(item, container);

        if (item is StringEditor)
            selectedTemplate = StringEditorTemplate;
        else if (item is IntegerEditor)
            selectedTemplate = IntegerEditorTemplate;
        else if (item is DoubleEditor)
            selectedTemplate = DoubleEditorTemplate;
        else if (item is BoolEditor)
            selectedTemplate = BoolEditorTemplate;

        return selectedTemplate;
    }
}
