using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Exceptions.Diagnostics;
using Tenjin.Extensions;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Implementations.Diagnostics;

/// <summary>
/// The default implementation of the IDiagnosticsLapStopwatch instance.
/// </summary>
public class DiagnosticsLapStopwatch(ISystemClockProvider systemClockProvider) : IDiagnosticsLapStopwatch
{
    private bool _running;
    private readonly IList<DiagnosticsStopwatchLap> _laps = new List<DiagnosticsStopwatchLap>();

    /// <inheritdoc />
    public Task StartLap(string? name = null)
    {
        if (_running)
        {
            throw new DiagnosticsStopwatchLapException(
                "A lap is already running. Stop the current lap before starting a new lap.");
        }

        var lap = new DiagnosticsStopwatchLap
        {
            Name = name,
            Order = (uint)_laps.Count + 1u,
            StartTimestamp = systemClockProvider.Now()
        };

        _running = true;
        _laps.Add(lap);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<DiagnosticsStopwatchLap> StopLap()
    {
        if (!_running)
        {
            throw new DiagnosticsStopwatchLapException(
                "Stopwatch is not running. Start the stopwatch before ending a new lap.");
        }

        var stoppedLap = _laps[_laps.LastIndex()] with
        {
            EndTimestamp = systemClockProvider.Now()
        };

        _laps[_laps.LastIndex()] = stoppedLap;
        _running = false;

        return Task.FromResult(stoppedLap);
    }

    /// <inheritdoc />
    public Task<DiagnosticsStopwatchLapStatistics> GetLapStatistics()
    {
        if (_running)
        {
            throw new DiagnosticsStopwatchLapException(
                "Stopwatch is running. Cannot calculate statistics while the stopwatch is still running.");
        }

        if (_laps.Count == 0)
        {
            throw new DiagnosticsStopwatchLapException(
                "Stopwatch was never started. Cannot calculate statistics with no recorded laps.");
        }

        var orderedTimes = _laps
           .Select(l => new
           {
               Lap = l,
               Timespan = l.TimeSpan()
           })
           .OrderBy(l => l.Timespan.TotalMilliseconds)
           .ToArray();
        var firstLap = _laps[0];
        var lastLap = _laps[_laps.LastIndex()];
        var fastest = orderedTimes[0].Lap;
        var slowest = orderedTimes[orderedTimes.LastIndex()].Lap;
        var averageTimespan = TimeSpan.FromTicks((long)orderedTimes.Average(l => l.Timespan.Ticks));
        var totalTimespan = TimeSpan.FromTicks(orderedTimes.Sum(l => l.Timespan.Ticks));

        return Task.FromResult(
            new DiagnosticsStopwatchLapStatistics
            {
                StartTimestamp = firstLap.StartTimestamp,
                EndTimestamp = lastLap.EndTimestamp,
                TotalTimespan = totalTimespan,
                AverageTimespan = averageTimespan,
                FastestLap = fastest,
                SlowestLap = slowest,
                Order = (uint)_laps.Count + 1u
            });
    }

    /// <inheritdoc />
    public Task<IEnumerable<DiagnosticsStopwatchLap>> GetAllLaps()
    {
        if (_running)
        {
            throw new DiagnosticsStopwatchLapException(
                "Stopwatch is running. Cannot fetch all laps while the stopwatch is still running.");
        }

        return Task.FromResult<IEnumerable<DiagnosticsStopwatchLap>>(_laps.ToArray());
    }
}