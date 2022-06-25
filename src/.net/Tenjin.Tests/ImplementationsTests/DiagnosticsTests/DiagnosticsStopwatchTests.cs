using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tenjin.Exceptions.Diagnostics;
using Tenjin.Extensions;
using Tenjin.Implementations.Diagnostics;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Tests.Services;

namespace Tenjin.Tests.ImplementationsTests.DiagnosticsTests
{
    [TestFixture, Parallelizable(ParallelScope.Children)]
    public class DiagnosticsStopwatchTests
    {
        [Test]
        public async Task Start_WhenAlreadyRunning_ThrowsAnException()
        {
            var stopwatch = GetStopwatch();

            await stopwatch.Start();

            Assert.ThrowsAsync<DiagnosticsStopwatchException>(() => stopwatch.Start());
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(25)]
        [TestCase(50)]
        [TestCase(75)]
        [TestCase(100)]
        public async Task StartStop_WhenCreatingMultipleLaps_CreatesTheCorrectLapNamesAndOrders(int numberOfLaps)
        {
            var stopwatch = GetStopwatch();

            for (var i = 0; i < numberOfLaps; i++)
            {
                var lapName = GetLapName(i);

                await stopwatch.Start(lapName);
                await stopwatch.Stop();
            }

            var laps = (await stopwatch.GetAllLaps()).ToList();

            Assert.AreEqual(numberOfLaps, laps.Count);

            for (var i = 0; i < laps.Count; i++)
            {
                var lap = laps[i];

                Assert.AreEqual(GetLapName(i), lap.Name);
                Assert.AreEqual(i + 1, lap.Order);
            }
        }

        [Test]
        public void Stop_WhenNotRunning_ThrowsAnException()
        {
            var stopwatch = GetStopwatch();

            Assert.ThrowsAsync<DiagnosticsStopwatchException>(() => stopwatch.Stop());
        }

        [Test]
        public async Task GetStatistics_WhenRunning_ThrowsAnException()
        {
            var stopwatch = GetStopwatch();

            await stopwatch.Start();

            Assert.ThrowsAsync<DiagnosticsStopwatchException>(() => stopwatch.GetStatistics());
        }

        [Test]
        public void GetStatistics_WithNoLaps_ThrowsAnException()
        {
            var stopwatch = GetStopwatch();

            Assert.ThrowsAsync<DiagnosticsStopwatchException>(() => stopwatch.GetStatistics());
        }

        [Test]
        public async Task GetAllLaps_WhenRunningCompleteLaps_ReturnsExpectedStatistics()
        {
            var timestamps = GetFixedTimestamps().ToList();
            var clockProvider = new CollectionSystemClockProvider(timestamps);
            var stopwatch = GetStopwatch(clockProvider);
            var lapCount = timestamps.Count / 2;

            for (var i = 0; i < lapCount; i++)
            {
                await stopwatch.Start();
                await stopwatch.Stop();
            }

            var statistics = await stopwatch.GetStatistics();

            // Fastest.
            Assert.AreEqual(2, statistics.FastestLap.Order);
            Assert.AreEqual(1.0, statistics.FastestLap.Timespan().TotalSeconds);

            // Slowest.
            Assert.AreEqual(4, statistics.SlowestLap.Order);
            Assert.AreEqual(60.0, statistics.SlowestLap.Timespan().TotalSeconds);

            // Other stats.
            Assert.AreEqual(89.0, statistics.TotalTimespan.TotalSeconds);
            Assert.AreEqual(17.8, statistics.AverageTimespan.TotalSeconds);
            Assert.AreEqual(new DateTime(2000, 01, 01, 01, 00, 00), statistics.StartTimestamp);
            Assert.AreEqual(new DateTime(2000, 01, 01, 01, 05, 03), statistics.EndTimestamp);
            Assert.AreEqual(6, statistics.Order);
        }

        private static IEnumerable<DateTime> GetFixedTimestamps()
        {
            return new[]
            {
                new DateTime(2000, 01, 01, 01, 00, 00),
                new DateTime(2000, 01, 01, 01, 00, 10), // Lap 01 = 10 Seconds.

                new DateTime(2000, 01, 01, 01, 00, 11),
                new DateTime(2000, 01, 01, 01, 00, 12), // Lap 02 = 01 second. Fastest lap.

                new DateTime(2000, 01, 01, 01, 01, 30),
                new DateTime(2000, 01, 01, 01, 01, 45), // Lap 03 = 15 seconds.

                new DateTime(2000, 01, 01, 01, 02, 30),
                new DateTime(2000, 01, 01, 01, 03, 30), // Lap 04 = 60 seconds. Slowest lap.

                new DateTime(2000, 01, 01, 01, 05, 00),
                new DateTime(2000, 01, 01, 01, 05, 03), // Lap 05 = 03 seconds.
            };
        }

        private static string GetLapName(int index)
        {
            return $"Lap {index + 1}";
        }

        private static IDiagnosticsStopwatch GetStopwatch(ISystemClockProvider? clockProvider = null)
        {
            return new DiagnosticsStopwatch(clockProvider ?? new SystemClockProvider(true));
        }
    }
}
