using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Exceptions.Messaging;
using Tenjin.Extensions;
using Tenjin.Implementations.Messaging.Publishers.Progress;
using Tenjin.Interfaces.Messaging.Publishers.Progress;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers;
using Tenjin.Models.Messaging.Publishers.Configuration;
using Tenjin.Models.Messaging.Publishers.Progress;
using Tenjin.Tests.Utilities;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests.PublishersTests.ProgressTests;

public class DefaultProgressPublisherTests
{
    private const int TestInitialPublishTotal = 22433;
    private const int TestTotal = 100;
    private const int NumberOfThreadProgressPublishers = 25;

    [Test]
    public void Configure_WhenProvidingANoneConfiguration_ThrowsNoException()
    {
        var publisher = GetProgressPublisher();
        var configuration = new ProgressPublisherConfiguration();

        Assert.DoesNotThrow(() => publisher.Configure(configuration));
    }

    [Test]
    public void Configure_WhenProvidingAProgressNotificationIntervalThatDoesNotExist_ThrowsAnException()
    {
        var publisher = GetProgressPublisher();
        var configuration = new ProgressPublisherConfiguration
        {
            Interval = (ProgressNotificationInterval)(-1)
        };

        var error = Assert.Throws<NotSupportedException>(() => publisher.Configure(configuration))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("No configuration support for interval -1.");
    }

    [Test]
    public void Configure_WhenProvidingAnInvalidFixedIntervalConfiguration_ThrowsAnException()
    {
        var publisher = GetProgressPublisher();
        var configuration = new ProgressPublisherConfiguration
        {
            Interval = ProgressNotificationInterval.FixedInterval
        };

        var error = Assert.Throws<PublisherException>(() => publisher.Configure(configuration))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Fixed Interval property not set for fixed interval configuration.");
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

        var error = Assert.Throws<PublisherException>(() => publisher.Configure(configuration))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Percentage Interval property not set for fixed percentage interval configuration.");
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
        await publisher.Initialise(TestInitialPublishTotal);

        tickEvents.Should().HaveCount(1);
        AssertTicks(TestInitialPublishTotal, new[] { 0 }, tickEvents);
    }

    [Test]
    public async Task Initialise_WhenSettingPublishToFalse_DoesNotPublishTheInitialProgressEvent()
    {
        var publisher = GetProgressPublisher();
        var tickEvents = new List<PublishEvent<ProgressEvent>>();
        var subscriber = GetMockSubscriber(tickEvents);

        await publisher.Subscribe(subscriber.Object);
        await publisher.Initialise(TestInitialPublishTotal, false);

        tickEvents.Should().BeEmpty();
    }

    [Test]
    public async Task InitialiseAndPublish_WhenInitialisingMultipleTimesWithFixedIntervalConfigurationAndDifferentTotals_ShouldPublishAsExpected()
    {
        var publisher = GetProgressPublisher();
        var configuration = new ProgressPublisherConfiguration(2);

        publisher.Configure(configuration);

        for (var i = 2; i <= TestTotal; i += 2)
        {
            await publisher.Initialise((ulong)i, false);

            var output = new List<PublishEvent<ProgressEvent>>();
            var expectedTicks = new List<int>();
            var mockSubscriber = GetMockSubscriber(output);
            await using var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

            for (var j = 1; j <= i; j++)
            {
                if (j % 2 == 0)
                {
                    expectedTicks.Add(j);
                }

                await publisher.Tick();
            }

            AssertTicks(i, expectedTicks, output);
        }
    }

    [Test]
    public async Task InitialiseAndPublish_WhenInitialisingMultipleTimesWithPercentageIntervalConfigurationAndDifferentTotals_ShouldPublishAsExpected()
    {
        const double percentageInterval = 25.0;
        var publisher = GetProgressPublisher();
        var configuration = new ProgressPublisherConfiguration(percentageInterval);

        publisher.Configure(configuration);

        for (var i = 1; i <= TestTotal; i++)
        {
            await publisher.Initialise((ulong)i, false);

            var currentPercentageLimit = percentageInterval;
            var output = new List<PublishEvent<ProgressEvent>>();
            var expectedTicks = new List<int>();
            var mockSubscriber = GetMockSubscriber(output);
            await using var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

            for (var j = 1; j <= i; j++)
            {
                var percentage = j / (double)i * 100.0;

                if (percentage >= currentPercentageLimit)
                {
                    currentPercentageLimit += percentageInterval;
                    expectedTicks.Add(j);
                }

                await publisher.Tick();
            }

            AssertTicks(i, expectedTicks, output);
        }
    }

