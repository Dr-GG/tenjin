﻿using System.Collections.Generic;
using System.Linq;
using Tenjin.Interfaces.Mappers;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extensions for IUnaryMapper and IBinaryMapper instances.
/// </summary>
public static class MapperExtensions
{
    /// <summary>
    /// Attempts to map a nullable property between two IUnaryMapper instances.
    /// </summary>
    public static TDestination? MapNullable<TSource, TDestination>(
        this IUnaryMapper<TSource, TDestination> mapper,
        TSource? source)
        where TSource : class
        where TDestination : class
    {
        return source == null
            ? null
            : mapper.Map(source);
    }

    /// <summary>
    /// Attempts to map a nullable property between two IBinaryMapper instances.
    /// </summary>
    public static TLeft? MapNullable<TLeft, TRight>(
        this IBinaryMapper<TLeft, TRight> mapper,
        TRight? source)
        where TRight : class
        where TLeft : class
    {
        return source == null
            ? null
            : mapper.Map(source);
    }

    /// <summary>
    /// Attempts to map a nullable property between two IBinaryMapper instances.
    /// </summary>
    public static TRight? MapNullable<TLeft, TRight>(
        this IBinaryMapper<TLeft, TRight> mapper,
        TLeft? source)
        where TRight : class
        where TLeft : class
    {
        return source == null
            ? null
            : mapper.Map(source);
    }

    /// <summary>
    /// Attempts to map a nullable collection between two IUnaryMapper instances.
    /// </summary>
    public static IEnumerable<TDestination> MapCollection<TSource, TDestination>(
        this IUnaryMapper<TSource, TDestination> mapper,
        IEnumerable<TSource>? source)
    {
        return source?
                .Select(mapper.Map)
                .ToList()
            ?? Enumerable.Empty<TDestination>();
    }

    /// <summary>
    /// Attempts to map a nullable collection between two IBinaryMapper instances.
    /// </summary>
    public static IEnumerable<TLeft> MapCollection<TLeft, TRight>(
        this IBinaryMapper<TLeft, TRight> mapper,
        IEnumerable<TRight>? source)
    {
        return source?
              .Select(mapper.Map)
                   .ToList()
               ?? Enumerable.Empty<TLeft>();
    }

    /// <summary>
    /// Attempts to map a nullable collection between two IBinaryMapper instances.
    /// </summary>
    public static IEnumerable<TRight> MapCollection<TLeft, TRight>(
        this IBinaryMapper<TLeft, TRight> mapper,
        IEnumerable<TLeft>? source)
    {
        return source?
                   .Select(mapper.Map)
                   .ToList()
               ?? Enumerable.Empty<TRight>();
    }
}