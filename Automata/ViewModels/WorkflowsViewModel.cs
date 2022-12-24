using System.Collections.ObjectModel;
using System.Windows.Input;
using Automata.Contracts.Services;
using Automata.Contracts.ViewModels;
using Automata.Core.Services;
using Automata.Models;
using Automata.Plugin.Basics.Models;
using Automata.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Automata.ViewModels;

public class WorkflowsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IWorkflowService _workflowService;
    private readonly AutomataController _controller;
    private WorkflowDetailsViewModel? _selected;
    private WorkflowDetailsViewModel? _activeView;
    private string _filterText = string.Empty;

    public WorkflowDetailsViewModel? Current
    {
        get => _selected;
        set
        {
            if (SetProperty(ref _selected, value))
            {
                OnPropertyChanged(nameof(HasCurrent));

                if (_selected is not null)
                {
                    SetActiveView(_selected.GetModel());
                }
            }
        }
    }

    public bool HasCurrent => Current is not null;

    public WorkflowDetailsViewModel? ActiveView
    {
        get => _activeView;
        private set => SetProperty(ref _activeView, value);
    }

    public string Filter
    {
        get => _filterText;
        set
        {
            var current = Current;

            if (SetProperty(ref _filterText, value))
            {
                OnPropertyChanged(nameof(Items));

                if (current is not null && Items.Contains(current))
                {
                    Current = current;
                }
            }
        }
    }

    private ObservableCollection<WorkflowDetailsViewModel> _allWorkflows = new ObservableCollection<WorkflowDetailsViewModel>();
    public ObservableCollection<WorkflowDetailsViewModel> Items =>
        Filter is null ? _allWorkflows : new ObservableCollection<WorkflowDetailsViewModel>(_allWorkflows.Where(i => ApplyFilter(i, Filter)));

    public ICommand AddWorkflowCommand { get; }

    public WorkflowsViewModel(IWorkflowService workflowService, AutomataController controller)
    {
        _workflowService = workflowService;
        _controller = controller;

        AddWorkflowCommand = new RelayCommand(AddWorkflow);

        _workflowService.AddWorkflow(_controller.Args.Workflows[0]);
        _workflowService.AddWorkflow(_controller.Args.Workflows[1]);
        _workflowService.AddWorkflow(_controller.Args.Workflows[2]);
        _workflowService.AddWorkflow(_controller.Args.Workflows[3]);
        _workflowService.Start();

        CoreServices.Instance.Messenger.Publish(new TriggerMessage("trig-A", ""));
    }

    public void SetActiveView(WorkflowArgs workflow)
    {
        ActiveView = new WorkflowDetailsViewModel((WorkflowArgs)workflow.Clone());
    }

    private void AddWorkflow()
    {
        SetActiveView(CreateDefaultWorkflow());
    }

    public void OnNavigatedTo(object parameter)
    {
        _allWorkflows = _controller.Args.Workflows
            .Select(workflow => new WorkflowDetailsViewModel(workflow))
            .ToObservableCollection();

        OnPropertyChanged(nameof(Items));
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (Current == null && Items.Any())
        {
            Current = Items.First();
        }
    }

    private WorkflowArgs CreateDefaultWorkflow()
    {
        return new WorkflowArgs
        {
            Name = $"New workflow #{_allWorkflows.Count() + 1}",
        };
    }

    private bool ApplyFilter(WorkflowDetailsViewModel workflow, string filter)
    {
        return workflow.Name?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ?? false;
    }
}
