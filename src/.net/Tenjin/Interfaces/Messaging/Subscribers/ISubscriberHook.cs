using System;
using System.Threading.Tasks;
using Tenjin.Interfaces.Messaging.Publishers;

namespace Tenjin.Interfaces.Messaging.Subscribers
{
    public interface ISubscriberHook<TData> : ISubscriber<TData>, IDisposable, IAsyncDisposable
    {
        Task<ISubscriberHook<TData>> Subscribe(IPublisher<TData> publisher);
    }
}
