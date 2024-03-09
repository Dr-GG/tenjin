using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Tenjin.Implementations.Diagnostics;
using Tenjin.Implementations.Mappers;
using Tenjin.Implementations.Messaging.Publishers;
using Tenjin.Interfaces.DependencyInjection;
using Tenjin.Interfaces.Diagnostics;
using Tenjin.Interfaces.Mappers;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Utilities;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extensions for IServiceCollection instances.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds singleton service registrations for all implementations of an interface type in an assembly.
    /// </summary>
    public static IServiceCollection AddSingletonInterfaceTypeDefinitions<TInterface>(
        this IServiceCollection services,
        Assembly assembly)
    {
        return services.AddSingletonInterfaceTypeDefinitions(assembly, typeof(TInterface));
    }

    /// <summary>
    /// Adds singleton service registrations for all implementations of an interface type in an assembly.
    /// </summary>
    public static IServiceCollection AddSingletonInterfaceTypeDefinitions(
        this IServiceCollection services,
        Assembly assembly,
        Type interfaceType)
    {
        var definitions = ReflectionUtilities.GetImplementationInterfaceDefinitions(assembly, interfaceType);

        foreach (var definition in definitions)
        {
            services.AddSingleton(interfaceType, definition);
        }

        return services;
    }

    /// <summary>
    /// Adds singleton service registrations for all implementations of a generic type definition in an assembly.
    /// </summary>
    public static IServiceCollection AddSingletonGenericTypeDefinitions(
        this IServiceCollection services,
        Assembly assembly,
        Type genericTypeDefinition)
    {
        var definitions = ReflectionUtilities.GetImplementationGenericTypeDefinitions(assembly, genericTypeDefinition);

        foreach (var definition in definitions)
        {
            foreach (var interfaceType in definition.Interfaces)
            {
                services.AddSingleton(interfaceType, definition.Implementation);
            }
        }

        return services;
    }

    /// <summary>
    /// Registers an IDependencyInjectionModule with the IServiceCollection.
    /// </summary>
    public static IServiceCollection RegisterModule<TModule>(
        this IServiceCollection services,
        TModule module) where TModule : IDependencyInjectionModule
    {
        module.Register(services);

        return services;
    }

    /// <summary>
    /// Registers all instances from an Assembly that inherits the Tenjin mapper interfaces.
    /// </summary>
    /// <remarks>
    /// All IUnaryMapper and IBinaryMapper instances will be registered as singletons.
    /// </remarks>
    public static IServiceCollection RegisterMappers(this IServiceCollection services, Assembly assembly)
    {
        return services
            .AddSingletonGenericTypeDefinitions(assembly, typeof(IUnaryMapper<,>))
            .AddSingletonGenericTypeDefinitions(assembly, typeof(IBinaryMapper<,>))
            .AddSingletonInterfaceTypeDefinitions<IDependencyInjectionUnaryMapper>(assembly)
            .AddSingletonInterfaceTypeDefinitions<IDependencyInjectionBinaryMapper>(assembly);
    }

    /// <summary>
    /// Registers the IMapperService with the IServiceCollection.
    /// </summary>
    public static IServiceCollection RegisterMapperService(this IServiceCollection services)
    {
        return services.AddSingleton<IMapperService, MapperService>();
    }

    /// <summary>
    /// Registers the ISystemClockProvider.
    /// </summary>
    public static IServiceCollection RegisterSystemClockProvider(this IServiceCollection services, bool useUct = true)
    {
        return services.AddSingleton<ISystemClockProvider>(_ => new SystemClockProvider(useUct));
    }

    /// <summary>
    /// Registers teh DiagnosticsLapStopwatch.
    /// </summary>
    public static IServiceCollection RegisterDiagnosticsWatch(this IServiceCollection services, bool useUct = true)
    {
        return services
            .RegisterSystemClockProvider(useUct)
            .AddScoped<IDiagnosticsLapStopwatch, DiagnosticsLapStopwatch>();
    }

    /// <summary>
    /// Registers the default implementation of the IPublisherRegistry for Autofac.
    /// </summary>
    public static IServiceCollection RegisterPublisherRegistry<TKey, TData>(
        this IServiceCollection services) where TKey : notnull
    {
        return services.AddSingleton<IPublisherRegistry<TKey, TData>, DotNetCorePublisherRegistry<TKey, TData>>();
    }
}
