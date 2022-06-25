using System;
using NUnit.Framework;
using Tenjin.Implementations.Comparers;

namespace Tenjin.Tests.ImplementationsTests.ComparersTests
{
    [TestFixture, Parallelizable(ParallelScope.Children)]
    public class FunctionComparerTests
    {
        private static readonly Func<int, int, int> TestFunctionComparer = (x, y) => x.CompareTo(y);

        [TestCase(null, null, 0)]
        [TestCase(1, null, 1)]
        [TestCase(null, 1, -1)]
        [TestCase(1, 1, 0)]
        [TestCase(2, 1, 1)]
        [TestCase(1, 2, -1)]
        public void CompareNonGeneric_ProvidedInput_GivesExpectedOutput(object? x, object? y, int expectedResult)
        {
            var comparer = GetFunctionComparer();
            var result = comparer.Compare(x, y);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(null, null, 0)]
        [TestCase(1, null, 1)]
        [TestCase(null, 1, -1)]
        [TestCase(1, 1, 0)]
        [TestCase(2, 1, 1)]
        [TestCase(1, 2, -1)]
        public void CompareGeneric_ProvidedInput_GivesExpectedOutput(int? x, int? y, int expectedResult)
        {
            var comparer = GetFunctionComparer();
            var result = comparer.Compare(x, y);

            Assert.AreEqual(expectedResult, result);
        }

        private static FunctionComparer<int> GetFunctionComparer()
        {
            return new FunctionComparer<int>(TestFunctionComparer);
        }
    }
}
