using Tenjin.Enums.Messaging;

namespace Tenjin.Models.Messaging.Publishers.Configuration;

public record ProgressPublisherConfiguration
{
    public ProgressPublisherConfiguration()
    { }

    public ProgressPublisherConfiguration(ulong fixedInterval)
    {
        Interval = ProgressNotificationInterval.FixedInterval;
        FixedInterval = fixedInterval;
    }

    public ProgressPublisherConfiguration(double percentageInterval)
    {
        Interval = ProgressNotificationInterval.PercentageInterval;
        PercentageInterval = percentageInterval;
    }

    public ProgressNotificationInterval Interval { get; init; } = ProgressNotificationInterval.None;
    public ulong? FixedInterval { get; set; }
    public double? PercentageInterval { get; set; }
}