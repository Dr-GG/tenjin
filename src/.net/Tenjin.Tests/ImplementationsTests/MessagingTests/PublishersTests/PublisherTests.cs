using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Extensions;
using Tenjin.Implementations.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers;
using Tenjin.Models.Messaging.Publishers.Configuration;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.Services;
using Tenjin.Tests.Utilities;
using Tenjin.Tests.UtilitiesTests;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests.PublishersTests;

[TestFixture]
public class PublisherTests
{
    private const int NumberOfTestSubscribers = 30;
    private const int NumberOfTestRepetitionMethodCalls = 100;
    private const int NumberOfThreadSubscribers = 50;
    private const int NumberOfDefaultPublishEventsSent = 6;

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenSubscribersIsEmpty_ReturnsEmpty(PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var result = await publisher.Subscribe();

        result.Should().BeEmpty();
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task SubscribeAndPublish_WhenSubscribingASingleSubscriber_SubscribesTheSubscriberAndReceivesPublishedEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscriber = GetMockSubscriber();
        var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        publisherLock.Should().NotBeNull();

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task SubscribeAndPublish_WhenSubscribingASingleSubscriberMultipleTimesWithTheSameId_SubscribesTheSubscriberAndReceivesPublishedEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscriber = GetMockSubscriber();

        for (var i = 0; i < NumberOfTestRepetitionMethodCalls; i++)
        {
            await publisher.Subscribe(mockSubscriber.Object);
        }

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task SubscribeAndPublish_WhenSubscribingMultipleSubscribers_SubscribesTheSubscriberAndReceivesPublishedEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToArray();
        var publisherLocks = (await publisher.Subscribe(mockSubscribers.Select(m => m.Object).ToArray())).ToList();

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        publisherLocks.Should().NotBeNull();
        publisherLocks.Should().HaveCount(mockSubscribers.Length);
        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscribers);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task SubscribeAndPublish_WhenSubscribingASingleSubscriberMultipleTimesWithTheSameId_ReturnsDifferentLocks(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscriber = GetMockSubscriber();
        var locks = new List<IPublisherLock>();

        for (var i = 0; i < NumberOfTestRepetitionMethodCalls; i++)
        {
            var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

            locks.Add(publisherLock);
        }

        locks.Should().HaveCount(NumberOfTestRepetitionMethodCalls);
        Assert.IsTrue(locks
            .All(outerLock => locks
                .Count(innerLock => outerLock != innerLock) == NumberOfTestRepetitionMethodCalls - 1));
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task UnsubscribeAndPublish_WhenUnsubscribingASingleSubscriber_RemovesTheSubscriber(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockToRemoveSubscriber = GetMockSubscriber();
        var mockToReceiveSubscriber = GetMockSubscriber(2);

        await publisher.Subscribe(mockToReceiveSubscriber.Object);
        await publisher.Subscribe(mockToRemoveSubscriber.Object);
        await publisher.Unsubscribe(mockToRemoveSubscriber.Object);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToReceiveSubscriber);
        AssertNoCalls(mockToRemoveSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task UnsubscribeAndPublish_WhenUnsubscribingMultipleSubscribers_RemovesTheSubscribers(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockToRemoveSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToArray();
        var mockToReceiveSubscribers = GetMockSubscribers(NumberOfTestSubscribers, NumberOfTestSubscribers).ToArray();
        var toRemoveSubscribers = mockToRemoveSubscribers.Select(m => m.Object).ToArray();
        var toReceiveSubscribers = mockToReceiveSubscribers.Select(m => m.Object).ToArray();

        await publisher.Subscribe(toRemoveSubscribers);
        await publisher.Subscribe(toReceiveSubscribers);
        await publisher.Unsubscribe(toRemoveSubscribers);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToReceiveSubscribers);
        AssertNoCalls(mockToRemoveSubscribers);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task UnsubscribeAndPublish_WhenUnsubscribingASingleSubscriberMultipleTimesWithTheSameId_UnsubscribesTheSubscriberAndReceivesNoPublishedEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockToRemoveSubscriber = GetMockSubscriber();
        var mockToReceiveSubscriber = GetMockSubscriber(2);

        await publisher.Subscribe(mockToReceiveSubscriber.Object);
        await publisher.Subscribe(mockToRemoveSubscriber.Object);

        for (var i = 0; i < NumberOfTestRepetitionMethodCalls; i++)
        {
            await publisher.Unsubscribe(mockToRemoveSubscriber.Object);
        }

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToReceiveSubscriber);
        AssertNoCalls(mockToRemoveSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public void SubscribeAndPublish_WhenNoSubscribersExist_StillContinues(PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var testData_1 = MessagingUtilities.GetRandomTestPublishData();

        Assert.DoesNotThrowAsync(() => publisher.Publish(testData_1));
    }

    [TestCase(PublisherThreadMode.Multi, false)]
    [TestCase(PublisherThreadMode.Single, false)]
    [TestCase(PublisherThreadMode.Multi, true)]
    [TestCase(PublisherThreadMode.Single, true)]
    public async Task Dispose_WhenSubscribersExist_InvokesTheExistingSubscribersWithTheAppropriateDisposeCalls(
        PublisherThreadMode threadMode,
        bool disposeAsync)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToList();

        await publisher.Subscribe(mockSubscribers.Select(m => m.Object).ToArray());

        if (disposeAsync)
        {
            await publisher.DisposeAsync();
        }
        else
        {
            publisher.Dispose();
        }

        AssertSubscribersDisposedCall(mockSubscribers, publisher);
    }

    [TestCase(PublisherThreadMode.Multi, false)]
    [TestCase(PublisherThreadMode.Single, false)]
    [TestCase(PublisherThreadMode.Multi, true)]
    [TestCase(PublisherThreadMode.Single, true)]
    public async Task Dispose_WhenSubscribersExistedAndThenRemoved_DoesNotInvokeAnyPublishedEvents(
        PublisherThreadMode threadMode,
        bool disposeAsync)
    {
        var publisher = GetPublisher(threadMode);
        var mockToRemoveSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToList();
        var mockToReceiveSubscribers = GetMockSubscribers(NumberOfTestSubscribers, NumberOfTestSubscribers).ToList();
        var toRemoveSubscribers = mockToRemoveSubscribers.Select(m => m.Object).ToArray();
        var toReceiveSubscribers = mockToReceiveSubscribers.Select(m => m.Object).ToArray();

        await publisher.Subscribe(toRemoveSubscribers);
        await publisher.Subscribe(toReceiveSubscribers);
        await publisher.Unsubscribe(toRemoveSubscribers);

        if (disposeAsync)
        {
            await publisher.DisposeAsync();
        }
        else
        {
            publisher.Dispose();
        }

        AssertSubscribersDisposedCall(mockToReceiveSubscribers, publisher);

        foreach (var mockSubscriber in mockToRemoveSubscribers)
        {
            AssertNoCalls(mockSubscriber);
        }
    }

    [TestCase(PublisherThreadMode.Multi, false)]
    [TestCase(PublisherThreadMode.Single, false)]
    [TestCase(PublisherThreadMode.Multi, true)]
    [TestCase(PublisherThreadMode.Single, true)]
    public async Task Dispose_WhenCalledMultipleTimes_WorksAsExpectedAndPublishesOnlyOnce(
        PublisherThreadMode threadMode,
        bool disposeAsync)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToList();

        await publisher.Subscribe(mockSubscribers.Select(m => m.Object).ToArray());

        for (var i = 0; i < NumberOfTestRepetitionMethodCalls; i++)
        {
            if (disposeAsync)
            {
                await publisher.DisposeAsync();
            }
            else
            {
                publisher.Dispose();
            }
        }

        AssertSubscribersDisposedCall(mockSubscribers, publisher);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingASingleLockThenPublishingAndDisposingIt_RemovesTheSubscriberAndGetsNoAdditionalPublishEvents(PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscriber = GetMockSubscriber();
        var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscriber);

        await publisherLock.DisposeAsync();
        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingASingleLockAndDisposingIt_RemovesTheSubscriberAndGetsNoPublishEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscriber = GetMockSubscriber();
        var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

        await publisherLock.DisposeAsync();

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertNoCalls(mockSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingMultipleLocksThenPublishingAndDisposingIt_RemovesTheSubscribersAndGetsNoAdditionalPublishEvents(PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToArray();
        var subscribers = mockSubscribers.Select(m => m.Object).ToArray();
        var publisherLocks = await publisher.Subscribe(subscribers);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscribers);

        foreach (var publisherLock in publisherLocks)
        {
            await publisherLock.DisposeAsync();
        }

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscribers);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingMultipleLocksAndDisposingIt_RemovesTheSubscribersAndGetsNoPublishEvents(PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToList();
        var subscribers = mockSubscribers.Select(m => m.Object).ToArray();
        var publisherLocks = await publisher.Subscribe(subscribers);

        foreach (var publisherLock in publisherLocks)
        {
            await publisherLock.DisposeAsync();
        }

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertNoCalls(mockSubscribers);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingMultipleLocksThenPublishingAndThenDisposingHalfOfIt_RemovesTheAppropriateSubscribersAndGetsNoAdditionalPublishEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockToRemoveSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToArray();
        var mockToReceiveSubscribers = GetMockSubscribers(NumberOfTestSubscribers, NumberOfTestSubscribers).ToArray();
        var toRemoveSubscribers = mockToRemoveSubscribers.Select(m => m.Object).ToArray();
        var toReceiveSubscribers = mockToReceiveSubscribers.Select(m => m.Object).ToArray();

        var toRemovePublisherLocks = await publisher.Subscribe(toRemoveSubscribers);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await publisher.Subscribe(toReceiveSubscribers);
        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToReceiveSubscribers);
        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToRemoveSubscribers);

