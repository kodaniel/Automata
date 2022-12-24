using System.Collections.Concurrent;
using Automata.Core.Contracts.EventAggregator;

namespace Automata.Services.EventAggregator;
internal class Messenger : IMessenger
{
    private readonly ConcurrentDictionary<Type, List<object>> _subscribers = new();

    private SynchronizationContext? synchronizationContext;

    public void SetMainThreadSynchronizationContext(SynchronizationContext context)
    {
        this.synchronizationContext = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Publish<T>(T message) where T : IMessage
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        Type messageType = typeof(T);
        if (_subscribers.ContainsKey(messageType))
        {
            var subscriptions = _subscribers[messageType];
            if (subscriptions == null || subscriptions.Count == 0)
                return;

            foreach (var subscription in subscriptions.Select(s => (Subscription<T>)s))
            {
                subscription.Invoke(message);
            }
        }
    }

    public ISubscription<T> Subscribe<T>(IMessenger.EventHandlerDelegate<T> handler, ThreadOptions threadOptions = ThreadOptions.Publisher)
        where T : IMessage
    {
        Type messageType = typeof(T);
        ISubscription<T> subscription = new Subscription<T>(handler, threadOptions, synchronizationContext);

        if (_subscribers.TryGetValue(messageType, out var handlers))
        {
            handlers.Add(subscription);
        }
        else
        {
            _subscribers[messageType] = new List<object> { subscription };
        }

        return subscription;
    }

    public bool Unsubscribe<T>(ISubscription<T> subscription) where T : IMessage
    {
        var removed = false;

        if (subscription == null)
            return false;

        Type messageType = typeof(T);
        if (_subscribers.ContainsKey(messageType))
        {
            removed = _subscribers[messageType].Remove(subscription);

            if (_subscribers[messageType].Count == 0)
                _subscribers.Remove(messageType, out _);
        }

        return removed;
    }

    public void ClearAllSubscriptions()
    {
        _subscribers.Clear();
    }
}
