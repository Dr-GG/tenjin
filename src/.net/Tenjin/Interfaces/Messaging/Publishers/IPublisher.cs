using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers.Configuration;

namespace Tenjin.Interfaces.Messaging.Publishers;

public interface IPublisher<TData> : IDisposable, IAsyncDisposable
{
    IPublisher<TData> Configure(PublisherConfiguration configuration);
    Task<IPublisherLock> Subscribe(ISubscriber<TData> subscriber);
    Task<IEnumerable<IPublisherLock>> Subscribe(params ISubscriber<TData>[] subscribers);
    Task Unsubscribe(params ISubscriber<TData>[] subscribers);
    Task<Guid> Publish(TData data);
}