using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tenjin.Exceptions.Mappers;
using Tenjin.Extensions;
using Tenjin.Implementations.Mappers;
using Tenjin.Interfaces.Mappers;
using Tenjin.Tests.Models.Mappers;

namespace Tenjin.Tests.ImplementationsTests.MappersTesters;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class MapperServiceTests
{
    [Test]
    public void Map_WhenAttemptingToMapWhereTheMapperDoesNotExist_ThrowsAnError()
    {
        var mapperService = GetDefaultMapperService();

        Assert.Throws<TenjinMapperException>(() => mapperService.Map<int, bool>(1));
        Assert.Throws<TenjinMapperException>(() => mapperService.Map<LeftModel, bool>(new LeftModel()));
        Assert.Throws<TenjinMapperException>(() => mapperService.Map<RightModel, bool>(new RightModel()));
        Assert.Throws<TenjinMapperException>(() => mapperService.Map<bool, DateTime>(false));
        Assert.Throws<TenjinMapperException>(() => mapperService.Map<DateTime, bool>(DateTime.MaxValue));
        Assert.Throws<TenjinMapperException>(() => mapperService.Map<int, short>(0));
        Assert.Throws<TenjinMapperException>(() => mapperService.Map<int, long>(0));
    }

    [Test]
    public void MapNullable_WhenAttemptingToMapWhereTheMapperDoesNotExist_ThrowsAnError()
    {
        var mapperService = GetDefaultMapperService();

        Assert.Throws<TenjinMapperException>(() => mapperService.MapNullable<string, LeftModel>(""));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapNullable<LeftModel, string>(new LeftModel()));
    }

    [Test]
    public void MapCollection_WhenAttemptingToMapWhereTheMapperDoesNotExist_ThrowsAnError()
    {
        var mapperService = GetDefaultMapperService();

        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<int, bool>([1]));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<LeftModel, bool>([new LeftModel()]));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<RightModel, bool>([new RightModel()]));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<bool, DateTime>([false]));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<DateTime, bool>([DateTime.MaxValue]));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<int, short>([0]));
        Assert.Throws<TenjinMapperException>(() => mapperService.MapCollection<int, long>([0]));
    }

    [Test]
    public void Map_WhenAttemptingToMapWhereTheMapperExists_MapsTheValue()
    {
        var mapperService = GetDefaultMapperService();

        mapperService.Map<string, int>("1").Should().Be(1);
        mapperService.Map<string, bool>("true").Should().Be(true);
        mapperService.Map<string, double>("123").Should().Be(123);

        mapperService.Map<int, string>(1).Should().Be("1");
        mapperService.Map<bool, string>(true).Should().Be("True");
        mapperService.Map<double, string>(123).Should().Be("123");

        mapperService.Map<short, int>(2).Should().Be(2);
        mapperService.Map<long, int>(3).Should().Be(3);

        var leftModel = new LeftModel { Property1 = 1, Property2 = "2" };
        var rightModel = new RightModel { Property1 = 1, Property2 = "2" };

        mapperService.Map<LeftModel, RightModel>(leftModel).Should().BeEquivalentTo(rightModel);
        mapperService.Map<RightModel, LeftModel>(rightModel).Should().BeEquivalentTo(leftModel);
    }

    [Test]
    public void MapNullable_WhenAttemptingToMapWhereTheMapperExists_MapsTheValue()
    {
        var mapperService = GetDefaultMapperService();

        var leftModel = new LeftModel { Property1 = 1, Property2 = "2" };
        var rightModel = new RightModel { Property1 = 1, Property2 = "2" };

        mapperService.MapNullable<LeftModel, RightModel>(leftModel).Should().BeEquivalentTo(rightModel);
        mapperService.MapNullable<RightModel, LeftModel>(rightModel).Should().BeEquivalentTo(leftModel);

        mapperService.MapNullable<LeftModel, RightModel>(null).Should().BeNull();
        mapperService.MapNullable<RightModel, LeftModel>(null).Should().BeNull();
    }

    [Test]
    public void MapCollection_WhenAttemptingToMapWhereTheMapperExists_MapsTheValue()
    {
        var mapperService = GetDefaultMapperService();

        mapperService.MapCollection<string, int>(["1", "2", "3"]).Should().BeEquivalentTo([1, 2, 3]);
        mapperService.MapCollection<string, bool>(["true", "false", "true"]).Should().BeEquivalentTo([true, false, true]);
        mapperService.MapCollection<string, double>(["123", "456", "789"]).Should().BeEquivalentTo([123, 456, 789]);

        mapperService.MapCollection<int, string>([1, 2, 3]).Should().BeEquivalentTo(["1", "2", "3"]);
        mapperService.MapCollection<bool, string>([true, false, true]).Should().BeEquivalentTo(["True", "False", "True"]);
        mapperService.MapCollection<double, string>([123, 456, 789]).Should().BeEquivalentTo(["123", "456", "789"]);

        mapperService.MapCollection<short, int>([2, 3, 4]).Should().BeEquivalentTo([2, 3, 4]);
        mapperService.MapCollection<long, int>([3, 4, 5]).Should().BeEquivalentTo([3, 4, 5]);

        mapperService.MapCollection<string, int>(null).Should().BeEmpty();
        mapperService.MapCollection<string, bool>(null).Should().BeEmpty();
        mapperService.MapCollection<string, double>(null).Should().BeEmpty();
        mapperService.MapCollection<int, string>(null).Should().BeEmpty();
        mapperService.MapCollection<bool, string>(null).Should().BeEmpty();
        mapperService.MapCollection<double, string>(null).Should().BeEmpty();
        mapperService.MapCollection<short, int>(null).Should().BeEmpty();
        mapperService.MapCollection<long, int>(null).Should().BeEmpty();

        mapperService.MapCollection<string, int>([]).Should().BeEmpty();
        mapperService.MapCollection<string, bool>([]).Should().BeEmpty();
        mapperService.MapCollection<string, double>([]).Should().BeEmpty();
        mapperService.MapCollection<int, string>([]).Should().BeEmpty();
        mapperService.MapCollection<bool, string>([]).Should().BeEmpty();
        mapperService.MapCollection<double, string>([]).Should().BeEmpty();
        mapperService.MapCollection<short, int>([]).Should().BeEmpty();
        mapperService.MapCollection<long, int>([]).Should().BeEmpty();

        var leftModels = new[]
        {
            new LeftModel { Property1 = 1, Property2 = "2" },
            new LeftModel { Property1 = 3, Property2 = "4" },
            new LeftModel { Property1 = 5, Property2 = "6" }
        };

        var rightModels = new[]
        {
            new RightModel { Property1 = 1, Property2 = "2" },
            new RightModel { Property1 = 3, Property2 = "4" },
            new RightModel { Property1 = 5, Property2 = "6" }
        };

        mapperService.MapCollection<LeftModel, RightModel>(leftModels).Should().BeEquivalentTo(rightModels);
        mapperService.MapCollection<RightModel, LeftModel>(rightModels).Should().BeEquivalentTo(leftModels);
        mapperService.MapCollection<LeftModel, RightModel>(null).Should().BeEmpty();
        mapperService.MapCollection<RightModel, LeftModel>(null).Should().BeEmpty();
        mapperService.MapCollection<LeftModel, RightModel>([]).Should().BeEmpty();
        mapperService.MapCollection<RightModel, LeftModel>([]).Should().BeEmpty();
    }

    private static IMapperService GetDefaultMapperService()
    {
        var serviceProvider = GetServiceProvider();

        return new MapperService(serviceProvider);
    }

    private static IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        services.RegisterMappers(typeof(MapperServiceTests).Assembly);

        return services.BuildServiceProvider();
    }
}
