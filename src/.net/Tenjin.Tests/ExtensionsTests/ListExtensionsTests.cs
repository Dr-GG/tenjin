using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests
{
    [TestFixture, Parallelizable(ParallelScope.Children)]
    public class ListExtensionsTests
    {
        private static readonly IEnumerable<int> BinaryInsertInt32List = new[]
        {
            1,
            2,
            5,
            6,
            9
        };

        private static readonly IEnumerable<string> BinaryInsertStringList = new[]
        {
            "a",
            "c",
            "e",
            "g",
            "i",
            "k",
            "m",
            "o",
        };

        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, false)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        [TestCase(19, false)]
        [TestCase(20, false)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, true)]
        [TestCase(7, true)]
        [TestCase(8, true)]
        [TestCase(9, true)]
        [TestCase(10, true)]
        [TestCase(11, true)]
        [TestCase(12, true)]
        [TestCase(13, true)]
        [TestCase(14, true)]
        [TestCase(15, true)]
        [TestCase(16, true)]
        [TestCase(17, true)]
        [TestCase(18, true)]
        [TestCase(19, true)]
        [TestCase(20, true)]
        public void BinaryInsert_Int32_UsingNoComparerAndNoAddWhenFound_UsesTheDefaultComparerAndDoesNotAddFoundItems(
            int toAddValue,
            bool addIfFound)
        {
            AssertBinaryInsert(BinaryInsertInt32List, toAddValue, addIfFound);
        }

        [TestCase("a", false)]
        [TestCase("b", false)]
        [TestCase("c", false)]
        [TestCase("d", false)]
        [TestCase("e", false)]
        [TestCase("f", false)]
        [TestCase("g", false)]
        [TestCase("h", false)]
        [TestCase("j", false)]
        [TestCase("i", false)]
        [TestCase("j", false)]
        [TestCase("k", false)]
        [TestCase("l", false)]
        [TestCase("m", false)]
        [TestCase("o", false)]
        [TestCase("p", false)]
        [TestCase("q", false)]
        [TestCase("r", false)]
        [TestCase("s", false)]
        [TestCase("t", false)]
        [TestCase("u", false)]
        [TestCase("v", false)]
        [TestCase("w", false)]
        [TestCase("x", false)]
        [TestCase("y", false)]
        [TestCase("z", false)]
        [TestCase("a", true)]
        [TestCase("b", true)]
        [TestCase("c", true)]
        [TestCase("d", true)]
        [TestCase("e", true)]
        [TestCase("f", true)]
        [TestCase("g", true)]
        [TestCase("h", true)]
        [TestCase("j", true)]
        [TestCase("i", true)]
        [TestCase("j", true)]
        [TestCase("k", true)]
        [TestCase("l", true)]
        [TestCase("m", true)]
        [TestCase("o", true)]
        [TestCase("p", true)]
        [TestCase("q", true)]
        [TestCase("r", true)]
        [TestCase("s", true)]
        [TestCase("t", true)]
        [TestCase("u", true)]
        [TestCase("v", true)]
        [TestCase("w", true)]
        [TestCase("x", true)]
        [TestCase("y", true)]
        [TestCase("z", true)]
        public void BinaryInsert_String_UsingNoComparerAndNoAddWhenFound_UsesTheDefaultComparerAndDoesNotAddFoundItems(
            string toAddValue,
            bool addIfFound)
        {
            AssertBinaryInsert(BinaryInsertStringList, toAddValue, addIfFound);
        }

        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, false)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        [TestCase(19, false)]
        [TestCase(20, false)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, true)]
        [TestCase(7, true)]
        [TestCase(8, true)]
        [TestCase(9, true)]
        [TestCase(10, true)]
        [TestCase(11, true)]
        [TestCase(12, true)]
        [TestCase(13, true)]
        [TestCase(14, true)]
        [TestCase(15, true)]
        [TestCase(16, true)]
        [TestCase(17, true)]
        [TestCase(18, true)]
        [TestCase(19, true)]
        [TestCase(20, true)]
        public void BinaryInsert_Int32_UsingAFunctionComparerAndNoAddWhenFound_UsesTheDefaultComparerAndDoesNotAddFoundItems(
            int toAddValue,
            bool addIfFound)
        {
            AssertReversedBinaryInsertWithFunctionComparer(BinaryInsertInt32List, toAddValue, addIfFound);
        }

        [TestCase("a", false)]
        [TestCase("b", false)]
        [TestCase("c", false)]
        [TestCase("d", false)]
        [TestCase("e", false)]
        [TestCase("f", false)]
        [TestCase("g", false)]
        [TestCase("h", false)]
        [TestCase("j", false)]
        [TestCase("i", false)]
        [TestCase("j", false)]
        [TestCase("k", false)]
        [TestCase("l", false)]
        [TestCase("m", false)]
        [TestCase("o", false)]
        [TestCase("p", false)]
        [TestCase("q", false)]
        [TestCase("r", false)]
        [TestCase("s", false)]
        [TestCase("t", false)]
        [TestCase("u", false)]
        [TestCase("v", false)]
        [TestCase("w", false)]
        [TestCase("x", false)]
        [TestCase("y", false)]
        [TestCase("z", false)]
        [TestCase("a", true)]
        [TestCase("b", true)]
        [TestCase("c", true)]
        [TestCase("d", true)]
        [TestCase("e", true)]
        [TestCase("f", true)]
        [TestCase("g", true)]
        [TestCase("h", true)]
        [TestCase("j", true)]
        [TestCase("i", true)]
        [TestCase("j", true)]
        [TestCase("k", true)]
        [TestCase("l", true)]
        [TestCase("m", true)]
        [TestCase("o", true)]
        [TestCase("p", true)]
        [TestCase("q", true)]
        [TestCase("r", true)]
        [TestCase("s", true)]
        [TestCase("t", true)]
        [TestCase("u", true)]
        [TestCase("v", true)]
        [TestCase("w", true)]
        [TestCase("x", true)]
        [TestCase("y", true)]
        [TestCase("z", true)]
        public void BinaryInsert_String_UsingAFunctionComparerAndNoAddWhenFound_UsesTheDefaultComparerAndDoesNotAddFoundItems(
            string toAddValue,
            bool addIfFound)
        {
            AssertReversedBinaryInsertWithFunctionComparer(BinaryInsertStringList, toAddValue, addIfFound);
        }

        private static void AssertReversedBinaryInsertWithFunctionComparer<T>(IEnumerable<T> items, T value, bool addWhenFound)
        {
            var itemsList = items.ToList();
            var reversedItems = itemsList.ToList();

            if (!reversedItems.Contains(value) || addWhenFound)
            {
                reversedItems.Add(value);
            }

            reversedItems.Sort();
            reversedItems.Reverse();
            itemsList.Reverse();

            itemsList
                .BinaryInsert(value, (left, right) => ((IComparable)right).CompareTo(left), addWhenFound);

            Assert.AreEqual(reversedItems, itemsList);
            Assert.AreNotSame(reversedItems, itemsList);
        }

        private static void AssertBinaryInsert<T>(IEnumerable<T> items, T value, bool addWhenFound)
        {
            var itemsList = items.ToList();
            var expectedItems = itemsList.ToList();

            if (!expectedItems.Contains(value) || addWhenFound)
            {
                expectedItems.Add(value);
                expectedItems.Sort();
            }

            itemsList.BinaryInsert(value, addWhenFound);

            Assert.AreEqual(expectedItems, itemsList);
            Assert.AreNotSame(expectedItems, itemsList);
        }
    }
}
