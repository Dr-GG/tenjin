using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Tenjin.Implementations.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.Utilities;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests.PublishersTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class PublisherLockTests
{
    [TestCase(true)]
    [TestCase(false)]
    public Task Dispose_WhenCalled_ItInvokesThePublishUnsubscribe(bool invokeAsync)
    {
        return TestPublisherCall(invokeAsync, 1);
    }

    private static async Task TestPublisherCall(bool invokeAsync, int numberOfDisposeInvokes)
    {
        var mockSubscriber = new Mock<ISubscriber<TestPublishData>>();
        var mockPublisher = new Mock<IPublisher<TestPublishData>>();
        var publisherLock = new PublisherLock<TestPublishData>(
            mockPublisher.Object, mockSubscriber.Object);

        for (var i = 0; i < numberOfDisposeInvokes; i++)
        {
            if (invokeAsync)
            {
                await DisposeUtilities.DisposeAsync(publisherLock);
            }
            else
            {
                DisposeUtilities.Dispose(publisherLock);
            }
        }

        mockPublisher.Verify(p => p.Unsubscribe(mockSubscriber.Object), Times.Once);
        mockPublisher.VerifyNoOtherCalls();
        mockSubscriber.VerifyNoOtherCalls();
    }
}