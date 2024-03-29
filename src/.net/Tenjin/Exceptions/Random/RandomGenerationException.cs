﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Exceptions.Random;

/// <summary>
/// The exception class that is generated during the random generation of values.
/// </summary>
[ExcludeFromCodeCoverage]
public class RandomGenerationException : TenjinException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RandomGenerationException() { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RandomGenerationException(string message) : base(message) { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RandomGenerationException(string message, Exception internalException) : base(message, internalException) { }
}