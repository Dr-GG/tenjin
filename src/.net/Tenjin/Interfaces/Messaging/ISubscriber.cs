using System.Threading.Tasks;
using Tenjin.Models.Messaging;

namespace Tenjin.Interfaces.Messaging
{
    public interface ISubscriber<TData>
    {
        string Id { get; }

        Task Receive(PublishEvent<TData> publishEvent);
    }
}
