using System;
using NUnit.Framework;
using Tenjin.Extensions;
using Tenjin.Models.Messaging.Diagnostics;

namespace Tenjin.Tests.ExtensionsTests
{
    [TestFixture]
    public class DiagnosticsStopwatchLapseExtensionsTests
    {
        private const string TimestampFormat = "yyyy-MM-dd HH:mm:ss";

        [TestCase("2000-01-01 09:00:00", "2000-01-01 09:00:01")]
        [TestCase("2000-01-01 09:00:00", "2000-01-01 09:01:00")]
        [TestCase("2000-01-01 09:00:00", "2000-01-01 10:00:00")]
        [TestCase("2000-01-01 09:00:00", "2000-01-02 09:00:00")]
        [TestCase("2000-01-01 09:00:00", "2000-02-01 09:00:00")]
        [TestCase("2000-01-01 09:00:00", "2001-01-01 09:00:00")]
        public void Timespan_WhenProvidingDifferentTimestamps_ProvidesTheExpectedResult(string start, string stop)
        {
            var startTimestamp = DateTime.ParseExact(start, TimestampFormat, null);
            var stopTimestamp = DateTime.ParseExact(stop, TimestampFormat, null);
            var timespan = stopTimestamp - startTimestamp;
            var lapse = new DiagnosticsStopwatchLapse
            {
                StartTimestamp = startTimestamp,
                EndTimestamp = stopTimestamp
            };

            Assert.AreEqual(timespan, lapse.Timespan());
        }
    }
}
