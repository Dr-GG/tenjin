using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tenjin.Models.Messaging.Configuration;

namespace Tenjin.Interfaces.Messaging
{
    public interface IPublisher<TData> : IDisposable, IAsyncDisposable
    {
        IPublisher<TData> Configure(PublisherConfiguration configuration);
        Task<IPublisherLock> Subscribe(ISubscriber<TData> subscriber);
        Task<IEnumerable<IPublisherLock>> Subscribe(params ISubscriber<TData>[] subscribers);
        Task Unsubscribe(params ISubscriber<TData>[] subscribers);
        Task<Guid> Publish(TData data);
    }
}
