using FluentAssertions;
using NUnit.Framework;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
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
        expectedOutput.Should().Be(value.IsNotNullOrEmpty());
    }

    [TestCase(null, true)]
    [TestCase("", true)]
    [TestCase(" ", false)]
    [TestCase("1", false)]
    [TestCase("12", false)]
    [TestCase("123", false)]
    public void IsNullOrEmpty_WhenGivenAnInput_MatchesExpectedOutput(
        string? value,
        bool expectedOutput)
    {
        expectedOutput.Should().Be(value.IsNullOrEmpty());
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
        string? left,
        string? right,
        bool expectedOutput)
    {
        expectedOutput.Should().Be(left.EqualsOrdinalIgnoreCase(right));
        expectedOutput.Should().Be(right.EqualsOrdinalIgnoreCase(left));
    }
}