using NUnit.Framework;
using Tenjin.Extensions;
using Tenjin.Models.Messaging.Publishers.Progress;

namespace Tenjin.Tests.ExtensionsTests
{
    [TestFixture]
    public class ProgressEventExtensionsTests
    {
        [Test]
        public void Percentage_WhenObjectIsNull_Returns0()
        {
            var result = ((ProgressEvent?)null).Percentage();

            Assert.AreEqual(0, result);
        }

        [TestCase(0ul, 0ul, 0, 0)]
        [TestCase(100ul, 0ul, 0, 0)]
        [TestCase(0ul, 100ul, 0, 0)]
        [TestCase(0ul, 100ul, 0, 0)]
        [TestCase(25ul, 100ul, 0, 25)]
        [TestCase(50ul, 100ul, 0, 50)]
        [TestCase(75ul, 100ul, 0, 75)]
        [TestCase(100ul, 100ul, 0, 100)]
        [TestCase(1ul, 3ul, 3, 33.333)]
        [TestCase(2ul, 3ul, 3, 66.667)]
        [TestCase(3ul, 3ul, 3, 100)]
        [TestCase(1ul, 7ul, 5, 14.28571)]
        [TestCase(2ul, 7ul, 5, 28.57143)]
        [TestCase(3ul, 7ul, 5, 42.85714)]
        [TestCase(4ul, 7ul, 5, 57.14286)]
        [TestCase(5ul, 7ul, 5, 71.42857)]
        [TestCase(6ul, 7ul, 5, 85.71429)]
        [TestCase(7ul, 7ul, 5, 100)]
        public void Percentage_WhenProvidedCertainValues_EqualsTheExpectedValue(
            ulong current,
            ulong total,
            int numberOfDecimals,
            double expected)
        {
            var progress = new ProgressEvent(current, total);

            Assert.AreEqual(expected, progress.Percentage(numberOfDecimals));
        }
    }
}
