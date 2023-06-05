using System;

namespace Tenjin.Models.Diagnostics;

/// <summary>
/// Depicts a single IDiagnosticsLapStopwatch lap.
/// </summary>
public record DiagnosticsStopwatchLap
{
    /// <summary>
    /// The zero-based index order of the lap.
    /// </summary>
    public uint Order { get; init; } = 0;

    /// <summary>
    /// The name of the lap, if any.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The start timestamp of the lap.
    /// </summary>
    public DateTime StartTimestamp { get; init; } = DateTime.MinValue;

    /// <summary>
    /// The end timestamp of the lap.
    /// </summary>
    public DateTime EndTimestamp { get; init; } = DateTime.MaxValue;
}