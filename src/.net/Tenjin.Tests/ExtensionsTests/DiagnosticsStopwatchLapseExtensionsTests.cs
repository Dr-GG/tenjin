using FluentAssertions;
using NUnit.Framework;
using System;
using Tenjin.Extensions;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture]
public class DiagnosticsStopwatchLapExtensionsTests
{
    private const string TimestampFormat = "yyyy-MM-dd HH:mm:ss";

    [TestCase("2000-01-01 09:00:00", "2000-01-01 09:00:01")]
    [TestCase("2000-01-01 09:00:00", "2000-01-01 09:01:00")]
    [TestCase("2000-01-01 09:00:00", "2000-01-01 10:00:00")]
    [TestCase("2000-01-01 09:00:00", "2000-01-02 09:00:00")]
    [TestCase("2000-01-01 09:00:00", "2000-02-01 09:00:00")]
    [TestCase("2000-01-01 09:00:00", "2001-01-01 09:00:00")]
    public void TimeSpan_WhenProvidingDifferentTimestamps_ProvidesTheExpectedResult(string start, string stop)
    {
        var startTimestamp = DateTime.ParseExact(start, TimestampFormat, null);
        var stopTimestamp = DateTime.ParseExact(stop, TimestampFormat, null);
        var timespan = stopTimestamp - startTimestamp;
        var lap = new DiagnosticsStopwatchLap
        {
            StartTimestamp = startTimestamp,
            EndTimestamp = stopTimestamp
        };

        timespan.Should().Be(lap.TimeSpan());
    }
}