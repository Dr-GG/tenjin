using System;
using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging.Publishers;

namespace Tenjin.Interfaces.Messaging.Subscribers;

/// <summary>
/// An interface that extends the ISubscriber interface and provides more convenient methods for IPublisher events.
/// </summary>
public interface ISubscriberHook<TData> : ISubscriber<TData>, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Subscribes the current instance to an IPublisher.
    /// </summary>
    Task<ISubscriberHook<TData>> Subscribe(IPublisher<TData> publisher);
}