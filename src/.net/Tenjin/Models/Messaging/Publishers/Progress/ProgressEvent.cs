using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Models.Messaging.Publishers.Progress;

/// <summary>
/// Depicts a progress event for the IProgressPublisher interface.
/// </summary>
[ExcludeFromCodeCoverage]
public record ProgressEvent
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public ProgressEvent() { }

    /// <summary>
    /// Creates a new instance with a set total.
    /// </summary>
    public ProgressEvent(ulong total) : this(0, total) { }

    /// <summary>
    /// Creates a new instance with a new current and total.
    /// </summary>
    public ProgressEvent(ulong current, ulong total)
    {
        Current = current;
        Total = total;
    }

    /// <summary>
    /// The current progress value.
    /// </summary>
    public ulong Current { get; init; }

    /// <summary>
    /// The total progress value.
    /// </summary>
    public ulong Total { get; init; }
}