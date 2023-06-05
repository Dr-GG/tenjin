namespace Tenjin.Interfaces.Mappers;

/// <summary>
/// A mapper interface that maps one object Type to another.
/// </summary>
public interface IUnaryMapper<in TSource, out TDestination>
{
    /// <summary>
    /// Maps an object Type to another object Type.
    /// </summary>
    TDestination Map(TSource value);
}