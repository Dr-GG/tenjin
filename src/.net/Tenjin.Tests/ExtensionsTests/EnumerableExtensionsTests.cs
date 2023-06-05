using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Extensions;
using Tenjin.Models.Enumerables;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class EnumerableExtensionsTests
{
    private const int BatchListTestSize = 30;

    [Test]
    public void IsNotEmpty_WhenCollectionIsNull_ReturnsFalse()
    {
        IEnumerable<object>? collection = null;

        collection.IsNotEmpty().Should().BeFalse();
    }

    [Test]
    public void IsNotEmpty_WhenIEnumerableIsEmpty_ReturnsFalse()
    {
        new List<object>().IsNotEmpty().Should().BeFalse();
    }

    [Test]
    public void IsNotEmpty_WhenIEnumerableIsNotEmpty_ReturnsTrue()
    {
        var array = new[] { 1, 2, 3 };

        array.IsNotEmpty().Should().BeTrue();
    }

    [Test]
    public void IsEmpty_WhenCollectionIsNull_ReturnsTrue()
    {
        IEnumerable<object>? collection = null;

        collection.IsEmpty().Should().BeTrue();
    }

    [Test]
    public void IsEmpty_WhenIEnumerableIsEmpty_ReturnsTrue()
    {
        new List<object>().IsEmpty().Should().BeTrue();
    }

    [Test]
    public void IsEmpty_WhenIEnumerableIsNotEmpty_ReturnsFalse()
    {
        var array = new[] { 1, 2, 3 };

        array.IsEmpty().Should().BeFalse();
    }

    [Test]
    public void Batch_WhenProvidingANullEnumerable_ReturnsAnEmptyEnumerable()
    {
        var result = ((IEnumerable<int>)null).Batch(10);

        result.Should().BeEmpty();
    }

    [Test]
    public void Batch_WhenProvidingAnEmptyEnumerable_ReturnsAnEmptyEnumerable()
    {
        var result = Array.Empty<int>().Batch(10);

        result.Should().BeEmpty();
    }

    [TestCase(-5)]
    [TestCase(-4)]
    [TestCase(-3)]
    [TestCase(-2)]
    [TestCase(-1)]
    [TestCase(0)]
    public void Batch_WhenProvidingABatchSizeLessThanOne_ThrowsAnArgument(int numberOfBatches)
    {
        var error = Assert.Throws<ArgumentOutOfRangeException>(() => GetBatchTestList().Batch(numberOfBatches))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Argument cannot be zero or less. (Parameter 'numberOfBatches')");
    }

    [TestCase(1, 1, 30, 30)]
    [TestCase(2, 2, 15, 15)]
    [TestCase(3, 3, 10, 10)]
    [TestCase(4, 4, 7, 9)]
    [TestCase(5, 5, 6, 6)]
    [TestCase(6, 6, 5, 5)]
    [TestCase(7, 7, 4, 6)]
    [TestCase(8, 8, 3, 9)]
    [TestCase(9, 9, 3, 6)]
    [TestCase(10, 10, 3, 3)]
    [TestCase(11, 11, 2, 10)]
    [TestCase(12, 12, 2, 8)]
    [TestCase(13, 13, 2, 6)]
    [TestCase(14, 14, 2, 4)]
    [TestCase(15, 15, 2, 2)]
    [TestCase(16, 16, 1, 15)]
    [TestCase(17, 17, 1, 14)]
    [TestCase(18, 18, 1, 13)]
    [TestCase(19, 19, 1, 12)]
    [TestCase(20, 20, 1, 11)]
    [TestCase(21, 21, 1, 10)]
    [TestCase(22, 22, 1, 9)]
    [TestCase(23, 23, 1, 8)]
    [TestCase(24, 24, 1, 7)]
    [TestCase(25, 25, 1, 6)]
    [TestCase(26, 26, 1, 5)]
    [TestCase(27, 27, 1, 4)]
    [TestCase(28, 28, 1, 3)]
    [TestCase(29, 29, 1, 2)]
    [TestCase(30, 30, 1, 1)]
    [TestCase(31, 30, 1, 1)]
    [TestCase(32, 30, 1, 1)]
    [TestCase(33, 30, 1, 1)]
    [TestCase(34, 30, 1, 1)]
    [TestCase(35, 30, 1, 1)]
    [TestCase(100, 30, 1, 1)]
    [TestCase(1000, 30, 1, 1)]
    [TestCase(10000, 30, 1, 1)]
    public void Batch_WhenProvidingAList_BatchesAccordingly(
        int numberOfBatches,
        int expectedNumberOfBatches,
        int expectedBatchSize,
        int expectedLastBatchSize)
    {
        var globalCounter = 0;
        var batches = GetBatchTestList().Batch(numberOfBatches).ToList();

        expectedNumberOfBatches.Should().Be(batches.Count);
        expectedLastBatchSize.Should().Be(batches.Last().Count());

        for (var i = 0; i < batches.Count; i++)
        {
            var batch = batches[i].ToList();
            var expectedSize = i + 1 < batches.Count
                ? expectedBatchSize
                : expectedLastBatchSize;

            expectedSize.Should().Be(batch.Count);

            foreach (var number in batch)
            {
                (++globalCounter).Should().Be(number);
            }
        }
    }

    [Test]
    public void LastIndex_WhenCollectionIsNull_TheMethodReturnsMinusOne()
    {
        ((ICollection<int>?)null).LastIndex().Should().Be(-1);
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

        expectedLastIndex.Should().Be(list.LastIndex());
    }

    [Test]
    public void ForEach_WhenEnumerableIsNull_DoesNothing()
    {
        var output = new List<int>();

        ((IEnumerable<int>?)null).ForEach(i => output.Add(i));

        output.Should().BeEmpty();
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
    public void DoesNotContain_WhenProvidingCertainInput_MatchesExpectedOutput(
        int input,
        bool expectedOutput)
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        list.DoesNotContain(input).Should().Be(expectedOutput);
    }

    [Test]
    public void ForEach_WhenEnumerableIsNotNull_ExecutesTheCodeCorrectly()
    {
        var output = new List<int>();
        IEnumerable<int> input = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        input.ForEach(i => output.Add(i));

        output.Should().BeEquivalentTo(input);
    }

    [Test]
    public void ForLoop_CallsActionForEachItemInEnumerable_WithIndexParameter()
    {
        var moqAction = new Mock<Action<int, int>>();
        var enumerable = new List<int> { 1, 2, 3, 4, 5 };

        enumerable.ForLoop(moqAction.Object);

        moqAction.Verify(v => v.Invoke(0, 1), Times.Once);
        moqAction.Verify(v => v.Invoke(1, 2), Times.Once);
        moqAction.Verify(v => v.Invoke(2, 3), Times.Once);
        moqAction.Verify(v => v.Invoke(3, 4), Times.Once);
        moqAction.Verify(v => v.Invoke(4, 5), Times.Once);
    }

    [Test]
    public void ForLoopWithContext_CallsActionForEachItemInEnumerable_WithIndexParameter()
    {
        var enumerable = new List<int> { 1, 2, 3, 4, 5 };

        enumerable.ForLoopWithContext((context, number) =>
        {
            context.Index.Should().Be(number - 1);

            if (number == 1)
            {
                context.IsFirst.Should().BeTrue();
                context.IsLast.Should().BeFalse();
                context.IsInBetween.Should().BeFalse();
            }
            else if (number == 5)
            {
                context.IsFirst.Should().BeFalse();
                context.IsLast.Should().BeTrue();
                context.IsInBetween.Should().BeFalse();
            }
            else
            {
                context.IsFirst.Should().BeFalse();
                context.IsLast.Should().BeFalse();
                context.IsInBetween.Should().BeTrue();
            }
        });
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