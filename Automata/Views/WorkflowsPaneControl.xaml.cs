using Automata.ViewModels;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Automata.Views;

public sealed partial class WorkflowsPaneControl : UserControl
{
    public WorkflowsViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty) as WorkflowsViewModel;
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", typeof(WorkflowsViewModel), typeof(WorkflowsPaneControl),
            new PropertyMetadata(null, OnViewModelPropertyChanged));

    private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WorkflowsPaneControl paneControl && paneControl.ViewModel is not null)
        {
            if (paneControl.ViewModel.HasCurrent)
                paneControl.WorkflowList.ScrollIntoView(paneControl.ViewModel.Current);
        }
    }

    public WorkflowsPaneControl()
    {
        InitializeComponent();
    }

    private void ListItemPointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (e.Pointer.PointerDeviceType is PointerDeviceType.Mouse or PointerDeviceType.Pen)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
        }
    }

    private void ListItemPointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (ViewModel is not null)
        {
            ViewModel.Filter = args.QueryText;
        }
    }
}
