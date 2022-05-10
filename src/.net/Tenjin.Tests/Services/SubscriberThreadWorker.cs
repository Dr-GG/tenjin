using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tenjin.Extensions;
using Tenjin.Implementations.Messaging.PublisherSubscriber;
using Tenjin.Interfaces.Messaging.PublishSubscriber;
using Tenjin.Models.Messaging.PublisherSubscriber;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.UtilitiesTests;

namespace Tenjin.Tests.Services
{
    public class SubscriberThreadWorker
    {
        private const int MinimumNumberOfPublishes = 2;
        private const int MaximumNumberOfPublishes = 5;
        private const int MinimumRandomMilliseconds = 10;
        private const int MaximumRandomMilliseconds = 100;

        private int _subscribeOffset;
        private int _unsubscribeOffset;
        private int _publishOffset;
        private IPublisherLock? _publisherLock;
        private readonly object _root = new();
        private readonly List<PublishEvent<TestPublishData>> _publishedEventsReceived = new();
        private readonly List<TestPublishData> _publishedDataSent = new();
        private readonly IPublisher<TestPublishData> _publisher;
        private readonly SubscriberHook<TestPublishData> _hook;

        public SubscriberThreadWorker(IPublisher<TestPublishData> publisher)
        {
            _publisher = publisher;
            _hook = new SubscriberHook<TestPublishData>(this, OnNext);
        }

        public bool ReceivedPublishedEvents => _publishedEventsReceived.IsNotEmpty();
        public bool ReceivedNoPublishedEvents => _publishedEventsReceived.IsEmpty();
        public int NumberOfPublishes { get; private set; }
        public IEnumerable<PublishEvent<TestPublishData>> PublishedEventsReceived => _publishedEventsReceived;
        public IEnumerable<TestPublishData> PublishedDataSent => _publishedDataSent;

        public bool PublishData { get; set; }
        public bool UnsubscribeWithLock { get; set; }

        public async Task Run()
        {
            Initialise();

            await Subscribe();
            await Publish();
            await Unsubscribe();
        }

        private Task OnNext(PublishEvent<TestPublishData> publishEvent)
        {
            lock (_root)
            {
                _publishedEventsReceived.Add(publishEvent);
            }

            return Task.CompletedTask;
        }

        private async Task Subscribe()
        {
            Thread.Sleep(_subscribeOffset);

            _publisherLock = await _publisher.Subscribe(_hook);
        }

        private async Task Publish()
        {
            if (!this.PublishData)
            {
                return;
            }

            for (var i = 0; i < NumberOfPublishes; i++)
            {
                var data = MessagingUtilities.GetRandomTestPublishData();

                _publishedDataSent.Add(data);
                await _publisher.Publish(data);

                Thread.Sleep(_publishOffset);
            }
        }

        private async Task Unsubscribe()
        {
            Thread.Sleep(_unsubscribeOffset);

            if (UnsubscribeWithLock && _publisherLock != null)
            {
                await _publisherLock.DisposeAsync();
            }
            else if (!UnsubscribeWithLock)
            {
                await _publisher.Unsubscribe(_hook);
            }
        }

        private void Initialise()
        {
            _subscribeOffset = GetRandomTimeOffset();
            _unsubscribeOffset = GetRandomTimeOffset();

            if (!PublishData)
            {
                return;
            }

            _publishOffset = GetRandomTimeOffset();
            NumberOfPublishes = GetRandomPublishCount();
        }

        private static int GetRandomTimeOffset()
        {
            var random = new Random();

            return random.Next(MinimumRandomMilliseconds, MaximumRandomMilliseconds + 1);
        }

        private static int GetRandomPublishCount()
        {
            var random = new Random();

            return random.Next(MinimumNumberOfPublishes, MaximumNumberOfPublishes + 1);
        }
    }
}
