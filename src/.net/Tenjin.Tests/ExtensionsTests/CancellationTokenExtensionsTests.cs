using FluentAssertions;
using NUnit.Framework;
using System.Threading;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture]
public class CancellationTokenExtensionsTests
{
    [Test]
    public void CanContinue_WhenCancellationTokenGoesThroughLifeCycle_ReturnsCorrectValue()
    {
        var source = new CancellationTokenSource();
        var token = source.Token;

        token.CanContinue().Should().BeTrue();

        source.Cancel();

        token.CanContinue().Should().BeFalse();
    }
}