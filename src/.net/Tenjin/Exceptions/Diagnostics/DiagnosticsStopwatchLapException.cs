using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Exceptions.Diagnostics;

/// <summary>
/// The exception that is generated during use of the IDiagnosticsStopwatch.
/// </summary>
[ExcludeFromCodeCoverage]
public class DiagnosticsStopwatchLapException : TenjinException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public DiagnosticsStopwatchLapException() { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public DiagnosticsStopwatchLapException(string message) : base(message) { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public DiagnosticsStopwatchLapException(string message, Exception internalException) : base(message, internalException) { }
}