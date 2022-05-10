using Tenjin.Models.Messaging.PublisherSubscriber.Progress;

namespace Tenjin.Implementations.Messaging.PublisherSubscriber.Progress
{
    public class DefaultProgressPublisher : ProgressPublisher<ProgressEvent>
    {
        protected override ProgressEvent CreateProgressEvent(ulong current, ulong total)
        {
            return new ProgressEvent(current, total);
        }
    }
}
