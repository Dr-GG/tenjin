namespace Tenjin.Enums.Messaging;

/// <summary>
/// Depicts which threading mode an IProgressPublisher instance uses.
/// </summary>
public enum PublisherThreadMode
{
    /// <summary>
    /// Uses multiple threads.
    /// </summary>
    Multi = 0,

    /// <summary>
    /// Uses a single thread.
    /// </summary>
    Single = 1
}