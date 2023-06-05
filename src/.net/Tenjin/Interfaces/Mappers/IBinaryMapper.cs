namespace Tenjin.Interfaces.Mappers;

/// <summary>
/// A mapper interface that maps between two object Types.
/// </summary>
public interface IBinaryMapper<TLeft, TRight>
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