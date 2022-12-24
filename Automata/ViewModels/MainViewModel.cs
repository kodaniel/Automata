using Automata.Contracts.Services;
using Automata.Core.Contracts.EventAggregator;
using Automata.Models;
using Automata.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Automata.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IWorkflowService _workflowService;
    private readonly IMessenger _messenger;

    public MainViewModel(IWorkflowService workflowService, IMessenger messenger)
    {
        _workflowService = workflowService;
        _messenger = messenger;

        _messenger.Subscribe<WorkflowHistory>(OnWorkflowExecuted, ThreadOptions.UiThread);
    }

    private void OnWorkflowExecuted(WorkflowHistory workflowHistoryItem)
    {
        WorkflowHistory.Add(workflowHistoryItem);
    }

    public ObservableCollection<WorkflowHistory> WorkflowHistory { get; private set; } = new();

    public bool WorkflowsEnabled
    {
        get => _workflowService.IsRunning;
        set
        {
            if (_workflowService.IsRunning != value)
            {
                if (!_workflowService.IsRunning)
                    StartWorkflows();
                else
                    StopWorkflows();

                OnPropertyChanged();
            }
        }
    }

    private void StartWorkflows()
    {
        _workflowService.Start();
    }

    private void StopWorkflows()
    {
        _workflowService.Stop();
    }
}
