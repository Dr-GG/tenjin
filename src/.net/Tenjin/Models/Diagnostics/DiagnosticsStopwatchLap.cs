using System;

namespace Tenjin.Models.Diagnostics
{
    public record DiagnosticsStopwatchLap
    {
        public uint Order { get; init; } = 0;
        public string? Name { get; init; }
        public DateTime StartTimestamp { get; init; } = DateTime.MinValue;
        public DateTime EndTimestamp { get; init; } = DateTime.MaxValue;
    }
}
