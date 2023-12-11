using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Exceptions;

/// <summary>
/// The base class for all Tenjin related errors or exceptions.
/// </summary>
[ExcludeFromCodeCoverage]
public class TenjinException : Exception
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinException() { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinException(string message) : base(message) { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinException(string message, Exception internalException)
        : base(message, internalException) { }
}
