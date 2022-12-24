using Automata.Core.Contracts.Workflow;
using Automata.Core.Helpers;
using Automata.Core.Models;
using NCrontab;
using System;
using System.ComponentModel;
using System.Threading;

namespace Automata.Plugin.Basics.Events;

public class Scheduler : BaseEventArgs
{
    private Timer _timer;

    [Description("CRON scheduler")]
    public FieldArgs<string> Cron { get; set; }

    public override void StartListener(WorkflowEventCallback callback)
    {
        base.StartListener(callback);

        _timer = new Timer(TimerElapsedCallback);

        WaitForNextOccurence();
    }

    public override void StopListener()
    {
        base.StopListener();

        _timer?.Dispose();
    }

    private void WaitForNextOccurence()
    {
        try
        {
            var now = DateTime.Now;
            var schedule = CrontabSchedule.Parse(Cron.Value);
            var nextOccurence = schedule.GetNextOccurrence(now);

            _timer.Change((int)(nextOccurence - now).TotalMilliseconds, Timeout.Infinite);
        }
        catch
        {
        }
    }

    private void TimerElapsedCallback(object state)
    {
        var context = new WorkflowContext();
        context.Write("time", DateTime.Now.ToString());

        workflowEventCallback?.Invoke(context);

        WaitForNextOccurence();
    }

    public override object Clone()
    {
        var clone = (Scheduler)MemberwiseClone();
        clone.Cron = (FieldArgs<string>)Cron.Clone();

        return clone;
    }
}
