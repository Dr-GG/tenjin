using Tenjin.Interfaces.Messaging.Publishers;

namespace Tenjin.Implementations.Messaging.Publishers
{
    public abstract class DiscoverablePublisher<TKey, TData> : 
        Publisher<TData>, 
        IDiscoverablePublisher<TKey, TData>
    {
        public abstract TKey Id { get; }
    }
}
