namespace Automata.Core.Contracts.EventAggregator;
public interface ISubscription<T> where T : IMessage
{
    ThreadOptions ThreadOptions { get; }

    void Invoke(T eventData);
}
