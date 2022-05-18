using System;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Extensions
{
    public static class DiagnosticsStopwatchLapExtensions
    {
        public static TimeSpan Timespan(this DiagnosticsStopwatchLap lap)
        {
            return lap.EndTimestamp - lap.StartTimestamp;
        }
    }
}
