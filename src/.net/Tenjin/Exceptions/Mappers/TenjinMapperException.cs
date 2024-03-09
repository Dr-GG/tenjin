using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Exceptions.Mappers;

[ExcludeFromCodeCoverage]
public class TenjinMapperException : TenjinException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinMapperException() { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinMapperException(Type inType, Type outType)
        : this($"Could not find a mapper from '{inType.AssemblyQualifiedName}' to '{outType.AssemblyQualifiedName}'") { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinMapperException(string message) : base(message) { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinMapperException(string message, Exception internalException) : base(message, internalException) { }
}
