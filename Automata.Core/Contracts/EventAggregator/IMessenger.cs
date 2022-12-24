namespace Automata.Core.Contracts.EventAggregator;
public interface IMessenger
{
    public delegate void EventHandlerDelegate<T>(T eventData)
        where T : IMessage;

    void ClearAllSubscriptions();

    void Publish<T>(T message)
        where T : IMessage;

    ISubscription<T> Subscribe<T>(EventHandlerDelegate<T> handler, ThreadOptions threadOptions = ThreadOptions.Publisher)
        where T : IMessage;

    bool Unsubscribe<T>(ISubscription<T> subscription)
        where T : IMessage;

    void SetMainThreadSynchronizationContext(SynchronizationContext context);
}