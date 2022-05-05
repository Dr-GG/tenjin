using Tenjin.Interfaces.Messaging;

namespace Tenjin.Implementations.Messaging
{
    public abstract class DiscoverablePublisher<TKey, TData> : 
        Publisher<TData>, 
        IDiscoverablePublisher<TKey, TData>
    {
        public abstract TKey Id { get; }
    }
}
