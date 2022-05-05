using System;
using Tenjin.Tests.Models.Messaging;

namespace Tenjin.Tests.UtilitiesTests
{
    public static class MessagingUtilities
    {
        public static TestPublishData GetRandomTestPublishData()
        {
            var random = new Random();

            return new TestPublishData
            {
                Value1 = random.Next(0, int.MaxValue),
                Value2 = Guid.NewGuid()
            };
        }
    }
}
