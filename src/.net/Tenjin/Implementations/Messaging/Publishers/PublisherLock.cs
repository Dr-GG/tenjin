using System;
using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;

namespace Tenjin.Implementations.Messaging.Publishers;

/// <summary>
/// The default implementation of the IPublisherLock interface that provides automated un-subscriptions.
/// </summary>
public class PublisherLock<TData>(IPublisher<TData> publisher, ISubscriber<TData> subscriber) : IPublisherLock
{
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
        publisher.Unsubscribe(subscriber).GetAwaiter().GetResult();
    }
}