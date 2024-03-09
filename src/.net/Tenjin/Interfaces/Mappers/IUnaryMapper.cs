namespace Tenjin.Interfaces.Mappers;

/// <summary>
/// An interface for dependency injection for IUnaryMapper interfaces in the IMapperService.
/// </summary>
public interface IDependencyInjectionUnaryMapper;

/// <summary>
/// A mapper interface that maps one object Type to another.
/// </summary>
public interface IUnaryMapper<in TSource, out TDestination> : IDependencyInjectionUnaryMapper
{
    /// <summary>
    /// Maps an object Type to another object Type.
    /// </summary>
    TDestination Map(TSource value);
}