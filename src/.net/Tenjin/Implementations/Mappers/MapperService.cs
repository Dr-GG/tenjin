using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Tenjin.Exceptions.Mappers;
using Tenjin.Interfaces.Mappers;
using Tenjin.Models.Mappers;

namespace Tenjin.Implementations.Mappers;

/// <summary>
/// The default implementation of the IMapperService.
/// </summary>
public class MapperService : IMapperService
{
    private readonly IDictionary<string, MapperServiceRecord> _mapMethods = new Dictionary<string, MapperServiceRecord>();

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public MapperService(IServiceProvider serviceProvider)
    {
        RegisterBinaryMappers(serviceProvider);
        RegisterUnaryMappers(serviceProvider);
    }

    /// <inheritdoc />
    public TOut Map<TIn, TOut>(TIn source)
    {
        var item = GetMapperServiceRecord<TIn, TOut>();
        var result = item.Method.Invoke(item.Mapper, [source])
                  ?? throw new TenjinMapperException(
                         $"The mapper '{item.Mapper.GetType().AssemblyQualifiedName}' mapped to a null instance.");

        return (TOut)result;
    }

    /// <inheritdoc />
    public TOut? MapNullable<TIn, TOut>(TIn? source) where TIn : class where TOut : class
    {
        return source == null
            ? null
            : Map<TIn, TOut>(source);
    }

    /// <inheritdoc />
    public IEnumerable<TOut> MapCollection<TIn, TOut>(IEnumerable<TIn>? source)
    {
        return source == null
            ? Array.Empty<TOut>()
            : source
                .Select(Map<TIn, TOut>)
                .ToList();
    }

    private static string GetKey(Type inType, Type outType)
    {
        return $"{inType.AssemblyQualifiedName}::{outType.AssemblyQualifiedName}";
    }

    private MapperServiceRecord GetMapperServiceRecord<TIn, TOut>()
    {
        var key = GetKey(typeof(TIn), typeof(TOut));

        if (!_mapMethods.TryGetValue(key, out var item))
        {
            throw new TenjinMapperException(typeof(TIn), typeof(TOut));
        }

        return item;
    }

    private void RegisterBinaryMappers(IServiceProvider serviceProvider)
    {
        var binaryMappers = serviceProvider.GetServices<IDependencyInjectionBinaryMapper>();

        foreach (var mapper in binaryMappers)
        {
            RegisterBinaryMapperObject(mapper);
        }
    }

    private void RegisterUnaryMappers(IServiceProvider serviceProvider)
    {
        var unaryMappers = serviceProvider.GetServices<IDependencyInjectionUnaryMapper>();

        foreach (var mapper in unaryMappers)
        {
            RegisterUnaryMapperObject(mapper);
        }
    }

    private void RegisterBinaryMapperObject(object mapper)
    {
        var interfaceTypes = mapper
            .GetType()
            .GetInterfaces()
            .Where(i => i.Name == typeof(IBinaryMapper<,>).Name);

        RegisterMapperObject(mapper, interfaceTypes);
    }

    private void RegisterUnaryMapperObject(object mapper)
    {
        var interfaceTypes = mapper
            .GetType()
            .GetInterfaces()
            .Where(i => i.Name == typeof(IUnaryMapper<,>).Name)
            .ToList();

        RegisterMapperObject(mapper, interfaceTypes);
    }

    private void RegisterMapperObject(object mapper, IEnumerable<Type> interfaceTypes)
    {
        foreach (var interfaceType in interfaceTypes)
        {
            var methods = interfaceType
                .GetMethods()
                .Where(m => m.Name == "Map");

            foreach (var method in methods)
            {
                var outType = method.ReturnType;
                var inType = method.GetParameters()[0].ParameterType;
                var key = GetKey(inType, outType);
                var item = new MapperServiceRecord
                {
                    Mapper = mapper,
                    Method = method
                };

                _mapMethods.TryAdd(key, item);
            }
        }
    }
}
