using System;

namespace Tenjin.Interfaces.Diagnostics;

/// <summary>
/// An interface that provides a DateTime instance.
/// </summary>
public interface ISystemClockProvider
{
    /// <summary>
    /// Gets the current DateTime instance.
    /// </summary>
    DateTime Now();
}