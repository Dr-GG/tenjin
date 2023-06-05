using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture]
public class DictionaryExtensionsTests
{
    private const string ExistingTestKey1 = "key1";
    private const string ExistingTestKey2 = "key2";
    private const string NonExistingTestKey1 = "non-key1";
    private const string NonExistingTestKey2 = "non-key2";

    [TestCase(ExistingTestKey1, false)]
    [TestCase(ExistingTestKey2, false)]
    [TestCase(NonExistingTestKey1, true)]
    [TestCase(NonExistingTestKey2, true)]
    public void DoesNotContainKey_WhenGivingASpecificKey_ReturnsTheExpectedValue(string key, bool expectedValue)
    {
        var dictionary = new Dictionary<string, string>
        {
            {ExistingTestKey1, "value1"},
            {ExistingTestKey2, "value2"}
        };

        expectedValue.Should().Be(dictionary.DoesNotContainKey(key));
    }
}