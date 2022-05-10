namespace Tenjin.Models.Messaging.PublisherSubscriber.Configuration
{
    public record PublisherConfiguration
    {
        public PublisherThreadConfiguration Threading = new();
    }
}
