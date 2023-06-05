using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers.Configuration;

namespace Tenjin.Interfaces.Messaging.Publishers;

/// <summary>
/// An interface that acts as a simple publisher.
/// </summary>
public interface IPublisher<TData> : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Configures the IPublisher instance via a PublisherConfiguration instance.
    /// </summary>
    IPublisher<TData> Configure(PublisherConfiguration configuration);

    /// <summary>
    /// Attaches a single ISubscriber instance.
    /// </summary>
    Task<IPublisherLock> Subscribe(ISubscriber<TData> subscriber);

    /// <summary>
    /// Attaches a collection of ISubscriber instances.
    /// </summary>
    Task<IEnumerable<IPublisherLock>> Subscribe(params ISubscriber<TData>[] subscribers);

    /// <summary>
    /// Unsubscribes a collection of ISubscriber instances.
    /// </summary>
    Task Unsubscribe(params ISubscriber<TData>[] subscribers);

    /// <summary>
    /// Publishes an event.
    /// </summary>
    Task<Guid> Publish(TData data);
}