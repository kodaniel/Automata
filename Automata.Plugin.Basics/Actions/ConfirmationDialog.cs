using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Automata.Core.Attributes;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;

namespace Automata.Plugin.Basics.Actions;
public class ConfirmationDialog : BaseActionArgs
{
    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> Title { get; set; }

    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> Message { get; set; }

    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> AcceptData { get; set; }

    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> CancelData { get; set; }

    public bool HandleOnCancel { get; set; }

    public ConfirmationDialog()
    {
        Title = new FieldArgs<string>();
        Message = new FieldArgs<string>();
        AcceptData = new FieldArgs<string>();
        CancelData = new FieldArgs<string>();
    }

    public override Task Execute(WorkflowContext context, CancellationToken cancellationToken)
    {
        Debug.WriteLine("Pop-up confirmation dialog, accepting by default.");
        var result = true; // TODO: Show dialog and return the result

        //context.Write("ConfirmationDialog.Result", result ? AcceptData : CancelData);
        context.Handled = HandleOnCancel;

        return Task.CompletedTask;
    }

    public override object Clone()
    {
        var clone = (ConfirmationDialog)MemberwiseClone();
        clone.Title = (FieldArgs<string>)Title.Clone();
        clone.Message = (FieldArgs<string>)Message.Clone();
        clone.AcceptData = (FieldArgs<string>)AcceptData.Clone();
        clone.CancelData = (FieldArgs<string>)CancelData.Clone();

        return clone;
    }
}
