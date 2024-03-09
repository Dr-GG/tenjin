namespace Tenjin.Interfaces.Mappers;

/// <summary>
/// An interface for dependency injection for IBinaryMapper interfaces in the IMapperService.
/// </summary>
public interface IDependencyInjectionBinaryMapper;

/// <summary>
/// A mapper interface that maps between two object Types.
/// </summary>
public interface IBinaryMapper<TLeft, TRight> : IDependencyInjectionBinaryMapper
{
    /// <summary>
    /// Maps between one type and another type.
    /// </summary>
    TLeft Map(TRight source);

    /// <summary>
    /// Maps between one type and another type.
    /// </summary>
    TRight Map(TLeft source);
}