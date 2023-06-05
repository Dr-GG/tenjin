using System;

namespace Tenjin.Models.Diagnostics;

/// <summary>
/// Depicts a collection of statistics regarding a collection DiagnosticsStopwatchLap instances.
/// </summary>
public record DiagnosticsStopwatchLapStatistics : DiagnosticsStopwatchLap
{
    /// <summary>
    /// The total TimeSpan of all DiagnosticsStopwatchLap instances.
    /// </summary>
    public TimeSpan TotalTimespan { get; init; }

    /// <summary>
    /// The average TimeSpan of all DiagnosticsStopwatchLap instances.
    /// </summary>
    public TimeSpan AverageTimespan { get; init; }

    /// <summary>
    /// The slowest DiagnosticsStopwatchLap instance.
    /// </summary>
    public DiagnosticsStopwatchLap SlowestLap { get; init; } = new();

    /// <summary>
    /// The fastest DiagnosticsStopwatchLap instance.
    /// </summary>
    public DiagnosticsStopwatchLap FastestLap { get; init; } = new();
}