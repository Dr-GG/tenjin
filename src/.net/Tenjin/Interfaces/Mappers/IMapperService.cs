using System.Collections.Generic;

namespace Tenjin.Interfaces.Mappers;

/// <summary>
/// A generic service for mapping objects.
/// </summary>
public interface IMapperService
{
    /// <summary>
    /// Maps a source object to a destination object.
    /// </summary>
    TOut Map<TIn, TOut>(TIn source);

    /// <summary>
    /// Maps a nullable source object to a destination object.
    /// </summary>
    TOut? MapNullable<TIn, TOut>(TIn? source) where TIn : class where TOut : class;

    /// <summary>
    /// Maps a collection of source objects to a destination object.
    /// </summary>
    IEnumerable<TOut> MapCollection<TIn, TOut>(IEnumerable<TIn>? source);
}
