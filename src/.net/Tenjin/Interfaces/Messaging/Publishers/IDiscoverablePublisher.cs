namespace Tenjin.Interfaces.Messaging.Publishers;

/// <summary>
/// An IPublisher interface that is discoverable.
/// </summary>
public interface IDiscoverablePublisher<out TKey, TData> : IPublisher<TData>
{
    /// <summary>
    /// The unique ID of the IPublisher instance.
    /// </summary>
    TKey Id { get; }
}