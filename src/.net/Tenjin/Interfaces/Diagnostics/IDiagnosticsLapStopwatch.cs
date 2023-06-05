using System.Collections.Generic;
using System.Threading.Tasks;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Interfaces.Diagnostics;

/// <summary>
/// An interface that provides diagnostic stopwatch functionality with lapping functionality.
/// </summary>
public interface IDiagnosticsLapStopwatch
{
    /// <summary>
    /// Starts a new lap.
    /// </summary>
    Task StartLap(string? name = null);

    /// <summary>
    /// Stops the current lap.
    /// </summary>
    Task<DiagnosticsStopwatchLap> StopLap();

    /// <summary>
    /// Gets all lap statistics.
    /// </summary>
    Task<DiagnosticsStopwatchLapStatistics> GetLapStatistics();

    /// <summary>
    /// Gets all laps.
    /// </summary>
    Task<IEnumerable<DiagnosticsStopwatchLap>> GetAllLaps();
}