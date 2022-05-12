namespace Tenjin.Interfaces.Messaging.Publishers
{
    public interface IPublisherRegistry<in TKey, TData> where TKey : notnull
    {
        IPublisher<TData> this[TKey key] { get; }

        IPublisher<TData> Get(TKey key);
        bool TryGet(TKey key, out IPublisher<TData>? publisher);
    }
}
