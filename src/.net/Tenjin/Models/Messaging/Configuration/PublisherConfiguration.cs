namespace Tenjin.Models.Messaging.Configuration
{
    public record PublisherConfiguration
    {
        public PublisherThreadConfiguration Threading = new();
    }
}
