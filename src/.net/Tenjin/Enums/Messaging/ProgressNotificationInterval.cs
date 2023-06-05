namespace Tenjin.Enums.Messaging;

/// <summary>
/// Indicates at which intervals progress should be emitted in IProgressPublisher instances.
/// </summary>
public enum ProgressNotificationInterval
{
    /// <summary>
    /// None.
    /// </summary>
    None = 0,

    /// <summary>
    /// Emits progress at a fixed interval.
    /// </summary>
    FixedInterval = 1,

    /// <summary>
    /// Emits intervals in percentage intervals.
    /// </summary>
    PercentageInterval = 2
}

