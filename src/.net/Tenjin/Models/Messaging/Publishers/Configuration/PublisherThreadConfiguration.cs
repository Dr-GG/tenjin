using System.Diagnostics.CodeAnalysis;
using Tenjin.Enums.Messaging;

namespace Tenjin.Models.Messaging.Publishers.Configuration;

/// <summary>
/// The configuration data structure used to configure the threading of an IPublisher instance.
/// </summary>
[ExcludeFromCodeCoverage]
public record PublisherThreadConfiguration
{
    /// <summary>
    /// Depicts the threading mode to be used within IPublisher instances.
    /// </summary>
    public PublisherThreadMode Mode { get; set; }

    /// <summary>
    /// The number of threads to be used.
    /// </summary>
    /// <remarks>
    /// When this property is not set, the number of logical processors will be used.
    /// </remarks>
    public int? NumberOfThreads { get; set; }
}