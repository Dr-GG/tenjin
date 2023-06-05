using System;
using Tenjin.Interfaces.Diagnostics;

namespace Tenjin.Implementations.Diagnostics;

/// <summary>
/// The default implementation of the ISystemClockProvider interface.
/// </summary>
public class SystemClockProvider : ISystemClockProvider
{
    private readonly bool _useUtc;

    /// <summary>
    /// Creates a new instance using local DateTime values.
    /// </summary>
    public SystemClockProvider() : this(false)
    { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public SystemClockProvider(bool useUtc)
    {
        _useUtc = useUtc;
    }

    /// <inheritdoc />
    public DateTime Now()
    {
        return _useUtc ? DateTime.UtcNow : DateTime.Now;
    }
}