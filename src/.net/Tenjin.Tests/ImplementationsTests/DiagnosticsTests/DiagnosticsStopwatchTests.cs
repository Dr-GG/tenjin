using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Exceptions.Diagnostics;
using Tenjin.Extensions;
using Tenjin.Implementations.Diagnostics;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Tests.Services;

namespace Tenjin.Tests.ImplementationsTests.DiagnosticsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class DiagnosticsStopwatchTests
{
    [Test]
    public async Task Start_WhenAlreadyRunning_ThrowsAnException()
    {
        var stopwatch = GetStopwatch();

        await stopwatch.StartLap();

        var error = Assert.ThrowsAsync<DiagnosticsStopwatchLapException>(() => stopwatch.StartLap())!;

        error.Should().NotBeNull();
        error.Message.Should().Be("A lap is already running. Stop the current lap before starting a new lap.");
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

            await stopwatch.StartLap(lapName);
            await stopwatch.StopLap();
        }

        var laps = (await stopwatch.GetAllLaps()).ToList();

        laps.Count.Should().Be(numberOfLaps);

        for (var i = 0; i < laps.Count; i++)
        {
            var lap = laps[i];

            lap.Name.Should().Be(GetLapName(i));
            lap.Order.Should().Be((uint)(i + 1));
        }
    }

    [Test]
    public void Stop_WhenNotRunning_ThrowsAnException()
    {
        var stopwatch = GetStopwatch();

        var error = Assert.ThrowsAsync<DiagnosticsStopwatchLapException>(stopwatch.StopLap)!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Stopwatch is not running. Start the stopwatch before ending a new lap.");
    }

    [Test]
    public async Task GetStatistics_WhenRunning_ThrowsAnException()
    {
        var stopwatch = GetStopwatch();

        await stopwatch.StartLap();

        var error = Assert.ThrowsAsync<DiagnosticsStopwatchLapException>(stopwatch.GetLapStatistics)!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Stopwatch is running. Cannot calculate statistics while the stopwatch is still running.");
    }

    [Test]
    public void GetStatistics_WithNoLaps_ThrowsAnException()
    {
        var stopwatch = GetStopwatch();

        var error = Assert.ThrowsAsync<DiagnosticsStopwatchLapException>(stopwatch.GetLapStatistics)!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Stopwatch was never started. Cannot calculate statistics with no recorded laps.");
    }


    [Test]
    public async Task GetAllLaps_WhenRunning_ThrowsAnException()
    {
        var stopwatch = GetStopwatch();

        await stopwatch.StartLap();

        var error = Assert.ThrowsAsync<DiagnosticsStopwatchLapException>(stopwatch.GetAllLaps)!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Stopwatch is running. Cannot fetch all laps while the stopwatch is still running.");
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
            await stopwatch.StartLap();
            await stopwatch.StopLap();
        }

        var statistics = await stopwatch.GetLapStatistics();

        // Fastest.
        statistics.FastestLap.Order.Should().Be(2);
        statistics.FastestLap.TimeSpan().TotalSeconds.Should().Be(1.0);

        // Slowest.
        statistics.SlowestLap.Order.Should().Be(4);
        statistics.SlowestLap.TimeSpan().TotalSeconds.Should().Be(60);

        // Other stats.
        statistics.TotalTimespan.TotalSeconds.Should().Be(89.0);
        statistics.AverageTimespan.TotalSeconds.Should().Be(17.8);

        statistics.StartTimestamp.Should().Be(new DateTime(2000, 01, 01, 01, 00, 00));
        statistics.EndTimestamp.Should().Be(new DateTime(2000, 01, 01, 01, 05, 03));
        statistics.Order.Should().Be(6);
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
            new DateTime(2000, 01, 01, 01, 05, 03) // Lap 05 = 03 seconds.
        };
    }

    private static string GetLapName(int index)
    {
        return $"Lap {index + 1}";
    }

    private static IDiagnosticsLapStopwatch GetStopwatch(ISystemClockProvider? clockProvider = null)
    {
        return new DiagnosticsLapStopwatch(clockProvider ?? new SystemClockProvider(true));
    }
}