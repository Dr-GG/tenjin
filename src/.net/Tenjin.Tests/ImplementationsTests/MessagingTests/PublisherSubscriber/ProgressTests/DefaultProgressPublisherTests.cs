using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Tenjin.Enums.Messaging;
using Tenjin.Extensions;
using Tenjin.Implementations.Messaging.PublisherSubscriber.Progress;
using Tenjin.Interfaces.Messaging.PublishSubscriber;
using Tenjin.Interfaces.Messaging.PublishSubscriber.Progress;
using Tenjin.Models.Messaging.PublisherSubscriber;
using Tenjin.Models.Messaging.PublisherSubscriber.Configuration;
using Tenjin.Models.Messaging.PublisherSubscriber.Progress;
using Tenjin.Tests.Utilities;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests.PublisherSubscriber.ProgressTests
{
    public class DefaultProgressPublisherTests
    {
        private const int InitialPublishTotal = 22433;
        private const int NumberOfThreadProgressPublishers = 25;

        [Test]
        public void Configure_WhenProvidingANoneConfiguration_ThrowsNoException()
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration();

            Assert.DoesNotThrow(() => publisher.Configure(configuration));
        }

        [Test]
        public void Configure_WhenProvidingAnInvalidFixedIntervalConfiguration_ThrowsAnException()
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration
            {
                Interval = ProgressNotificationInterval.FixedInterval
            };

            Assert.Throws<InvalidOperationException>(() => publisher.Configure(configuration));
        }

        [Test]
        public void Configure_WhenProvidingAnValidFixedIntervalConfiguration_ThrowsNoException()
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration(10);

            Assert.DoesNotThrow(() => publisher.Configure(configuration));
        }

        [Test]
        public void Configure_WhenProvidingAnInvalidPercentageIntervalConfiguration_ThrowsAnException()
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration
            {
                Interval = ProgressNotificationInterval.PercentageInterval
            };

            Assert.Throws<InvalidOperationException>(() => publisher.Configure(configuration));
        }

        [Test]
        public void Configure_WhenProvidingAValidPercentageIntervalConfiguration_ThrowsNoException()
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration(10);

            Assert.DoesNotThrow(() => publisher.Configure(configuration));
        }

        [Test]
        public async Task Initialise_WhenSettingPublishToTrue_PublishesTheInitialProgressEvent()
        {
            var publisher = GetProgressPublisher();
            var tickEvents = new List<PublishEvent<ProgressEvent>>();
            var subscriber = GetMockSubscriber(tickEvents);

            await publisher.Subscribe(subscriber.Object);
            await publisher.Initialise(InitialPublishTotal);

            Assert.AreEqual(1, tickEvents.Count);
            AssertTicks(InitialPublishTotal, new [] { 0 }, tickEvents);
        }

        [Test]
        public async Task Initialise_WhenSettingPublishToFalse_DoesNotPublishTheInitialProgressEvent()
        {
            var publisher = GetProgressPublisher();
            var tickEvents = new List<PublishEvent<ProgressEvent>>();
            var subscriber = GetMockSubscriber(tickEvents);

            await publisher.Subscribe(subscriber.Object);
            await publisher.Initialise(InitialPublishTotal, false);

            Assert.IsEmpty(tickEvents);
        }

        // Perfect test cases.
        [TestCase(10, 1,  1, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(10, 1,  2, 10, new[] { 2, 4, 6, 8, 10 })]
        [TestCase(10, 1,  3, 10, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 1,  4, 10, new[] { 4, 8, 10 })]
        [TestCase(10, 1,  5, 10, new[] { 5, 10 })]
        [TestCase(10, 1,  6, 10, new[] { 6, 10 })]
        [TestCase(10, 1,  7, 10, new[] { 7, 10 })]
        [TestCase(10, 1,  8, 10, new[] { 8, 10 })]
        [TestCase(10, 1,  9, 10, new[] { 9, 10 })]
        [TestCase(10, 1, 10, 10, new[] { 10 })]
        [TestCase(10, 1, 20, 10, new[] { 10 })]

        // Test cases where there are more tick executes than the total.
        [TestCase(10, 1, 1, 15, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 2, 15, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 3, 15, new[] { 3, 6, 9, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 4, 15, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 5, 15, new[] { 5, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 6, 15, new[] { 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 7, 15, new[] { 7, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 8, 15, new[] { 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 9, 15, new[] { 9, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 10, 15, new[] { 10, 10, 10, 10, 10, 10 })]

        // Perfect test cases with larger ticks.
        [TestCase(10, 2,  1, 5, new[] { 2, 4, 6, 8, 10 })]
        [TestCase(10, 2,  2, 5, new[] { 2, 4, 6, 8, 10 })]
        [TestCase(10, 2,  3, 5, new[] { 4, 8, 10 })]
        [TestCase(10, 2,  4, 5, new[] { 4, 8, 10 })]
        [TestCase(10, 2,  5, 5, new[] { 6, 10 })]
        [TestCase(10, 2,  6, 5, new[] { 6, 10 })]
        [TestCase(10, 2,  7, 5, new[] { 8, 10 })]
        [TestCase(10, 2,  8, 5, new[] { 8, 10 })]
        [TestCase(10, 2,  9, 5, new[] { 10 })]
        [TestCase(10, 2, 10, 5, new[] { 10 })]
        [TestCase(10, 3,  1, 4, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 3,  2, 4, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 3,  3, 4, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 3,  4, 4, new[] { 6, 10 })]
        [TestCase(10, 3,  5, 4, new[] { 6, 10 })]
        [TestCase(10, 3,  6, 4, new[] { 6, 10 })]
        [TestCase(10, 3,  7, 4, new[] { 9, 10 })]
        [TestCase(10, 3,  8, 4, new[] { 9, 10 })]
        [TestCase(10, 3,  9, 4, new[] { 9, 10 })]
        [TestCase(10, 3, 10, 4, new[] { 10 })]

        // Test cases with larger ticks and tick executes than the total.
        [TestCase(10, 2,  1, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  2, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  3, 10, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  4, 10, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  5, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  6, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  7, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  8, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  9, 10, new[] { 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2, 10, 10, new[] { 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  1, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  2, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  3, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  4, 8, new[] { 6, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  5, 8, new[] { 6, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  6, 8, new[] { 6, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  7, 8, new[] { 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  8, 8, new[] { 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  9, 8, new[] { 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3, 10, 8, new[] { 10, 10, 10, 10, 10 })]
        public async Task Tick_WhenConfiguringFixedInterval_ExpectsPublishedIntervals(
            int total, 
            int ticks,
            int interval,
            int numberOfTicks,
            IEnumerable<int> expectedTicks)
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration((ulong)interval);
            var tickEvents = new List<PublishEvent<ProgressEvent>>();
            var mockSubscriber = GetMockSubscriber(tickEvents);

            publisher.Configure(configuration);
            await publisher.Initialise((ulong)total);
            await publisher.Subscribe(mockSubscriber.Object);

            for (var i = 0; i < numberOfTicks; i++)
            {
                await publisher.Tick((ulong)ticks);
            }

            AssertTicks(total, expectedTicks, tickEvents);
        }

        // Perfect test cases with even percentages.
        [TestCase(10, 1,  10, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(10, 1,  20, 10, new[] { 2, 4, 6, 8, 10 })]
        [TestCase(10, 1,  30, 10, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 1,  40, 10, new[] { 4, 8, 10 })]
        [TestCase(10, 1,  50, 10, new[] { 5, 10 })]
        [TestCase(10, 1,  60, 10, new[] { 6, 10 })]
        [TestCase(10, 1,  70, 10, new[] { 7, 10 })]
        [TestCase(10, 1,  80, 10, new[] { 8, 10 })]
        [TestCase(10, 1,  90, 10, new[] { 9, 10 })]
        [TestCase(10, 1, 100, 10, new[] { 10 })]
        [TestCase(10, 1, 200, 10, new[] { 10 })]

        // Test cases where there are more tick executes than the total with even percentages.
        [TestCase(10, 1,  10, 15, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  20, 15, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  30, 15, new[] { 3, 6, 9, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  40, 15, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  50, 15, new[] { 5, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  60, 15, new[] { 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  70, 15, new[] { 7, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  80, 15, new[] { 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1,  90, 15, new[] { 9, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 1, 100, 15, new[] { 10, 10, 10, 10, 10, 10 })]

        // Perfect test cases with larger ticks with even percentages.
        [TestCase(10, 2,  10, 5, new[] { 2, 4, 6, 8, 10 })]
        [TestCase(10, 2,  20, 5, new[] { 2, 4, 6, 8, 10 })]
        [TestCase(10, 2,  30, 5, new[] { 4, 6, 10 })]
        [TestCase(10, 2,  40, 5, new[] { 4, 8, 10 })]
        [TestCase(10, 2,  50, 5, new[] { 6, 10 })]
        [TestCase(10, 2,  60, 5, new[] { 6, 10 })]
        [TestCase(10, 2,  70, 5, new[] { 8, 10 })]
        [TestCase(10, 2,  80, 5, new[] { 8, 10 })]
        [TestCase(10, 2,  90, 5, new[] { 10 })]
        [TestCase(10, 2, 100, 5, new[] { 10 })]
        [TestCase(10, 3,  10, 4, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 3,  20, 4, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 3,  30, 4, new[] { 3, 6, 9, 10 })]
        [TestCase(10, 3,  40, 4, new[] { 6, 9, 10 })]
        [TestCase(10, 3,  50, 4, new[] { 6, 10 })]
        [TestCase(10, 3,  60, 4, new[] { 6, 10 })]
        [TestCase(10, 3,  70, 4, new[] { 9, 10 })]
        [TestCase(10, 3,  80, 4, new[] { 9, 10 })]
        [TestCase(10, 3,  90, 4, new[] { 9, 10 })]
        [TestCase(10, 3, 100, 4, new[] { 10 })]

        // Test cases with larger ticks and tick executes than the total with even percentages.
        [TestCase(10, 2,  10, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  20, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  30, 10, new[] { 4, 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  40, 10, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  50, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  60, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  70, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  80, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2,  90, 10, new[] { 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 2, 100, 10, new[] { 10, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  10, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  20, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  30, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  40, 8, new[] { 6, 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  50, 8, new[] { 6, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  60, 8, new[] { 6, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  70, 8, new[] { 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  80, 8, new[] { 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3,  90, 8, new[] { 9, 10, 10, 10, 10, 10 })]
        [TestCase(10, 3, 100, 8, new[] { 10, 10, 10, 10, 10 })]

        // Test cases with larger ticks and tick executes than the total with uneven percentages.
        [TestCase(10, 1,   5, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(10, 1,  15, 10, new[] { 2, 3, 5, 6, 8, 9, 10 })]
        [TestCase(10, 1,  25, 10, new[] { 3, 5, 8, 10 })]
        [TestCase(10, 1,  35, 10, new[] { 4, 7, 10 })]
        [TestCase(10, 1,  45, 10, new[] { 5, 9, 10 })]
        [TestCase(10, 1,  55, 10, new[] { 6, 10 })]
        [TestCase(10, 1,  65, 10, new[] { 7, 10 })]
        [TestCase(10, 1,  75, 10, new[] { 8, 10 })]
        [TestCase(10, 1,  85, 10, new[] { 9, 10 })]
        [TestCase(10, 1,  95, 10, new[] { 10 })]
        [TestCase(10, 1, 105, 10, new[] { 10 })]
        [TestCase(10, 1, 33, 10, new[] { 4, 7, 10 })]
        [TestCase(10, 1, 66, 10, new[] { 7, 10 })]
        public async Task Tick_WhenConfiguringPercentageInterval_ExpectsPublishedIntervals(
            int total,
            int ticks,
            double percentage,
            int numberOfTicks,
            IEnumerable<int> expectedTicks)
        {
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration(percentage);
            var tickEvents = new List<PublishEvent<ProgressEvent>>();
            var mockSubscriber = GetMockSubscriber(tickEvents);

            publisher.Configure(configuration);
            await publisher.Initialise((ulong)total);
            await publisher.Subscribe(mockSubscriber.Object);

            for (var i = 0; i < numberOfTicks; i++)
            {
                await publisher.Tick((ulong)ticks);
            }

            AssertTicks(total, expectedTicks, tickEvents);
        }

        [Test]
        public async Task Tick_WhenRunningMultipleThreads_ExecutesAsExpected()
        {
            var rootLock = new object();
            var publisher = GetProgressPublisher();
            var configuration = new ProgressPublisherConfiguration();
            var tickEvents = new List<PublishEvent<ProgressEvent>>();
            var mockSubscriber = GetMockSubscriber(tickEvents);
            var tasks = GetProgressThreadPublishersTasks(publisher);
            var threadCountId = new Dictionary<int, int>();

            mockSubscriber
                .Setup(m => m.Receive(It.IsAny<PublishEvent<ProgressEvent>>()))
                .Callback((PublishEvent<ProgressEvent> progressEvent) =>
                {
                    lock (rootLock)
                    {
                        ThreadingUtilities.IncreaseThreadIdDictionary(threadCountId);

                        tickEvents.Add(progressEvent);
                    }
                });

            publisher.Configure(configuration);
            await publisher.Initialise(NumberOfThreadProgressPublishers);
            await publisher.Subscribe(mockSubscriber.Object);

            tasks.RunParallel();

            Assert.Greater(threadCountId.Count, 1);

            var orderedEvents = tickEvents
                .Select(e => e.Data!)
                .OrderBy(e => e.Current)
                .ToList();

            Assert.AreEqual(NumberOfThreadProgressPublishers, orderedEvents.Count);

            for (var i = 0; i < NumberOfThreadProgressPublishers; i++)
            {
                Assert.AreEqual(i + 1, orderedEvents[i]!.Current);
            }
        }

        private static IEnumerable<Func<Task>> GetProgressThreadPublishersTasks(IProgressPublisher<ProgressEvent> publisher)
        {
            var result = new List<Func<Task>>();

            for (var i = 0; i < NumberOfThreadProgressPublishers; i++)
            {
                result.Add(publisher.Tick);
            }

            return result;
        }

        private static void AssertTicks(
            int total,
            IEnumerable<int> expectedTicks, 
            IEnumerable<PublishEvent<ProgressEvent>> publishedTicks)
        {
            var ulongTicks = expectedTicks.Select(t => (ulong) t).ToList();
            var eventTicks = publishedTicks.ToList();

            Assert.AreEqual(ulongTicks.Count, eventTicks.Count);

            for (var i = 0; i < ulongTicks.Count; i++)
            {
                Assert.AreEqual(ulongTicks[i], eventTicks[i].Data!.Current);
                Assert.AreEqual(total, eventTicks[i].Data!.Total);
            }
        }

        private static Mock<ISubscriber<ProgressEvent>> GetMockSubscriber(ICollection<PublishEvent<ProgressEvent>> output)
        {
            var result = new Mock<ISubscriber<ProgressEvent>>();

            result
                .SetupGet(s => s.Id)
                .Returns(result.GetHashCode().ToString());

            result
                .Setup(s => s.Receive(It.IsAny<PublishEvent<ProgressEvent>>()))
                .Callback((PublishEvent<ProgressEvent> publishEvent) => output.Add(publishEvent));

            return result;
        }

        private static IProgressPublisher<ProgressEvent> GetProgressPublisher()
        {
            return new DefaultProgressPublisher();
        }
    }
}
