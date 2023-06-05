using System.Collections.Generic;
using Tenjin.Interfaces.Messaging.Publishers;

namespace Tenjin.Implementations.Messaging.Publishers;

/// <summary>
/// The default implementation of the IPublisherRegistry interface.
/// </summary>
public abstract class PublisherRegistry<TKey, TData> : IPublisherRegistry<TKey, TData> where TKey : notnull
{
    private readonly IDictionary<TKey, IDiscoverablePublisher<TKey, TData>> _publishers = new Dictionary<TKey, IDiscoverablePublisher<TKey, TData>>();

    /// <inheritdoc />
    public IPublisher<TData> this[TKey key] => Get(key);

    /// <inheritdoc />
    public IPublisher<TData> Get(TKey key)
    {
        if (_publishers.TryGetValue(key, out var publisher))
        {
            return publisher;
        }

        throw new KeyNotFoundException($"A discoverable publisher with the key {key} was not found.");
    }

    /// <inheritdoc />
    public bool TryGet(TKey key, out IPublisher<TData>? publisher)
    {
        if (_publishers.TryGetValue(key, out var result))
        {
            publisher = result;

            return true;
        }

        publisher = default;

        return false;
    }

    /// <summary>
    /// Registers a collection of IDiscoverablePublisher instances.
    /// </summary>
    protected void Register(IEnumerable<IDiscoverablePublisher<TKey, TData>> publishers)
    {
        foreach (var publisher in publishers)
        {
            _publishers.Add(publisher.Id, publisher);
        }
    }
}