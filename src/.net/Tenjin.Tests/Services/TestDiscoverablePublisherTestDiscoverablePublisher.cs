using Tenjin.Implementations.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Publishers;
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
