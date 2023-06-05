namespace Tenjin.Enums.Messaging;

/// <summary>
/// Depicts the event type that an IProgressPublisher instance publishes.
/// </summary>
public enum PublishEventType
{
    /// <summary>
    /// Depicts a normal publishing event.
    /// </summary>
    Publish = 1,

    /// <summary>
    /// Depicts a normal disposing event.
    /// </summary>
    Disposing = 2,

    /// <summary>
    /// Depicts an error.
    /// </summary>
    Error = 3
}