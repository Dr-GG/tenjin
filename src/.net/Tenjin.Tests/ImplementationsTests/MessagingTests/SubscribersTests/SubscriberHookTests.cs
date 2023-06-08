using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Implementations.Messaging.Subscribers;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.Services;
using Tenjin.Tests.Utilities;
using Tenjin.Tests.UtilitiesTests;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests.SubscribersTests;

[TestFixture]
public class SubscriberHookTests
{
    [Test]
    public void Constructor_WhenGivingAnObject_UsesTheHashCodeOfTheObjectForTheId()
    {
        var parentObject = new object();
        var expectedHashCode = parentObject.GetHashCode().ToString();
        var hook = new SubscriberHook<TestPublishData>(parentObject, GetEmptyFunction());

        expectedHashCode.Should().Be(hook.Id);
    }

    [Test]
    public async Task Subscribe_WhenSubscribing_SubscribesToThePublisher()
    {
        var hook = GetSubscriberHook();
        var mockPublisher = GetMockPublisher();

        await hook.Subscribe(mockPublisher.Object);

        mockPublisher.Verify(p => p.Subscribe(hook), Times.Once);
        mockPublisher.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Subscribe_WhenSubscribingTwice_ThrowsAnError()
    {
        var hook = GetSubscriberHook();
        var mockPublisher = GetMockPublisher();

        await hook.Subscribe(mockPublisher.Object);

        var error = Assert.ThrowsAsync<InvalidOperationException>(
            () => hook.Subscribe(mockPublisher.Object))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Subscriber hook already has a publisher.");
    }

    [TestCase(new[] { PublishEventType.Publish }, false, false, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish }, true, false, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish }, false, true, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish }, true, true, true, false, false)]
    [TestCase(new[] { PublishEventType.Error }, false, false, false, false, false)]
    [TestCase(new[] { PublishEventType.Error }, true, false, false, false, false)]
    [TestCase(new[] { PublishEventType.Error }, false, true, false, false, true)]
    [TestCase(new[] { PublishEventType.Error }, true, true, false, false, true)]
    [TestCase(new[] { PublishEventType.Disposing }, false, false, false, false, false)]
    [TestCase(new[] { PublishEventType.Disposing }, true, false, false, true, false)]
    [TestCase(new[] { PublishEventType.Disposing }, false, true, false, false, false)]
    [TestCase(new[] { PublishEventType.Disposing }, true, true, false, true, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error }, false, false, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error }, true, false, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error }, false, true, true, false, true)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error }, true, true, true, false, true)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Disposing }, false, false, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Disposing }, true, false, true, true, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Disposing }, false, true, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Disposing }, true, true, true, true, false)]
    [TestCase(new[] { PublishEventType.Error, PublishEventType.Disposing }, false, false, false, false, false)]
    [TestCase(new[] { PublishEventType.Error, PublishEventType.Disposing }, true, false, false, true, false)]
    [TestCase(new[] { PublishEventType.Error, PublishEventType.Disposing }, false, true, false, false, true)]
    [TestCase(new[] { PublishEventType.Error, PublishEventType.Disposing }, true, true, false, true, true)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error, PublishEventType.Disposing }, false, false, true, false, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error, PublishEventType.Disposing }, true, false, true, true, false)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error, PublishEventType.Disposing }, false, true, true, false, true)]
    [TestCase(new[] { PublishEventType.Publish, PublishEventType.Error, PublishEventType.Disposing }, true, true, true, true, true)]
    public async Task Receive_WhenInputtingCertainTypes_ExecutesTheCorrectAction(
        IEnumerable<PublishEventType> publishEventTypes,
        bool hasOnDispose,
        bool hasOnError,
        bool publishOnNext,
        bool publishOnDisposed,
        bool publishOnError)
    {
        var hookTester = new SubscriberHookTester();
        var hook = GetSubscriberHook(
            hookTester.OnNextAction,
            hasOnDispose ? hookTester.OnDisposeAction : null,
            hasOnError ? hookTester.OnErrorAction : null);
        var publishEvents = publishEventTypes.Select(
            type =>
                new PublishEvent<TestPublishData>
                {
                    Type = type,
                    Id = Guid.NewGuid(),
                    Data = MessagingUtilities.GetRandomTestPublishData()
                }).ToList();

        foreach (var publishEvent in publishEvents)
        {
            await hook.Receive(publishEvent);
        }

        AssertPublishedData(publishEvents, publishOnNext, PublishEventType.Publish, hookTester.OnNextEvent);
        AssertPublishedData(publishEvents, publishOnError, PublishEventType.Error, hookTester.OnErrorEvent);
        AssertPublishedData(publishEvents, publishOnDisposed, PublishEventType.Disposing, hookTester.OnDisposeEvent);
    }

    [Test]
    public void Receive_WhenProvidingAnEventTypeThatIsNotSupported_ThrowsAnException()
    {
        var hook = GetSubscriberHook();
        var publishEvent = new PublishEvent<TestPublishData>
        {
            Type = (PublishEventType)(-1)
        };
        var error = Assert.ThrowsAsync<NotSupportedException>(() => hook.Receive(publishEvent))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("No action relay for publish event type -1.");
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Dispose_WhenSubscribingAndInvokingTheDispose_ItInvokesTheDisposeOfThePublisherLock(bool disposeAsync)
    {
        await TestDisposeCall(true, disposeAsync, 1);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Dispose_WhenNotSubscribingAndInvokingTheDispose_ItDoesNotInvokeAnythingOfTheLockAndStillWorks(bool disposeAsync)
    {
        await TestDisposeCall(false, disposeAsync, 1);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Dispose_WhenNotSubscribingAndInvokingTheDisposeMultipleTimes_ItDoesNotInvokeAnythingOfTheLockAndStillWorks(bool disposeAsync)
    {
        await TestDisposeCall(false, disposeAsync, 10);
    }

    private static async Task TestDisposeCall(bool subscribe, bool disposeAsync, int numberOfDisposeInvokes)
    {
        var mockLock = GetMockPublisherLock();
        var mockPublisher = GetMockPublisher(mockLock);
        var hook = GetSubscriberHook();

        if (subscribe)
        {
            await hook.Subscribe(mockPublisher.Object);
        }

        for (var i = 0; i < numberOfDisposeInvokes; i++)
        {
            if (disposeAsync)
            {
                await DisposeUtilities.DisposeAsync(hook);
            }
            else
            {
                DisposeUtilities.Dispose(hook);
            }
        }

        if (subscribe)
        {
            mockLock.Verify(l => l.Dispose(), Times.Once);
        }

        mockLock.VerifyNoOtherCalls();
    }

    private static void AssertPublishedData(
        IEnumerable<PublishEvent<TestPublishData>> publishEvents,
        bool shouldHavePublishedEvent,
        PublishEventType publishEventType,
        PublishEvent<TestPublishData>? publishedEvent)
    {
        if (shouldHavePublishedEvent)
        {
            var actualEvent = publishEvents.Single(p => p.Type == publishEventType);

            publishedEvent.Should().NotBeNull();
            publishedEvent.Should().Be(actualEvent);
        }
        else
        {
            publishedEvent.Should().BeNull();
        }
    }

    private static Mock<IPublisherLock> GetMockPublisherLock()
    {
        var result = new Mock<IPublisherLock>();

        return result;
    }

    private static Mock<IPublisher<TestPublishData>> GetMockPublisher(
        Mock<IPublisherLock>? mockPublisherLock = null)
    {
        mockPublisherLock ??= GetMockPublisherLock();

        var result = new Mock<IPublisher<TestPublishData>>();

        result
            .Setup(p => p.Subscribe(It.IsAny<ISubscriber<TestPublishData>>()))
            .ReturnsAsync(mockPublisherLock.Object);

        return result;
    }

    private static SubscriberHook<TestPublishData> GetSubscriberHook(
        Func<PublishEvent<TestPublishData>, Task>? onNextAction = null,

        Func<PublishEvent<TestPublishData>, Task>? onDisposeAction = null,
        Func<PublishEvent<TestPublishData>, Task>? onErrorAction = null)
    {
        onNextAction ??= GetEmptyFunction();

        return new SubscriberHook<TestPublishData>(
            new object(),
            onNextAction,
            onDisposeAction,
            onErrorAction);
    }

    private static Func<PublishEvent<TestPublishData>, Task> GetEmptyFunction()
    {
        return _ => Task.CompletedTask;
    }
}