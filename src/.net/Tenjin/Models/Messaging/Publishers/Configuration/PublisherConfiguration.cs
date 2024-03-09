using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Models.Messaging.Publishers.Configuration;

/// <summary>
/// The configuration structure to be used for IPublisher instances.
/// </summary>
[ExcludeFromCodeCoverage]
public record PublisherConfiguration
{
    /// <summary>
    /// The PublisherThreadConfiguration instance to be used.
    /// </summary>
    public PublisherThreadConfiguration Threading { get; set; } = new();
}