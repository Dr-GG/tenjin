using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Extensions;
using Tenjin.Implementations.Mappers;
using Tenjin.Interfaces.Mappers;
using Tenjin.Tests.Mappers;
using Tenjin.Tests.Models.Mappers;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class MapperExtensionsTests
{
    [Test]
    public void IUnaryMapper_MapNullable_WhenSourceIsNull_ReturnsNull()
    {
        var unaryMapper = GetLeftToRightMapper();
        var result = unaryMapper.MapNullable(null);

        result.Should().BeNull();
    }

    [Test]
    public void IUnaryMapper_MapCollection_WhenMappingNullCollection_ReturnsEmpty()
    {
        var unaryMapper = GetLeftToRightMapper();
        var result = unaryMapper.MapCollection(null);

        result.Should().BeEmpty();
    }

    [Test]
    public void IUnaryMapper_MapCollection_WhenMappingCollection_ReturnsCollection()
    {
        var unaryMapper = GetLeftToRightMapper();
        var input = new[]
        {
            new LeftModel { Property1 = 1, Property2 = "test-1" },
            new LeftModel { Property1 = 2, Property2 = "test-2" }
        };
        var result = unaryMapper.MapCollection(input).ToList();

        result.Count.Should().Be(2);

        var result1 = result[0];
        var result2 = result[1];

        result1.Property1.Should().Be(1);
        result2.Property1.Should().Be(2);

        result1.Property2.Should().Be("test-1");
        result2.Property2.Should().Be("test-2");
    }

    [Test]
    public void IBinaryMapper_MapNullable_WhenSourceNull_ReturnsNull()
    {
        var mapper = GetBinaryMapper();
        var rightDestination = mapper.MapNullable((LeftModel?)null);
        var leftDestination = mapper.MapNullable((RightModel?)null);

        rightDestination.Should().BeNull();
        leftDestination.Should().BeNull();
    }

    [Test]
    public void IBinaryMapper_MapCollection_WhenMappingNullCollection_ReturnsEmpty()
    {
        var binaryMapper = GetBinaryMapper();
        var rightResult = binaryMapper.MapCollection((IEnumerable<LeftModel>?)null);
        var leftResult = binaryMapper.MapCollection((IEnumerable<RightModel>?)null);

        leftResult.Should().BeEmpty();
        rightResult.Should().BeEmpty();
    }

    [Test]
    public void IBinaryMapper_MapCollection_WhenMappingCollection_ReturnsCollection()
    {
        var binaryMapper = GetBinaryMapper();
        var leftInput = new[]
        {
            new LeftModel { Property1 = 1, Property2 = "test-1" },
            new LeftModel { Property1 = 2, Property2 = "test-2" }
        };
        var rightInput = new[]
        {
            new RightModel { Property1 = 1, Property2 = "test-1" },
            new RightModel { Property1 = 2, Property2 = "test-2" }
        };
        var leftResult = binaryMapper.MapCollection(rightInput).ToList();
        var rightResult = binaryMapper.MapCollection(leftInput).ToList();

        leftResult.Should().HaveCount(2);
        rightResult.Should().HaveCount(2);

        var leftResult1 = leftResult[0];
        var leftResult2 = leftResult[1];

        leftResult1.Should().BeOfType<LeftModel>();
        leftResult2.Should().BeOfType<LeftModel>();
        leftResult1.Property1.Should().Be(1);
        leftResult2.Property1.Should().Be(2);
        leftResult1.Property2.Should().Be("test-1");
        leftResult2.Property2.Should().Be("test-2");

        var rightResult1 = rightResult[0];
        var rightResult2 = rightResult[1];

        rightResult1.Should().BeOfType<RightModel>();
        rightResult2.Should().BeOfType<RightModel>();
        rightResult1.Property1.Should().Be(1);
        rightResult2.Property1.Should().Be(2);
        rightResult1.Property2.Should().Be("test-1");
        rightResult2.Property2.Should().Be("test-2");
    }

    private static IUnaryMapper<LeftModel, RightModel> GetLeftToRightMapper()
    {
        return new LeftToRightMapper();
    }

    private static IUnaryMapper<RightModel, LeftModel> GetRightToLeftMapper()
    {
        return new RightToLeftMapper();
    }

    private static IBinaryMapper<LeftModel, RightModel> GetBinaryMapper()
    {
        return new BinaryMapper<LeftModel, RightModel>(
            GetLeftToRightMapper(), GetRightToLeftMapper());
    }
}