using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class EnumerableExtensionsTests
{
    private const int BatchListTestSize = 30;

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
        "o"
    };

    [TestCase( 1, false)]
    [TestCase( 2, false)]
    [TestCase( 3, false)]
    [TestCase( 4, false)]
    [TestCase( 5, false)]
    [TestCase( 6, false)]
    [TestCase( 7, false)]
    [TestCase( 8, false)]
    [TestCase( 9, false)]
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
    [TestCase( 1, true)]
    [TestCase( 2, true)]
    [TestCase( 3, true)]
    [TestCase( 4, true)]
    [TestCase( 5, true)]
    [TestCase( 6, true)]
    [TestCase( 7, true)]
    [TestCase( 8, true)]
    [TestCase( 9, true)]
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
        var enumeratedItems = items.ToList();
        var reversedItems = enumeratedItems.ToList();

        if (!reversedItems.Contains(value) || addWhenFound)
        {
            reversedItems.Add(value);
        }

        reversedItems.Sort();
        reversedItems.Reverse();
        enumeratedItems.Reverse();

        var binaryInsertItems = enumeratedItems.AsEnumerable()
            .BinaryInsert(value, (left, right) => ((IComparable)right).CompareTo(left), addWhenFound);

        Assert.AreEqual(reversedItems, binaryInsertItems);
        Assert.AreNotSame(reversedItems, binaryInsertItems);
    }

    private static void AssertBinaryInsert<T>(IEnumerable<T> items, T value, bool addWhenFound)
    {
        var enumeratedItems = items.ToList();
        var originalItems = enumeratedItems.ToList();
        var expectedItems = enumeratedItems.ToList();

        if (!expectedItems.Contains(value) || addWhenFound)
        {
            expectedItems.Add(value);
            expectedItems.Sort();
        }

        var binaryInsertItems = enumeratedItems.AsEnumerable().BinaryInsert(value, addWhenFound);

        Assert.AreEqual(expectedItems, binaryInsertItems);
        Assert.AreNotSame(expectedItems, originalItems);
    }

    [Test]
    public void IsNotEmpty_WhenCollectionIsNull_ReturnsFalse()
    {
        IEnumerable<object>? collection = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.IsFalse(collection.IsNotEmpty());
    }

    [Test]
    public void IsNotEmpty_WhenIEnumerableIsEmpty_ReturnsFalse()
    {
        Assert.IsFalse(new List<object>().IsNotEmpty());
    }

    [Test]
    public void IsNotEmpty_WhenIEnumerableIsNotEmpty_ReturnsTrue()
    {
        var array = new[] { 1, 2, 3 };

        Assert.IsTrue(array.IsNotEmpty());
    }

    [Test]
    public void IsEmpty_WhenCollectionIsNull_ReturnsTrue()
    {
        IEnumerable<object>? collection = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.IsTrue(collection.IsEmpty());
    }

    [Test]
    public void IsEmpty_WhenIEnumerableIsEmpty_ReturnsTrue()
    {
        Assert.IsTrue(new List<object>().IsEmpty());
    }

    [Test]
    public void IsEmpty_WhenIEnumerableIsNotEmpty_ReturnsFalse()
    {
        var array = new[] { 1, 2, 3 };

        Assert.IsFalse(array.IsEmpty());
    }

    [Test]
    public void Batch_WhenProvidingANullEnumerable_ReturnsAnEmptyEnumerable()
    {
        var result = ((IEnumerable<int>)null).Batch(10);

        Assert.IsEmpty(result);
    }

    [Test]
    public void Batch_WhenProvidingAnEmptyEnumerable_ReturnsAnEmptyEnumerable()
    {
        var result = Array.Empty<int>().Batch(10);

        Assert.IsEmpty(result);
    }

    [TestCase(-5)]
    [TestCase(-4)]
    [TestCase(-3)]
    [TestCase(-2)]
    [TestCase(-1)]
    [TestCase(0)]
    public void Batch_WhenProvidingABatchSizeLessThanOne_ThrowsAnArgument(int numberOfBatches)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GetBatchTestList().Batch(numberOfBatches));
    }

    [TestCase(   1,   1, 30, 30)]
    [TestCase(   2,   2, 15, 15)]
    [TestCase(   3,   3, 10, 10)]
    [TestCase(   4,   4,  7,  9)]
    [TestCase(   5,   5,  6,  6)]
    [TestCase(   6,   6,  5,  5)]
    [TestCase(   7,   7,  4,  6)]
    [TestCase(   8,   8,  3,  9)]
    [TestCase(   9,   9,  3,  6)]
    [TestCase(  10,  10,  3,  3)]
    [TestCase(  11,  11,  2, 10)]
    [TestCase(  12,  12,  2,  8)]
    [TestCase(  13,  13,  2,  6)]
    [TestCase(  14,  14,  2,  4)]
    [TestCase(  15,  15,  2,  2)]
    [TestCase(  16,  16,  1, 15)]
    [TestCase(  17,  17,  1, 14)]
    [TestCase(  18,  18,  1, 13)]
    [TestCase(  19,  19,  1, 12)]
    [TestCase(  20,  20,  1, 11)]
    [TestCase(  21,  21,  1, 10)]
    [TestCase(  22,  22,  1,  9)]
    [TestCase(  23,  23,  1,  8)]
    [TestCase(  24,  24,  1,  7)]
    [TestCase(  25,  25,  1,  6)]
    [TestCase(  26,  26,  1,  5)]
    [TestCase(  27,  27,  1,  4)]
    [TestCase(  28,  28,  1,  3)]
    [TestCase(  29,  29,  1,  2)]
    [TestCase(  30,  30,  1,  1)]
    [TestCase(  31,  30, 1,   1)]
    [TestCase(  32,  30, 1,   1)]
    [TestCase(  33,  30, 1,   1)]
    [TestCase(  34,  30, 1,   1)]
    [TestCase(  35,  30, 1,   1)]
    [TestCase( 100,  30, 1,   1)]
    [TestCase( 1000, 30, 1,   1)]
    [TestCase(10000, 30, 1,   1)]
    public void Batch_WhenProvidingAList_BatchesAccordingly(
        int numberOfBatches, 
        int expectedNumberOfBatches, 
        int expectedBatchSize,
        int expectedLastBatchSize)
    {
        var globalCounter = 0;
        var batches = GetBatchTestList().Batch(numberOfBatches).ToList();

        Assert.AreEqual(expectedNumberOfBatches, batches.Count);
        Assert.AreEqual(expectedLastBatchSize, batches.Last().Count());

        for (var i = 0; i < batches.Count; i++)
        {
            var batch = batches[i].ToList();
            var expectedSize = i + 1 < batches.Count
                ? expectedBatchSize
                : expectedLastBatchSize;

            Assert.AreEqual(expectedSize, batch.Count);

            foreach (var number in batch)
            {
                Assert.AreEqual(++globalCounter, number);
            }
        }
    }

    [Test]
    public void LastIndex_WhenCollectionIsNull_TheMethodReturnsMinusOne()
    {
        Assert.AreEqual(-1, ((ICollection<int>?)null).LastIndex());
    }

    [TestCase(0, -1)]
    [TestCase(1, 0)]
    [TestCase(2, 1)]
    [TestCase(3, 2)]
    [TestCase(4, 3)]
    [TestCase(5, 4)]
    public void LastIndex_WhenPopulatingTheListWithItems_TheLastIndexIsCorrect(int numberOfItems, int expectedLastIndex)
    {
        var list = new List<int>();

        for (var i = 0; i < numberOfItems; i++)
        {
            list.Add(i);
        }

        Assert.AreEqual(expectedLastIndex, list.LastIndex());
    }

    private static IEnumerable<int> GetBatchTestList()
    {
        var result = new List<int>();

        for (var i = 0; i < BatchListTestSize; i++)
        {
            result.Add(i + 1);
        }

        return result;
    }
}