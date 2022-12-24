using Automata.Core.Contracts.Workflow;
using Automata.Core.Models;
using Automata.Plugin.Basics.Actions;
using Automata.Plugin.Basics.Events;
using Automata.Plugin.IO.Events;
using Automata.Daemon.Models;

namespace Automata.Daemon.Services;
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
                Id = new Guid("31ed436c-09b4-4cc9-bbd5-3495edfd7a02"),
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
                Id = new Guid("8fb38ffc-0c7e-41b4-9fa7-d777bb69281a"),
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
                Id = new Guid("90e3f54e-318f-4efd-85a0-9803a5731831"),
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
                Id = new Guid("7998dd02-e82d-4398-9946-c392e48add8b"),
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
