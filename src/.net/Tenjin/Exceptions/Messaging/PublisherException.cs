﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Exceptions.Messaging;

/// <summary>
/// The exception that is generated in the IPublisher instances.
/// </summary>
[ExcludeFromCodeCoverage]
public class PublisherException : TenjinException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public PublisherException() { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public PublisherException(string message) : base(message) { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public PublisherException(string message, Exception internalException) : base(message, internalException) { }
}
