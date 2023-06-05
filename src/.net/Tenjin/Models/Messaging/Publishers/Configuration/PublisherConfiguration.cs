namespace Tenjin.Models.Messaging.Publishers.Configuration;

/// <summary>
/// The configuration structure to be used for IPuiblisher instances.
/// </summary>
public record PublisherConfiguration
{
    /// <summary>
    /// The PublisherThreadConfiguration instance to be used.
    /// </summary>
    public PublisherThreadConfiguration Threading { get; set; } = new();
}