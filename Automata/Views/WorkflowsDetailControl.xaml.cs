using Automata.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Automata.Views;

public sealed partial class WorkflowsDetailControl : UserControl
{
    public WorkflowDetailsViewModel? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as WorkflowDetailsViewModel;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = 
        DependencyProperty.Register("ListDetailsMenuItem", typeof(WorkflowDetailsViewModel), typeof(WorkflowsDetailControl), 
            new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public WorkflowsDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WorkflowsDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
