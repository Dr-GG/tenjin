using System;
using NUnit.Framework;
using Tenjin.Implementations.Diagnostics;

namespace Tenjin.Tests.ImplementationsTests.DiagnosticsTests
{
    [TestFixture, Parallelizable(ParallelScope.Children)]
    public class SystemClockProviderTests
    {
        [Test]
        public void Constructor_WhenProvidingNonUtc_ProvidesNonUtcDate()
        {
            var provided = new SystemClockProvider(false);
            var realNow = DateTime.Now;
            var providedNow = provided.Now();
            var secondsDifference = (providedNow - realNow).TotalSeconds;

            Assert.LessOrEqual(secondsDifference, 1.0);
        }

        [Test]
        public void Constructor_WhenProvidingUtc_ProvidesUtcDate()
        {
            var provided = new SystemClockProvider(true);
            var realNow = DateTime.UtcNow;
            var providedNow = provided.Now();
            var secondsDifference = (providedNow - realNow).TotalSeconds;

            Assert.LessOrEqual(secondsDifference, 1.0);
        }
    }
}
