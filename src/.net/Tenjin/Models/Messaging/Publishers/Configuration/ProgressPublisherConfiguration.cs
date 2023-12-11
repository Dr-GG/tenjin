using Tenjin.Enums.Messaging;

namespace Tenjin.Models.Messaging.Publishers.Configuration;

/// <summary>
/// The configuration data structure that configures an IProgressPublisher instance.
/// </summary>
public record ProgressPublisherConfiguration
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public ProgressPublisherConfiguration() { }

    /// <summary>
    /// Creates a new instance with a fixed interval.
    /// </summary>
    public ProgressPublisherConfiguration(ulong fixedInterval)
    {
        Interval = ProgressNotificationInterval.FixedInterval;
        FixedInterval = fixedInterval;
    }

    /// <summary>
    /// Creates a new instance with a percentage interval.
    /// </summary>
    public ProgressPublisherConfiguration(double percentageInterval)
    {
        Interval = ProgressNotificationInterval.PercentageInterval;
        PercentageInterval = percentageInterval;
    }

    /// <summary>
    /// The ProgressNotificationInterval to use.
    /// </summary>
    public ProgressNotificationInterval Interval { get; init; } = ProgressNotificationInterval.None;

    /// <summary>
    /// The fixed interval.
    /// </summary>
    /// <remarks>
    /// This property must be set when Interval is set to ProgressNotificationInterval.FixedInterval.
    /// </remarks>
    public ulong? FixedInterval { get; set; }

    /// <summary>
    /// The percentage interval.
    /// </summary>
    /// <remarks>
    /// This property must be set when Interval is set to ProgressNotificationInterval.PercentageInterval.
    /// </remarks>
    public double? PercentageInterval { get; set; }
}