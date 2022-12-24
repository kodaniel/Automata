using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Automata.Core.Attributes;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;
using Automata.Core.Services;
using Automata.Plugin.Basics.Models;

namespace Automata.Plugin.Basics.Actions;

[Description("Triggers every messages with the same Id.")]
public class MessageSender : BaseActionArgs
{
    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> MessageId { get; set; }

    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> Message { get; set; }

    [Description("Stop workflow after trigger if true.")]
    [FieldOptions(AllowExpression = true)]
    public FieldArgs<bool> HandleTrigger { get; set; }

    public MessageSender()
    {
        MessageId = new FieldArgs<string>();
        Message = new FieldArgs<string>();
        HandleTrigger = new FieldArgs<bool>();
    }

    public override Task Execute(WorkflowContext context, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(MessageId.Evaluate(context)))
        {
            context.Handled = HandleTrigger.Evaluate(context);

            var message = new TriggerMessage(MessageId.Evaluate(context), Message.Evaluate(context));
            //CoreServices.Instance.Messenger.Publish(message);
            Debug.WriteLine($"Raise trigger '{message.Id}' with parameter '{message.Message}'.");
        }

        return Task.CompletedTask;
    }

    public override object Clone()
    {
        var clone = (MessageSender)MemberwiseClone();
        clone.MessageId = (FieldArgs<string>)MessageId.Clone();
        clone.Message = (FieldArgs<string>)Message.Clone();

        return clone;
    }
}