    // Perfect test cases.
    [TestCase(10, 1, 1, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    [TestCase(10, 1, 2, 10, new[] { 2, 4, 6, 8, 10 })]
    [TestCase(10, 1, 3, 10, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 1, 4, 10, new[] { 4, 8, 10 })]
    [TestCase(10, 1, 5, 10, new[] { 5, 10 })]
    [TestCase(10, 1, 6, 10, new[] { 6, 10 })]
    [TestCase(10, 1, 7, 10, new[] { 7, 10 })]
    [TestCase(10, 1, 8, 10, new[] { 8, 10 })]
    [TestCase(10, 1, 9, 10, new[] { 9, 10 })]
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
    [TestCase(10, 2, 1, 5, new[] { 2, 4, 6, 8, 10 })]
    [TestCase(10, 2, 2, 5, new[] { 2, 4, 6, 8, 10 })]
    [TestCase(10, 2, 3, 5, new[] { 4, 8, 10 })]
    [TestCase(10, 2, 4, 5, new[] { 4, 8, 10 })]
    [TestCase(10, 2, 5, 5, new[] { 6, 10 })]
    [TestCase(10, 2, 6, 5, new[] { 6, 10 })]
    [TestCase(10, 2, 7, 5, new[] { 8, 10 })]
    [TestCase(10, 2, 8, 5, new[] { 8, 10 })]
    [TestCase(10, 2, 9, 5, new[] { 10 })]
    [TestCase(10, 2, 10, 5, new[] { 10 })]
    [TestCase(10, 3, 1, 4, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 3, 2, 4, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 3, 3, 4, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 3, 4, 4, new[] { 6, 10 })]
    [TestCase(10, 3, 5, 4, new[] { 6, 10 })]
    [TestCase(10, 3, 6, 4, new[] { 6, 10 })]
    [TestCase(10, 3, 7, 4, new[] { 9, 10 })]
    [TestCase(10, 3, 8, 4, new[] { 9, 10 })]
    [TestCase(10, 3, 9, 4, new[] { 9, 10 })]
    [TestCase(10, 3, 10, 4, new[] { 10 })]

    // Test cases with larger ticks and tick executes than the total.
    [TestCase(10, 2, 1, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 2, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 3, 10, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 4, 10, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 5, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 6, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 7, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 8, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 9, 10, new[] { 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 10, 10, new[] { 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 1, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 2, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 3, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 4, 8, new[] { 6, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 5, 8, new[] { 6, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 6, 8, new[] { 6, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 7, 8, new[] { 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 8, 8, new[] { 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 9, 8, new[] { 9, 10, 10, 10, 10, 10 })]
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
    [TestCase(10, 1, 10, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    [TestCase(10, 1, 20, 10, new[] { 2, 4, 6, 8, 10 })]
    [TestCase(10, 1, 30, 10, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 1, 40, 10, new[] { 4, 8, 10 })]
    [TestCase(10, 1, 50, 10, new[] { 5, 10 })]
    [TestCase(10, 1, 60, 10, new[] { 6, 10 })]
    [TestCase(10, 1, 70, 10, new[] { 7, 10 })]
    [TestCase(10, 1, 80, 10, new[] { 8, 10 })]
    [TestCase(10, 1, 90, 10, new[] { 9, 10 })]
    [TestCase(10, 1, 100, 10, new[] { 10 })]
    [TestCase(10, 1, 200, 10, new[] { 10 })]

    // Test cases where there are more tick executes than the total with even percentages.
    [TestCase(10, 1, 10, 15, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 20, 15, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 30, 15, new[] { 3, 6, 9, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 40, 15, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 50, 15, new[] { 5, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 60, 15, new[] { 6, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 70, 15, new[] { 7, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 80, 15, new[] { 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 90, 15, new[] { 9, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 1, 100, 15, new[] { 10, 10, 10, 10, 10, 10 })]

    // Perfect test cases with larger ticks with even percentages.
    [TestCase(10, 2, 10, 5, new[] { 2, 4, 6, 8, 10 })]
    [TestCase(10, 2, 20, 5, new[] { 2, 4, 6, 8, 10 })]
    [TestCase(10, 2, 30, 5, new[] { 4, 6, 10 })]
    [TestCase(10, 2, 40, 5, new[] { 4, 8, 10 })]
    [TestCase(10, 2, 50, 5, new[] { 6, 10 })]
    [TestCase(10, 2, 60, 5, new[] { 6, 10 })]
    [TestCase(10, 2, 70, 5, new[] { 8, 10 })]
    [TestCase(10, 2, 80, 5, new[] { 8, 10 })]
    [TestCase(10, 2, 90, 5, new[] { 10 })]
    [TestCase(10, 2, 100, 5, new[] { 10 })]
    [TestCase(10, 3, 10, 4, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 3, 20, 4, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 3, 30, 4, new[] { 3, 6, 9, 10 })]
    [TestCase(10, 3, 40, 4, new[] { 6, 9, 10 })]
    [TestCase(10, 3, 50, 4, new[] { 6, 10 })]
    [TestCase(10, 3, 60, 4, new[] { 6, 10 })]
    [TestCase(10, 3, 70, 4, new[] { 9, 10 })]
    [TestCase(10, 3, 80, 4, new[] { 9, 10 })]
    [TestCase(10, 3, 90, 4, new[] { 9, 10 })]
    [TestCase(10, 3, 100, 4, new[] { 10 })]

    // Test cases with larger ticks and tick executes than the total with even percentages.
    [TestCase(10, 2, 10, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 20, 10, new[] { 2, 4, 6, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 30, 10, new[] { 4, 6, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 40, 10, new[] { 4, 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 50, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 60, 10, new[] { 6, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 70, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 80, 10, new[] { 8, 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 90, 10, new[] { 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 2, 100, 10, new[] { 10, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 10, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 20, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 30, 8, new[] { 3, 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 40, 8, new[] { 6, 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 50, 8, new[] { 6, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 60, 8, new[] { 6, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 70, 8, new[] { 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 80, 8, new[] { 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 90, 8, new[] { 9, 10, 10, 10, 10, 10 })]
    [TestCase(10, 3, 100, 8, new[] { 10, 10, 10, 10, 10 })]

    // Test cases with larger ticks and tick executes than the total with uneven percentages.
    [TestCase(10, 1, 5, 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    [TestCase(10, 1, 15, 10, new[] { 2, 3, 5, 6, 8, 9, 10 })]
    [TestCase(10, 1, 25, 10, new[] { 3, 5, 8, 10 })]
    [TestCase(10, 1, 35, 10, new[] { 4, 7, 10 })]
    [TestCase(10, 1, 45, 10, new[] { 5, 9, 10 })]
    [TestCase(10, 1, 55, 10, new[] { 6, 10 })]
    [TestCase(10, 1, 65, 10, new[] { 7, 10 })]
    [TestCase(10, 1, 75, 10, new[] { 8, 10 })]
    [TestCase(10, 1, 85, 10, new[] { 9, 10 })]
    [TestCase(10, 1, 95, 10, new[] { 10 })]
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

        threadCountId.Should().HaveCountGreaterThan(1);

        var currentValues = tickEvents
            .Select(e => e.Data!)
            .ToList();

        Assert.AreEqual(NumberOfThreadProgressPublishers, tickEvents.Count);

        for (var i = 0; i < NumberOfThreadProgressPublishers; i++)
        {
            Assert.AreEqual(i + 1, currentValues[i]!.Current);
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
        var ulongTicks = expectedTicks.Select(t => (ulong)t).ToList();
        var eventTicks = publishedTicks.ToList();

        ulongTicks.Count.Should().Be(eventTicks.Count);

        for (var i = 0; i < ulongTicks.Count; i++)
        {
            ulongTicks[i].Should().Be(eventTicks[i].Data!.Current);
            eventTicks[i].Data!.Total.Should().Be((ulong)total);
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