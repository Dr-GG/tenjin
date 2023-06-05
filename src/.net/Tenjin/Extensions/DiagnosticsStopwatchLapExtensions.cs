using System;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for the DiagnosticsStopwatchLap structure.
/// </summary>
public static class DiagnosticsStopwatchLapExtensions
{
    /// <summary>
    /// Calculates the Timespan instance of a DiagnosticsStopwatchLap using the EndTimestamp and StartTimestamp.
    /// </summary>
    public static TimeSpan TimeSpan(this DiagnosticsStopwatchLap lap)
    {
        return lap.EndTimestamp - lap.StartTimestamp;
    }
}