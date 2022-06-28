using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Exceptions.Diagnostics;
using Tenjin.Extensions;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Implementations.Diagnostics;

public class DiagnosticsStopwatch : IDiagnosticsStopwatch
{
    private bool _running;

    private readonly ISystemClockProvider _systemClockProvider;
    private readonly IList<DiagnosticsStopwatchLap> _laps = new List<DiagnosticsStopwatchLap>();

    public DiagnosticsStopwatch(ISystemClockProvider systemClockProvider)
    {
        _systemClockProvider = systemClockProvider;
    }

    public Task Start(string? name = null)
    {
        if (_running)
        {
            throw new DiagnosticsStopwatchException(
                "A lap is already running. Stop the current lap before starting a new lap");
        }

        var lap = new DiagnosticsStopwatchLap
        {
            Name = name,
            Order = (uint)_laps.Count + 1u,
            StartTimestamp = _systemClockProvider.Now()
        };

        _running = true;
        _laps.Add(lap);

        return Task.CompletedTask;
    }

    public Task<DiagnosticsStopwatchLap> Stop()
    {
        if (!_running)
        {
            throw new DiagnosticsStopwatchException(
                "Stopwatch is not running. Start the stopwatch before ending a new lap.");
        }

        var stoppedLap = _laps.Last() with
        {
            EndTimestamp = _systemClockProvider.Now()
        };

        _laps[_laps.LastIndex()] = stoppedLap;
        _running = false;

        return Task.FromResult(stoppedLap);
    }

    public Task<DiagnosticsStopwatchLapStatistics> GetStatistics()
    {
        if (_running)
        {
            throw new DiagnosticsStopwatchException(
                "Stopwatch is running. Cannot calculate statistics while the stopwatch is still running");
        }

        if (_laps.Count == 0)
        {
            throw new DiagnosticsStopwatchException(
                "Stopwatch was never started. Cannot calculate statistics with no recorded laps");
        }

        var orderedTimes = _laps
            .Select(l => new
            {
                Lap = l,
                Timespan = l.Timespan()
            })
            .OrderBy(l => l.Timespan.TotalMilliseconds).ToArray();
        var firstLap = _laps.First();
        var lastLap = _laps.Last();
        var fastest = orderedTimes.First().Lap;
        var slowest = orderedTimes.Last().Lap;
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

    public Task<IEnumerable<DiagnosticsStopwatchLap>> GetAllLaps()
    {
        if (_running)
        {
            throw new DiagnosticsStopwatchException(
                "Stopwatch is running. Cannot fetch all laps while the stopwatch is still running");
        }

        return Task.FromResult<IEnumerable<DiagnosticsStopwatchLap>>(_laps.ToArray());
    }
}