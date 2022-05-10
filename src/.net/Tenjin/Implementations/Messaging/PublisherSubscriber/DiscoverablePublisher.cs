using Tenjin.Interfaces.Messaging.PublishSubscriber;

namespace Tenjin.Implementations.Messaging.PublisherSubscriber
{
    public abstract class DiscoverablePublisher<TKey, TData> : 
        Publisher<TData>, 
        IDiscoverablePublisher<TKey, TData>
    {
        public abstract TKey Id { get; }
    }
}
