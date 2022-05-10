using System;
using System.Threading.Tasks;

namespace Tenjin.Interfaces.Messaging.PublishSubscriber
{
    public interface ISubscriberHook<TData> : ISubscriber<TData>, IDisposable, IAsyncDisposable
    {
        Task<ISubscriberHook<TData>> Subscribe(IPublisher<TData> publisher);
    }
}
