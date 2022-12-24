using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Automata.Core.Attributes;
using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;

namespace Automata.Plugin.Basics.Actions;
public enum Severity
{
    Info,
    Debug,
    Warning,
    Error,
    Fatal
}

public class Logger : BaseActionArgs
{
    [FieldOptions(Bindable = true, AllowExpression = true)]
    public FieldArgs<string> Message { get; set; }

    public Severity Severity { get; set; }

    public Logger()
    {
        Message = new FieldArgs<string>();
    }

    public override Task Execute(WorkflowContext context, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {Severity}: {Message.Evaluate(context)}");

        return Task.CompletedTask;
    }

    public override object Clone()
    {
        var clone = (Logger)MemberwiseClone();
        clone.Message = (FieldArgs<string>)Message;

        return clone;
    }
}
