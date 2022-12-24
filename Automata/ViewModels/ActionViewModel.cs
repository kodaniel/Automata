using Automata.Core.Contracts.Workflow;
using Automata.ViewModels.Base;
using Automata.ViewModels.Fields;

namespace Automata.ViewModels;

public class ActionViewModel : ViewModelBase
{
    private readonly BaseActionArgs _args;

    public ActionViewModel(BaseActionArgs args)
    {
        _args = args;

        Name = args.GetType().Name.ToTitleCase();
        Fields = EditorFactory.CreateFields(_args);
    }

    public IEnumerable<Editor> Fields { get; }

    public string Name { get; }
}
