using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging;

namespace Tenjin.Implementations.Messaging
{
    public class PublisherLock<TData> : IPublisherLock
    {
        private bool _disposed;

        private readonly IPublisher<TData> _publisher;
        private readonly ISubscriber<TData> _subscriber;

        public PublisherLock(IPublisher<TData> publisher, ISubscriber<TData> subscriber)
        {
            _publisher = publisher;
            _subscriber = subscriber;
        }

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            await _publisher.Unsubscribe(_subscriber);

            _disposed = true;
        }
    }
}
