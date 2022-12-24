using Automata.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Automata.Views;

public sealed partial class WorkflowsPage2 : Page
{
    public WorkflowsViewModel ViewModel { get; }

    public WorkflowsPage2()
    {
        ViewModel = App.GetService<WorkflowsViewModel>();

        InitializeComponent();
    }
}
