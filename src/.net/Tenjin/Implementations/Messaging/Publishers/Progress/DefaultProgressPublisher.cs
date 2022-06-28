using Tenjin.Models.Messaging.Publishers.Progress;

namespace Tenjin.Implementations.Messaging.Publishers.Progress;

public class DefaultProgressPublisher : ProgressPublisher<ProgressEvent>
{
    protected override ProgressEvent CreateProgressEvent(ulong current, ulong total)
    {
        return new ProgressEvent(current, total);
    }
}