using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Tenjin.Exceptions.Random;

/// <summary>
/// The exception class that is generated during the random generation of values.
/// </summary>
[Serializable, ExcludeFromCodeCoverage]
public class RandomGenerationException : TenjinException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RandomGenerationException() : base()
    { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RandomGenerationException(string message) : base(message)
    { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RandomGenerationException(string message, Exception internalException) : base(message, internalException)
    { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    protected RandomGenerationException(SerializationInfo info, StreamingContext streamingContext) : base(info, streamingContext)
    { }
}