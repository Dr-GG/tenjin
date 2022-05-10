using System.Threading.Tasks;
using Tenjin.Models.Messaging.PublisherSubscriber.Configuration;
using Tenjin.Models.Messaging.PublisherSubscriber.Progress;

namespace Tenjin.Interfaces.Messaging.PublishSubscriber.Progress
{
    public interface IProgressPublisher<TProgressEvent> : IPublisher<TProgressEvent>
        where TProgressEvent : ProgressEvent
    {
        IProgressPublisher<TProgressEvent> Configure(ProgressPublisherConfiguration configuration);
        Task Initialise(ulong total, bool publish = true);
        Task Tick();
        Task Tick(ulong ticks);
    }
}
