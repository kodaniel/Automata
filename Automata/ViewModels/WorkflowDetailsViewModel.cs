using Automata.Models;
using Automata.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Automata.ViewModels;

public class WorkflowDetailsViewModel : ViewModelBase
{
    private readonly WorkflowArgs _workflowArgs;

    public WorkflowDetailsViewModel(WorkflowArgs workflow)
    {
        _workflowArgs = workflow;

        Initialize();
    }

    public EventViewModel? Event { get; set; }

    public ObservableCollection<ActionViewModel> Actions { get; } = new();

    public string? Name
    {
        get => _workflowArgs.Name;
        set => SetProperty(_workflowArgs.Name, value, _workflowArgs, (m, x) => m.Name = x);
    }

    public WorkflowArgs GetModel() => _workflowArgs;

    private void Initialize()
    {
        if (_workflowArgs.Event != null)
            Event = new EventViewModel(_workflowArgs.Event);

        _workflowArgs.Actions.ForEach(actionArg =>
        {
            Actions.Add(new ActionViewModel(actionArg));
        });
    }
}
