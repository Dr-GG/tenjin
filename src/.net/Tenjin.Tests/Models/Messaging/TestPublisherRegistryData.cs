using System.Collections.Generic;
using System.Linq;

namespace Tenjin.Tests.Models.Messaging
{
    public record TestPublisherRegistryData<TKey>
    {
        public IEnumerable<TKey> PublisherIds { get; init; } = Enumerable.Empty<TKey>();
        public IEnumerable<TKey> TestExistingPublisherIds { get; init; } = Enumerable.Empty<TKey>();
        public IEnumerable<TKey> NonExistingPublisherIds { get; init; } = Enumerable.Empty<TKey>();
    }
}
