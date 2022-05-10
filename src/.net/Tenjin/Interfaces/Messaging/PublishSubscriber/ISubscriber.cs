using System.Threading.Tasks;
using Tenjin.Models.Messaging.PublisherSubscriber;

namespace Tenjin.Interfaces.Messaging.PublishSubscriber
{
    public interface ISubscriber<TData>
    {
        string Id { get; }

        Task Receive(PublishEvent<TData> publishEvent);
    }
}
