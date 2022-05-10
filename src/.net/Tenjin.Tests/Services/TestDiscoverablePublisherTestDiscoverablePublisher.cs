using Tenjin.Implementations.Messaging.PublisherSubscriber;
using Tenjin.Interfaces.Messaging.PublishSubscriber;
using Tenjin.Tests.Models.Messaging;

namespace Tenjin.Tests.Services
{
    public class TestDiscoverablePublisher<TKey> : Publisher<TestPublishData>, IDiscoverablePublisher<TKey, TestPublishData>
    {
        public TestDiscoverablePublisher(TKey key)
        {
            Id = key;
        }

        public TKey Id { get; }
    }
}
