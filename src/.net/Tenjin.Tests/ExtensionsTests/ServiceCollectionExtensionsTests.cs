using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tenjin.Extensions;
using Tenjin.Implementations.Diagnostics;
using Tenjin.Implementations.Mappers;
using Tenjin.Implementations.Messaging.Publishers;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Interfaces.Mappers;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Tests.Mappers;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.Modules;
using Tenjin.Tests.Services;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public static class ServiceCollectionExtensionsTests
{
    [Test]
    public static void RegisterModule_WhenRegisteringModule_ExecutesTheNecessaryMethod()
    {
        var services = new ServiceCollection();
        var module = new DependencyInjectionModule();

        services.RegisterModule(module);

        var provider = services.BuildServiceProvider();

        AssertTypeIsRegistered<ComplexStringBinaryMapper, IBinaryMapper<string, int>>(provider);
        AssertTypeIsRegistered<ComplexStringBinaryMapper, IBinaryMapper<string, double>>(provider);
        AssertTypeIsRegistered<ComplexStringBinaryMapper, IBinaryMapper<string, bool>>(provider);

        AssertTypeIsRegistered<ComplexIntegerUnaryMapper, IUnaryMapper<short, int>>(provider);
        AssertTypeIsRegistered<ComplexIntegerUnaryMapper, IUnaryMapper<long, int>>(provider);

        AssertTypeIsRegistered<ComplexStringUnaryMapper, IUnaryMapper<string, int>>(provider);
        AssertTypeIsRegistered<ComplexStringUnaryMapper, IUnaryMapper<string, double>>(provider);
        AssertTypeIsRegistered<ComplexStringUnaryMapper, IUnaryMapper<string, bool>>(provider);
    }

    [Test]
    public static void RegisterMapperService_WhenInvoked_RegistersTheMapperService()
    {
        var services = new ServiceCollection();

        services.RegisterMapperService();

        var provider = services.BuildServiceProvider();

        AssertTypeIsRegistered<MapperService, IMapperService>(provider);
    }

    [TestCase(true)]
    [TestCase(false)]
    public static void RegisterSystemClockProvider_WhenInvoked_RegistersTheSystemClockProvider(bool useUtc)
    {
        var services = new ServiceCollection();

        services.RegisterSystemClockProvider(useUtc);

        var provider = services.BuildServiceProvider();

        AssertTypeIsRegistered<SystemClockProvider, ISystemClockProvider>(provider);
    }

    [TestCase(true)]
    [TestCase(false)]
    public static void RegisterDiagnosticsWatch_WhenInvoked_RegistersTheSystemClockProvider(bool useUtc)
    {
        var services = new ServiceCollection();

        services.RegisterDiagnosticsWatch(useUtc);

        var provider = services.BuildServiceProvider();

        AssertTypeIsRegistered<SystemClockProvider, ISystemClockProvider>(provider);
        AssertTypeIsRegistered<DiagnosticsLapStopwatch, IDiagnosticsLapStopwatch>(provider);
    }

    [Test]
    public static void RegisterPublisherRegistry_WhenRegisteringARegistry_RegistersAllAppropriatePublishers()
    {
        var services = new ServiceCollection();

        services
            .RegisterPublisherRegistry<string, TestPublishData>()
            .AddSingleton<IDiscoverablePublisher<string, TestPublishData>, TestDiscoverablePublisherStringId01>()
            .AddSingleton<IDiscoverablePublisher<string, TestPublishData>, TestDiscoverablePublisherStringId02>();

        var provider = services.BuildServiceProvider();

        AssertTypeIsRegistered<DotNetCorePublisherRegistry<string, TestPublishData>, IPublisherRegistry<string, TestPublishData>>(provider);

        var registry = provider.GetService<IPublisherRegistry<string, TestPublishData>>();

        registry.Should().NotBeNull();
        registry.Get("01").Should().BeOfType<TestDiscoverablePublisherStringId01>();
        registry.Get("02").Should().BeOfType<TestDiscoverablePublisherStringId02>();
    }

    private static void AssertTypeIsRegistered<TImplementation, TInterface>(IServiceProvider serviceProvider)
    {
        AssertTypeIsRegistered(serviceProvider, typeof(TInterface), typeof(TImplementation));
    }

    private static void AssertTypeIsRegistered(IServiceProvider serviceProvider, Type interfaceType, Type implementationType)
    {
        var result = serviceProvider.GetService(interfaceType);

        result.Should().NotBeNull();
        result.Should().BeOfType(implementationType);
    }
}
