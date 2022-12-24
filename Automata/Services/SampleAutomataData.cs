using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;
using Automata.Models;
using Automata.Plugin.Basics.Actions;
using Automata.Plugin.Basics.Events;
using Automata.Plugin.IO.Events;

namespace Automata.Services;
public class SampleAutomataData
{
    public static AutomataArgs GetSampleAutomata()
    {
        return new AutomataArgs
        {
            Workflows = GetSampleWorkflows().ToList()
        };
    }

    private static IEnumerable<WorkflowArgs> GetSampleWorkflows()
    {
        return new List<WorkflowArgs>
        {
            new WorkflowArgs
            {
                Name = "trigger proxy",
                IsEnabled = true,
                LastTriggered = DateTime.Now,
                Event = new MessageReceiver
                {
                    MessageId = new FieldArgs<string>
                    {
                        Value = "trig-A"
                    }
                },
                Actions = new List<BaseActionArgs>
                {
                    new ConfirmationDialog
                    {
                        Title = new FieldArgs<string>
                        {
                            Value = "Dialog"
                        },
                        Message = new FieldArgs<string>
                        {
                            Value = "Are you sure?"
                        },
                        AcceptData = new FieldArgs<string>
                        {
                            ContextId = "dialog.accept",
                            IsExpression = true,
                            Expression = "igen",
                            Value = "yes"
                        },
                        CancelData = new FieldArgs<string>
                        {
                            Value = "no"
                        }
                    },
                    new MessageSender
                    {
                        MessageId = new FieldArgs<string>
                        {
                            Value = "trig-B"
                        },
                        Message = new FieldArgs<string>
                        {
                            IsExpression = true,
                            Expression = "{dialog.accept}"
                        }
                    }
                }
            },
            new WorkflowArgs
            {
                Name = "logger",
                IsEnabled = true,
                LastTriggered = DateTime.Now,
                Event = new MessageReceiver
                {
                    MessageId = new FieldArgs<string>
                    {
                        Value = "trig-B"
                    },
                    Message = new FieldArgs<string>
                    {
                        ContextId = "trigger.param"
                    }
                },
                Actions = new List<BaseActionArgs>
                {
                    new Logger
                    {
                        Severity = Severity.Info,
                        Message = new FieldArgs<string>
                        {
                            IsExpression = true,
                            Expression = "The 'logger' workflow has been triggered. {trigger.param}"
                        }
                    }
                }
            },
            new WorkflowArgs
            {
                Name = "Monitor download folder",
                IsEnabled = true,
                LastTriggered = DateTime.Now,
                Event = new FileSystemMonitor
                {
                    Folder = new FieldArgs<string>
                    {
                        Value = @"D:\Work\Automata"
                    }
                },
                Actions = new List<BaseActionArgs>
                {
                    new Logger
                    {
                        Severity = Severity.Info,
                        Message = new FieldArgs<string>
                        {
                            IsExpression = true,
                            Expression = "A file changed in '{filename}'."
                        }
                    }
                }
            },
            new WorkflowArgs
            {
                Name = "Scheduler",
                IsEnabled = true,
                LastTriggered = DateTime.Now,
                Event = new Scheduler
                {
                    Cron = new FieldArgs<string>
                    {
                        Value = "* * * * *"
                    }
                },
                Actions = new List<BaseActionArgs>
                {
                    new Logger
                    {
                        Severity = Severity.Info,
                        Message = new FieldArgs<string>
                        {
                            IsExpression = true,
                            Expression = "Scheduled job run at '{time}'."
                        }
                    }
                }
            }
        };
    }
}
