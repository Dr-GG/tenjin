﻿using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Tenjin.Implementations.Messaging;
using Tenjin.Interfaces.Messaging;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.Utilities;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests
{
    [TestFixture]
    public class PublisherLockTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public async Task Dispose_WhenCalled_ItInvokesThePublishUnsubscribe(bool invokeAsync)
        {
            await TestPublisherCall(invokeAsync, 1);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Dispose_WhenCalledMultipleTimes_ItInvokesThePublishUnsubscribeOnlyOnce(bool invokeAsync)
        {
            await TestPublisherCall(invokeAsync, 10);
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
}
