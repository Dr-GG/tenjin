using System.Threading.Tasks;
using Tenjin.Models.Messaging.Publishers;

namespace Tenjin.Interfaces.Messaging.Subscribers;

public interface ISubscriber<TData>
{
    string Id { get; }

    Task Receive(PublishEvent<TData> publishEvent);
}