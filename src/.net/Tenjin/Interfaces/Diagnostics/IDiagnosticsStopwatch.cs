using System.Collections.Generic;
using System.Threading.Tasks;
using Tenjin.Models.Messaging.Diagnostics;

namespace Tenjin.Interfaces.Diagnostics
{
    public interface IDiagnosticsStopwatch
    {
        Task Start(string? name = null);
        Task<DiagnosticsStopwatchLapse> Stop();
        Task<DiagnosticsStopwatchLapseStatistics> GetStatistics();
        Task<IEnumerable<DiagnosticsStopwatchLapse>> GetAllLapses();
    }
}
