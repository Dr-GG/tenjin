using System;
using Tenjin.Models.Messaging.Diagnostics;

namespace Tenjin.Extensions
{
    public static class DiagnosticsStopwatchLapseExtensions
    {
        public static TimeSpan Timespan(this DiagnosticsStopwatchLapse lapse)
        {
            return lapse.EndTimestamp - lapse.StartTimestamp;
        }
    }
}
