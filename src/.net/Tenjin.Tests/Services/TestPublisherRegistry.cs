using System.Linq;
using Tenjin.Implementations.Messaging.PublisherSubscriber;
using Tenjin.Tests.Models.Messaging;

namespace Tenjin.Tests.Services
{
    public class TestPublisherRegistry<TKey> : PublisherRegistry<TKey, TestPublishData>
    {
        public TestPublisherRegistry(TestPublisherRegistryData<TKey> data)
        {
            var publishers = data.PublisherIds
                .Select(id => new TestDiscoverablePublisher<TKey>(id))
                .ToArray();

            Register(publishers);
        }
    }
}
