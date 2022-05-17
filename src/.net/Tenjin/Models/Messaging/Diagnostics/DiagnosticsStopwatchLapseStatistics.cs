using System;

namespace Tenjin.Models.Messaging.Diagnostics
{
    public record DiagnosticsStopwatchLapseStatistics : DiagnosticsStopwatchLapse
    {
        public TimeSpan TotalTimespan { get; init; }
        public TimeSpan AverageTimespan { get; init; }
        public DiagnosticsStopwatchLapse SlowestLapse { get; init; } = new();
        public DiagnosticsStopwatchLapse FastestLapse { get; init; } = new();
    }
}
