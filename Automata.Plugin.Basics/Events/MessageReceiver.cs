using Automata.Core.Contracts.EventAggregator;
using System.ComponentModel;
using System.Diagnostics;
using Automata.Core.Contracts.Workflow;
using Automata.Plugin.Basics.Models;
using Automata.Core.Services;
using Automata.Core.Models;
using Automata.Core.Attributes;

namespace Automata.Plugin.Basics.Events;
public class MessageReceiver : BaseEventArgs
{
    private ISubscription<TriggerMessage> _subscription;

    [DisplayName("Message unique ID")]
    [Description("Event will be raised when the message ID fires.")]
    [FieldOptions(Bindable = true, AllowExpression = false)]
    public FieldArgs<string> MessageId { get; set; }

    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> Message { get; set; }

    public MessageReceiver()
    {
        MessageId = new FieldArgs<string>();
        Message = new FieldArgs<string>();
    }

    public override void StartListener(WorkflowEventCallback callback)
    {
        base.StartListener(callback);

        //_subscription = CoreServices.Instance.Messenger.Subscribe<TriggerMessage>(RaiseEvent);
    }

    public override void StopListener()
    {
        base.StopListener();

        //if (_subscription != null)
        //    CoreServices.Instance.Messenger.Unsubscribe(_subscription);
    }

    private void RaiseEvent(TriggerMessage message)
    {
        if (MessageId.Value == message.Id)
        {
            Message.Value = message.Message;
            Debug.WriteLine($"{MessageId.Value} has been triggered. Message = {Message.Value}");

            workflowEventCallback?.Invoke(new WorkflowContext());
        }
    }

    public override object Clone()
    {
        var clone = (MessageReceiver)MemberwiseClone();
        clone.MessageId = (FieldArgs<string>)MessageId.Clone();
        clone.Message = (FieldArgs<string>)Message.Clone();

        return clone;
    }
}
