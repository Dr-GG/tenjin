using System;
using System.Threading.Tasks;

namespace Tenjin.Interfaces.Messaging
{
    public interface ISubscriberHook<TData> : ISubscriber<TData>, IDisposable, IAsyncDisposable
    {
        Task<ISubscriberHook<TData>> Subscribe(IPublisher<TData> publisher);
    }
}
