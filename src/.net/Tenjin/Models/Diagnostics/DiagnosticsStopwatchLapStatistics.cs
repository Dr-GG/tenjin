using System;

namespace Tenjin.Models.Diagnostics
{
    public record DiagnosticsStopwatchLapStatistics : DiagnosticsStopwatchLap
    {
        public TimeSpan TotalTimespan { get; init; }
        public TimeSpan AverageTimespan { get; init; }
        public DiagnosticsStopwatchLap SlowestLap { get; init; } = new();
        public DiagnosticsStopwatchLap FastestLap { get; init; } = new();
    }
}
