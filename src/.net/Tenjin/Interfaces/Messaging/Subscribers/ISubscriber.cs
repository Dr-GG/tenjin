using System.Threading.Tasks;
using Tenjin.Models.Messaging.Publishers;

namespace Tenjin.Interfaces.Messaging.Subscribers;

/// <summary>
/// A simple subscriber interface that works with the IPublisher interface.
/// </summary>
public interface ISubscriber<TData>
{
    /// <summary>
    /// Gets the unique ID of the ISubscriber instance.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Receives and processes a single PublishEvent.
    /// </summary>
    Task Receive(PublishEvent<TData> publishEvent);
}