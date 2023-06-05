using FluentAssertions;
using NUnit.Framework;
using System;
using Tenjin.Implementations.Diagnostics;
using Tenjin.Interfaces.Diagnostics;

namespace Tenjin.Tests.ImplementationsTests.DiagnosticsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class SystemClockProviderTests
{
    [Test]
    public void Constructor_WhenProvidingNoParameters_ProvidesNonUtcDate()
    {
        AssertNonUtc(new SystemClockProvider());
    }

    [Test]
    public void Constructor_WhenProvidingNonUtc_ProvidesNonUtcDate()
    {
        AssertNonUtc(new SystemClockProvider(false));
    }

    [Test]
    public void Constructor_WhenProvidingUtc_ProvidesUtcDate()
    {
        var provided = new SystemClockProvider(true);
        var realNow = DateTime.UtcNow;
        var providedNow = provided.Now();
        var secondsDifference = (providedNow - realNow).TotalSeconds;

        secondsDifference.Should().BeLessThanOrEqualTo(1.0);
    }

    private static void AssertNonUtc(ISystemClockProvider provider)
    {
        var realNow = DateTime.Now;
        var providedNow = provider.Now();
        var secondsDifference = (providedNow - realNow).TotalSeconds;

        secondsDifference.Should().BeLessThanOrEqualTo(1.0);
    }
}