// ReSharper disable UnusedMemberInSuper.Global

namespace Tenjin.Interfaces.Messaging.Publishers;

/// <summary>
/// An interface that provides a registry of IPublisher instances.
/// </summary>
public interface IPublisherRegistry<in TKey, TData> where TKey : notnull
{
    /// <summary>
    /// Gets an IPublisher based on the key of an IPublisher.
    /// </summary>
    IPublisher<TData> this[TKey key] { get; }

    /// <summary>
    /// Gets an IPublisher based on the key of an IPublisher.
    /// </summary>
    IPublisher<TData> Get(TKey key);

    /// <summary>
    /// Tries to get an IPublisher instance based on a key.
    /// </summary>
    bool TryGet(TKey key, out IPublisher<TData>? publisher);
}