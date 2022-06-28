namespace Tenjin.Models.Messaging.Publishers.Configuration;

public record PublisherConfiguration
{
    public PublisherThreadConfiguration Threading = new();
}