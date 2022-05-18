using System.Collections.Generic;
using System.Threading.Tasks;
using Tenjin.Models.Diagnostics;

namespace Tenjin.Interfaces.Diagnostics
{
    public interface IDiagnosticsStopwatch
    {
        Task Start(string? name = null);
        Task<DiagnosticsStopwatchLap> Stop();
        Task<DiagnosticsStopwatchLapStatistics> GetStatistics();
        Task<IEnumerable<DiagnosticsStopwatchLap>> GetAllLaps();
    }
}