        foreach (var publisherLock in toRemovePublisherLocks)
        {
            await publisherLock.DisposeAsync();
        }

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 2, mockToReceiveSubscribers);
        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToRemoveSubscribers);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingMultipleLocksThenDisposingHalfOfIt_RemovesTheAppropriateSubscribersAndGetsNoAdditionalPublishEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockToRemoveSubscribers = GetMockSubscribers(NumberOfTestSubscribers).ToArray();
        var mockToReceiveSubscribers = GetMockSubscribers(NumberOfTestSubscribers, NumberOfTestSubscribers).ToArray();
        var toRemoveSubscribers = mockToRemoveSubscribers.Select(m => m.Object).ToArray();
        var toReceiveSubscribers = mockToReceiveSubscribers.Select(m => m.Object).ToArray();
        var toRemovePublisherLocks = await publisher.Subscribe(toRemoveSubscribers);

        await publisher.Subscribe(toReceiveSubscribers);

        foreach (var publisherLock in toRemovePublisherLocks)
        {
            await publisherLock.DisposeAsync();
        }

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockToReceiveSubscribers);
        AssertNoCalls(mockToRemoveSubscribers);
    }

    [TestCase(PublisherThreadMode.Multi)]
    [TestCase(PublisherThreadMode.Single)]
    public async Task Subscribe_WhenGettingASingleLockAndDisposingItMultipleTimes_ExecutesSuccessfullyAndRemovesTheSubscriberWithNoAdditionalPublishEvents(
        PublisherThreadMode threadMode)
    {
        var publisher = GetPublisher(threadMode);
        var mockSubscriber = GetMockSubscriber();
        var publisherLock = await publisher.Subscribe(mockSubscriber.Object);

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscriber);

        for (var i = 0; i < NumberOfTestRepetitionMethodCalls; i++)
        {
            await publisherLock.DisposeAsync();
        }

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        AssertDefaultPublishEvents(publisher, testData_1, testData_2, testData_3, 1, mockSubscriber);
    }

    [TestCase(PublisherThreadMode.Multi, false)]
    [TestCase(PublisherThreadMode.Single, false)]
    [TestCase(PublisherThreadMode.Multi, true)]
    [TestCase(PublisherThreadMode.Single, true)]
    public async Task SubscribeUnsubscribe_WhenRunningMultipleThreadsWithRandomSubscribeAndUnsubscribeThenPublish_NoSubscribersShouldHaveAnyPublishedEvents(
        PublisherThreadMode threadMode,
        bool disposeUsingLock)
    {
        var publisher = GetPublisher(threadMode);
        var threadWorkers = GetSubscriberThreadWorkers(publisher, false, disposeUsingLock).ToList();

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        threadWorkers
            .Select(t => t.ToFunctionTask(t.Run))
            .RunParallel();

        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        threadWorkers.All(t => t.ReceivedNoPublishedEvents).Should().BeTrue();
    }

    [TestCase(PublisherThreadMode.Multi, false)]
    [TestCase(PublisherThreadMode.Single, false)]
    [TestCase(PublisherThreadMode.Multi, true)]
    [TestCase(PublisherThreadMode.Single, true)]
    public async Task SubscribePublishUnsubscribe_WhenRunningMultipleThreadsWithRandomSubscribeAndPublishingAndUnsubscribe_AllSubscribersShouldHaveSomeData(
        PublisherThreadMode threadMode,
        bool disposeUsingLock)
    {
        var publisher = GetPublisher(threadMode);
        var threadWorkers = GetSubscriberThreadWorkers(publisher, true, disposeUsingLock).ToList();
        var controlCheckSubscriber = GetMockSubscriber(-1);

        await publisher.Subscribe(controlCheckSubscriber.Object);

        threadWorkers
            .Select(t => t.ToFunctionTask(t.Run))
            .RunParallel();

        AssertSubscribeThreadWorkers(controlCheckSubscriber, threadWorkers);
    }

    [Test]
    public async Task Publish_WhenRunningOnSingleThreadMode_AllSubscribersExecuteSequentiallyAndOnOneThread()
    {
        var publisher = GetPublisher(PublisherThreadMode.Single);
        var concurrencyLock = new object();
        var orderOfReceivedSubscribers = new List<ISubscriber<TestPublishData>>();
        var threadIdCounts = new Dictionary<int, int>();
        var mockSubscribers = GetThreadSensitiveMockSubscribers(concurrencyLock, orderOfReceivedSubscribers, threadIdCounts).ToList();
        var subscribers = mockSubscribers.Select(m => m.Object).ToArray();


        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await publisher.Subscribe(subscribers);
        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        // This test code ensures that the subscribers were called in sequence as expected on a single thread.
        for (var i = 0; i < NumberOfDefaultPublishEventsSent; i++)
        {
            var orderIndexOffset = 0;

            foreach (var subscriber in subscribers)
            {
                var receivedSubscriber = orderOfReceivedSubscribers[orderIndexOffset++];

                Assert.AreEqual(receivedSubscriber, subscriber);
            }
        }

        threadIdCounts.Should().HaveCount(1);
        orderOfReceivedSubscribers.Should().HaveCount(threadIdCounts[threadIdCounts.Keys.First()]);
    }

    [Test]
    public async Task Publish_WhenRunningOnMultiThreadMode_AllSubscribersExecuteNonSequentiallyAndOnMultipleThread()
    {
        var publisher = GetPublisher(PublisherThreadMode.Multi);
        var concurrencyLock = new object();
        var orderOfReceivedSubscribers = new List<ISubscriber<TestPublishData>>();
        var threadIdCounts = new Dictionary<int, int>();
        var mockSubscribers = GetThreadSensitiveMockSubscribers(concurrencyLock, orderOfReceivedSubscribers, threadIdCounts).ToList();
        var subscribers = mockSubscribers.Select(m => m.Object).ToArray();

        GetDefaultPublishData(out var testData_1, out var testData_2, out var testData_3);

        await publisher.Subscribe(subscribers);
        await PublishDefaultData(publisher, testData_1, testData_2, testData_3);

        var threadCounts = threadIdCounts.Values.Sum();

        threadIdCounts.Should().HaveCountGreaterThan(1);
        orderOfReceivedSubscribers.Should().HaveCount(threadCounts);
    }

    [Test]
    public void Publish_WhenAnInvalidThreadingConfigurationModeWasProvided_ThrowsAnError()
    {
        var publisher = GetPublisher(PublisherThreadMode.Multi);
        var invalidConfiguration = new PublisherConfiguration
        {
            Threading = new PublisherThreadConfiguration
            {
                Mode = (PublisherThreadMode)(-1)
            }
        };

        publisher.Configure(invalidConfiguration);

        var error = Assert.ThrowsAsync<NotSupportedException>(() => publisher.Publish(new TestPublishData()))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("No dispatch method found for threading mode -1.");
    }

    private static void AssertSubscribeThreadWorkers(
        Mock<ISubscriber<TestPublishData>> controlCheckSubscriber,
        IEnumerable<SubscriberThreadWorker> workers)
    {
        var enumeratedWorkers = workers.ToList();
        var totalPublishes = enumeratedWorkers.Sum(w => w.NumberOfPublishes);

        Assert.IsTrue(enumeratedWorkers.All(t => t.ReceivedPublishedEvents));

        controlCheckSubscriber
            .Verify(c =>
                    c.Receive(It.IsAny<PublishEvent<TestPublishData>>()),
                Times.Exactly(totalPublishes));

        foreach (var threadWorker in enumeratedWorkers)
        {
            AssertSubscribeThreadWorker(controlCheckSubscriber, threadWorker);
        }
    }

    private static void AssertSubscribeThreadWorker(
        Mock<ISubscriber<TestPublishData>> controlCheckSubscriber,
        SubscriberThreadWorker worker)
    {
        var receivedEvents = worker.PublishedEventsReceived.ToList();
        var sentData = worker.PublishedDataSent.ToList();
        var controlCheckValidateFunction = (
                PublishEvent<TestPublishData> publishEvent,
                TestPublishData publishedData) =>
            publishEvent.Data != null
            && publishEvent.Data.Value1 == publishedData.Value1
            && publishEvent.Data.Value2 == publishedData.Value2;

        receivedEvents.Should().HaveCountGreaterThanOrEqualTo(worker.NumberOfPublishes);

        foreach (var data in sentData)
        {
            receivedEvents.SingleOrDefault(r => controlCheckValidateFunction(r, data)).Should().NotBeNull();

            controlCheckSubscriber
                .Verify(c => c.Receive(It.Is<PublishEvent<TestPublishData>>(
                        publishedEvent => controlCheckValidateFunction(publishedEvent, data))),
                    Times.Once);
        }
    }

    private static void AssertDefaultPublishEvents(
        IPublisher<TestPublishData> publisher,
        TestPublishData testData_1,
        TestPublishData testData_2,
        TestPublishData testData_3,
        int factor,
        params Mock<ISubscriber<TestPublishData>>[] mockSubscribers)
    {
        foreach (var mockSubscriber in mockSubscribers)
        {
            AssertSubscriberPublishedCall(mockSubscriber, publisher, testData_1, 1 * factor);
            AssertSubscriberPublishedCall(mockSubscriber, publisher, testData_2, 2 * factor);
            AssertSubscriberPublishedCall(mockSubscriber, publisher, testData_3, 3 * factor);
        }
    }

    private static void AssertSubscribersDisposedCall(
        IEnumerable<Mock<ISubscriber<TestPublishData>>> mockSubscribers,
        IPublisher<TestPublishData> publisher)
    {
        foreach (var mockSubscriber in mockSubscribers)
        {
            AssertReceiveCall(
                mockSubscriber,
                publisher,
                PublishEventType.Disposing,
                null,
                1);
        }
    }

    private static void AssertSubscriberPublishedCall(
        Mock<ISubscriber<TestPublishData>> mockSubscriber,
        IPublisher<TestPublishData> publisher,
        TestPublishData testData,
        int numberOfInvokes)
    {
        AssertReceiveCall(
            mockSubscriber,
            publisher,
            PublishEventType.Publish,
            testData,
            numberOfInvokes);
    }

    private static void AssertReceiveCall(
        Mock<ISubscriber<TestPublishData>> mockSubscriber,
        IPublisher<TestPublishData> publisher,
        PublishEventType eventType,
        TestPublishData? testData,
        int numberOfInvokes)
    {
        var now = DateTime.UtcNow;
        var validateFunction = (PublishEvent<TestPublishData> publishEvent) =>
            Equals(testData, publishEvent.Data)
            && publishEvent.Type == eventType
            && publishEvent.Id.DoesNotEqual(Guid.Empty)
            && publishEvent.Source.Equals(publisher)
            && publishEvent.CreateTimestamp <= now
            && publishEvent.DispatchTimestamp >= publishEvent.CreateTimestamp;

        mockSubscriber.Verify(m => m.Receive(
                It.Is<PublishEvent<TestPublishData>>(
                    publishEvent => validateFunction(publishEvent))),
            Times.Exactly(numberOfInvokes));
    }

    private static void AssertNoCalls(IEnumerable<Mock<ISubscriber<TestPublishData>>> mockSubscribers)
    {
        foreach (var mockSubscriber in mockSubscribers)
        {
            AssertNoCalls(mockSubscriber);
        }
    }

    private static void AssertNoCalls(Mock<ISubscriber<TestPublishData>> mockSubscriber)
    {
        mockSubscriber.Verify(m => m.Receive(It.IsAny<PublishEvent<TestPublishData>>()), Times.Never);
    }

    private static IPublisher<TestPublishData> GetPublisher(PublisherThreadMode threadMode)
    {
        return new Publisher<TestPublishData>()
            .Configure(new PublisherConfiguration
            {
                Threading = new PublisherThreadConfiguration
                {
                    Mode = threadMode
                }
            });
    }

    private static IEnumerable<Mock<ISubscriber<TestPublishData>>> GetMockSubscribers(
        int numberOfSubscribers,
        int idOffset = 0)
    {
        var result = new List<Mock<ISubscriber<TestPublishData>>>(numberOfSubscribers);

        for (var i = 0; i < numberOfSubscribers; i++)
        {
            result.Add(GetMockSubscriber(idOffset + i + 1));
        }

        return result;
    }

    private static IEnumerable<Mock<ISubscriber<TestPublishData>>> GetThreadSensitiveMockSubscribers(
        object concurrencyLock,
        ICollection<ISubscriber<TestPublishData>> orderOfReceivedSubscribers,
        IDictionary<int, int> threadIdCounts)
    {
        var mockSubscribers = GetMockSubscribers(NumberOfThreadSubscribers).ToList();

        foreach (var mockSubscriber in mockSubscribers)
        {
            mockSubscriber
                .Setup(m => m.Receive(It.IsAny<PublishEvent<TestPublishData>>()))
                .Callback(() =>
                {
                    lock (concurrencyLock)
                    {
                        ThreadingUtilities.IncreaseThreadIdDictionary(threadIdCounts);

                        orderOfReceivedSubscribers.Add(mockSubscriber.Object);
                    }
                });
        }

        return mockSubscribers;
    }

    private static void GetDefaultPublishData(
        out TestPublishData testData_1,
        out TestPublishData testData_2,
        out TestPublishData testData_3)
    {
        testData_1 = MessagingUtilities.GetRandomTestPublishData();
        testData_2 = MessagingUtilities.GetRandomTestPublishData();
        testData_3 = MessagingUtilities.GetRandomTestPublishData();
    }

    private static async Task PublishDefaultData(
        IPublisher<TestPublishData> publisher,
        TestPublishData testData_1,
        TestPublishData testData_2,
        TestPublishData testData_3)
    {
        await publisher.Publish(testData_1);
        await publisher.Publish(testData_2);
        await publisher.Publish(testData_2);
        await publisher.Publish(testData_3);
        await publisher.Publish(testData_3);
        await publisher.Publish(testData_3);
    }

    private static Mock<ISubscriber<TestPublishData>> GetMockSubscriber(int id = 1)
    {
        var result = new Mock<ISubscriber<TestPublishData>>();

        result
            .SetupGet(m => m.Id)
            .Returns(id.ToString());

        result
            .Setup(m => m.Receive(It.IsAny<PublishEvent<TestPublishData>>()))
            .Returns(Task.CompletedTask);

        return result;
    }

    private static IEnumerable<SubscriberThreadWorker> GetSubscriberThreadWorkers(
        IPublisher<TestPublishData> publisher,
        bool publishData,
        bool disposeUsingLock)
    {
        var result = new List<SubscriberThreadWorker>(NumberOfThreadSubscribers);

        for (var i = 0; i < NumberOfThreadSubscribers; i++)
        {
            var worker = new SubscriberThreadWorker(publisher)
            {
                PublishData = publishData,
                UnsubscribeWithLock = disposeUsingLock
            };

            result.Add(worker);
        }

        return result;
    }
}