using NUnit.Framework;
using Tenjin.Enums.Messaging;
using Tenjin.Models.Messaging.Configuration;

namespace Tenjin.Tests.ModelsTests.MessagingTests.ConfigurationTests
{
    [TestFixture]
    public class ProgressPublisherConfigurationTests
    {
        [Test]
        public void Constructor_WhenProvidingAFixedInterval_SetsTheTypeAsFixedInterval()
        {
            var config = new ProgressPublisherConfiguration(10ul);

            Assert.AreEqual(ProgressNotificationInterval.FixedInterval, config.Interval);
        }

        [Test]
        public void Constructor_WhenProvidingAPercentageInterval_SetsTheTypeAsPercentageInterval()
        {
            var config = new ProgressPublisherConfiguration(10.0d);

            Assert.AreEqual(ProgressNotificationInterval.PercentageInterval, config.Interval);
        }

        [Test]
        public void Constructor_WhenProvidingNoParameters_SetsTheTypeAsNone()
        {
            var config = new ProgressPublisherConfiguration();

            Assert.AreEqual(ProgressNotificationInterval.None, config.Interval);
        }
    }
}
