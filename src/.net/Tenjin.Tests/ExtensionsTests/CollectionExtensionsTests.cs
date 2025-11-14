using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class CollectionExtensionsTests
{
    [Test]
    public void AddRange_WhenRootCollectionIsNullAndNewCollectionIsNull_ReturnsAnEmptyArray()
    {
        Assert.DoesNotThrow(() => ((ICollection<int>?)null).AddRange(null));
    }

    [Test]
    public void AddRange_WhenRootCollectionIsNotNullAndNewCollectionIsNull_DoesNothing()
    {
        ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        collection.AddRange(null);

        collection.Should().HaveCount(10);
    }

    [Test]
    public void AddRange_WhenRootCollectionIsNotNullAndNewCollectionIsNotNull_ReturnsTheInputCollection()
    {
        ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5 };
        ICollection<int> input = new List<int> { 6, 7, 8, 9, 10 };
        ICollection<int> expected = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        collection.AddRange(input);

        collection.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void RemoveRange_WhenRootCollectionIsNullAndNewCollectionIsNull_ReturnsAnEmptyArray()
    {
        Assert.DoesNotThrow(() => ((ICollection<int>?)null).RemoveRange(null));
    }

    [Test]
    public void RemoveRange_WhenRootCollectionIsNotNullAndNewCollectionIsNull_DoesNothing()
    {
        ICollection<int> collection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        collection.RemoveRange(null);

        collection.Should().HaveCount(10);
    }

    [Test]
    public void RemoveRange_WhenRootCollectionIsNotNullAndNewCollectionIsNotNull_ReturnsTheInputCollection()
    {
        var collection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var input = new List<int> { 6, 7, 8, 9, 10 };
        var expected = new[] { 1, 2, 3, 4, 5 };

        collection.RemoveRange(input);

        collection.Should().BeEquivalentTo(expected);
    }
}