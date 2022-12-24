using Automata.Core.Contracts.EventAggregator;
using System.Diagnostics;

namespace Automata.Services.EventAggregator;
public class Subscription<T> : ISubscription<T> where T : IMessage
{
    private readonly SynchronizationContext? _context;
    private readonly IMessenger.EventHandlerDelegate<T> _handler;

    public Subscription(IMessenger.EventHandlerDelegate<T> handler, ThreadOptions threadOptions, SynchronizationContext? context)
    {
        _handler = handler;
        _context = context;

        ThreadOptions = threadOptions;
    }

    public ThreadOptions ThreadOptions { get; }

    public void Invoke(T eventData)
    {
        switch (ThreadOptions)
        {
            case ThreadOptions.Publisher:
                ExecuteSafely(eventData);
                break;
            case ThreadOptions.UiThread:
                if (_context == null)
                    ThrowSynchronizationContextIsNullException();
                _context!.Post(_ => ExecuteSafely(eventData), null);
                break;
            case ThreadOptions.Background:
                Task.Run(() => ExecuteSafely(eventData));
                break;
        }
    }

    private void ExecuteSafely(T eventData)
    {
        try
        {
            _handler.Invoke(eventData);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    private void ThrowSynchronizationContextIsNullException()
    {
        throw new InvalidOperationException($"Could not invoke subscription on {nameof(ThreadOptions.UiThread)} without {nameof(SynchronizationContext)} set.");
    }
}
