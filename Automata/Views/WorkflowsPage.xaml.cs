using Automata.ViewModels;

using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

namespace Automata.Views;

public sealed partial class WorkflowsPage : Page
{
    public WorkflowsViewModel ViewModel
    {
        get;
    }

    public WorkflowsPage()
    {
        ViewModel = App.GetService<WorkflowsViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
