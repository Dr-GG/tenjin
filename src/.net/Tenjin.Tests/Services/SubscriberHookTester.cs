using System;
using System.Threading.Tasks;
using Tenjin.Models.Messaging.Publishers;
using Tenjin.Tests.Models.Messaging;

namespace Tenjin.Tests.Services
{
    public class SubscriberHookTester
    {
        public PublishEvent<TestPublishData>? OnNextEvent { get; set; }
        public PublishEvent<TestPublishData>? OnDisposeEvent { get; set; }
        public PublishEvent<TestPublishData>? OnErrorEvent { get; set; }

        public Func<PublishEvent<TestPublishData>, Task> OnNextAction =>
            publishEvent =>
            {
                OnNextEvent = publishEvent;

                return Task.CompletedTask;
            };

        public Func<PublishEvent<TestPublishData>, Task> OnDisposeAction =>
            publishEvent =>
            {
                OnDisposeEvent = publishEvent;

                return Task.CompletedTask;
            };

        public Func<PublishEvent<TestPublishData>, Task> OnErrorAction =>
            publishEvent =>
            {
                OnErrorEvent = publishEvent;

                return Task.CompletedTask;
            };
    }
}
