namespace Tenjin.Interfaces.Messaging.PublishSubscriber
{
    public interface IDiscoverablePublisher<out TKey, TData> : IPublisher<TData>
    {
        TKey Id { get; }
    }
}
