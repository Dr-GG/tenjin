using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Exceptions.Diagnostics;
using Tenjin.Extensions;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Models.Messaging.Diagnostics;

namespace Tenjin.Implementations.Diagnostics
{
    public class DiagnosticsStopwatch : IDiagnosticsStopwatch
    {
        private bool _running;

        private readonly ISystemClockProvider _systemClockProvider;
        private readonly IList<DiagnosticsStopwatchLapse> _lapses = new List<DiagnosticsStopwatchLapse>();

        public DiagnosticsStopwatch() : this(new SystemClockProvider(true))
        { }

        public DiagnosticsStopwatch(ISystemClockProvider systemClockProvider)
        {
            _systemClockProvider = systemClockProvider;
        }

        public Task Start(string? name = null)
        {
            if (_running)
            {
                throw new DiagnosticsStopwatchException(
                    "A lapse is already running. Stop the current lapse before starting a new lapse");
            }

            var lapse = new DiagnosticsStopwatchLapse
            {
                Name = name,
                Order = (uint)_lapses.Count + 1u,
                StartTimestamp = _systemClockProvider.Now()
            };

            _running = true;
            _lapses.Add(lapse);

            return Task.CompletedTask;
        }

        public Task<DiagnosticsStopwatchLapse> Stop()
        {
            if (!_running)
            {
                throw new DiagnosticsStopwatchException(
                    "Stopwatch is not running. Start the stopwatch before ending a new lapse.");
            }

            var stoppedLapse = _lapses.Last() with
            {
                EndTimestamp = _systemClockProvider.Now()
            };

            _lapses[_lapses.LastIndex()] = stoppedLapse;
            _running = false;

            return Task.FromResult(stoppedLapse);
        }

        public Task<DiagnosticsStopwatchLapseStatistics> GetStatistics()
        {
            if (_running)
            {
                throw new DiagnosticsStopwatchException(
                    "Stopwatch is running. Cannot calculate statistics while the stopwatch is still running");
            }

            if (_lapses.Count == 0)
            {
                throw new DiagnosticsStopwatchException(
                    "Stopwatch was never started. Cannot calculate statistics with no recorded lapses");
            }

            var orderedTimes = _lapses
                .Select(l => new
                {
                    Lapse = l,
                    Timespan = l.Timespan()
                })
                .OrderBy(l => l.Timespan.TotalMilliseconds).ToArray();
            var firstLapse = _lapses.First();
            var lastLapse = _lapses.Last();
            var fastest = orderedTimes.First().Lapse;
            var slowest = orderedTimes.Last().Lapse;
            var averageTimespan = TimeSpan.FromTicks((long)orderedTimes.Average(l => l.Timespan.Ticks));
            var totalTimespan = TimeSpan.FromTicks(orderedTimes.Sum(l => l.Timespan.Ticks));

            return Task.FromResult(
                new DiagnosticsStopwatchLapseStatistics
                {
                    StartTimestamp = firstLapse.StartTimestamp,
                    EndTimestamp = lastLapse.EndTimestamp,
                    TotalTimespan = totalTimespan,
                    AverageTimespan = averageTimespan,
                    FastestLapse = fastest,
                    SlowestLapse = slowest,
                    Order = (uint)_lapses.Count + 1u,
                });
        }

        public Task<IEnumerable<DiagnosticsStopwatchLapse>> GetAllLapses()
        {
            if (_running)
            {
                throw new DiagnosticsStopwatchException(
                    "Stopwatch is running. Cannot fetch all lapses while the stopwatch is still running");
            }

            return Task.FromResult<IEnumerable<DiagnosticsStopwatchLapse>>(_lapses.ToArray());
        }
    }
}
