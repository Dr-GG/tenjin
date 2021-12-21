using NUnit.Framework;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("1", true)]
        [TestCase("12", true)]
        [TestCase("123", true)]
        public void IsNotNullOrEmpty_WhenGivenAnInput_MatchesExpectedOutput(
            string? value,
            bool expectedOutput)
        {
            Assert.AreEqual(expectedOutput, value.IsNotNullOrEmpty());
        }

        [TestCase(null, null, true)]
        [TestCase("", "", true)]
        [TestCase("123", "123", true)]
        [TestCase("tenjin", "TENJIN", true)]
        [TestCase("tEnJiN", "TeNjIn", true)]
        [TestCase("", null, false)]
        [TestCase("1234", "123", false)]
        [TestCase("tenjin-tests", "TENJIN", false)]
        [TestCase("tEnJiN", "TeNjIn-tests", false)]
        public void EqualsOrdinalIgnoreCase_WhenGivenInput_MatchesExpectedOutput(
            string? left, string? right, bool expectedOutput)
        {
            Assert.AreEqual(expectedOutput, left.EqualsOrdinalIgnoreCase(right));
            Assert.AreEqual(expectedOutput, right.EqualsOrdinalIgnoreCase(left));
        }
    }
}
