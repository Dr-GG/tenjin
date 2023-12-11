using System;
using Tenjin.Interfaces.Diagnostics;

namespace Tenjin.Implementations.Diagnostics;

/// <summary>
/// The default implementation of the ISystemClockProvider interface.
/// </summary>
public class SystemClockProvider(bool useUtc) : ISystemClockProvider
{
    /// <summary>
    /// Creates a new instance using local DateTime values.
    /// </summary>
    public SystemClockProvider() : this(false) { }

    /// <inheritdoc />
    public DateTime Now()
    {
        return useUtc ? DateTime.UtcNow : DateTime.Now;
    }
}