using System;

namespace Tenjin.Exceptions.Diagnostics;

public class DiagnosticsStopwatchException : Exception
{
    public DiagnosticsStopwatchException(string message): base(message)
    { }
}