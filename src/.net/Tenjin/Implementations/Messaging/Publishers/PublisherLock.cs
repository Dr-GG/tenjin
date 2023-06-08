using System;
using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;

namespace Tenjin.Implementations.Messaging.Publishers;

/// <summary>
/// The default implementation of the IPublisherLock interface that provides automated un-subscriptions.
/// </summary>
public class PublisherLock<TData> : IPublisherLock
{
    private readonly IPublisher<TData> _publisher;
    private readonly ISubscriber<TData> _subscriber;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public PublisherLock(IPublisher<TData> publisher, ISubscriber<TData> subscriber)
    {
        _publisher = publisher;
        _subscriber = subscriber;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Dispose(true);

        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        _publisher.Unsubscribe(_subscriber).GetAwaiter().GetResult();
    }
}