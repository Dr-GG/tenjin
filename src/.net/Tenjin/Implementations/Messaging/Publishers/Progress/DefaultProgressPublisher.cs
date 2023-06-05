using Tenjin.Models.Messaging.Publishers.Progress;

namespace Tenjin.Implementations.Messaging.Publishers.Progress;

/// <summary>
/// The default IProgressPublisher instance that makes use of a ProgressEvent instance.
/// </summary>
public class DefaultProgressPublisher : ProgressPublisher<ProgressEvent>
{
    /// <inheritdoc />
    protected override ProgressEvent CreateProgressEvent(ulong current, ulong total)
    {
        return new ProgressEvent(current, total);
    }
}