using Tenjin.Interfaces.Mappers;

namespace Tenjin.Implementations.Mappers;

/// <summary>
/// The default implementation of the IBinaryMapper that uses two IUnaryMapper instances.
/// </summary>
public class BinaryMapper<TLeft, TRight>(
    IUnaryMapper<TLeft, TRight> leftToRightMapper,
    IUnaryMapper<TRight, TLeft> rightToLeftMapper) : IBinaryMapper<TLeft, TRight>
{
    /// <inheritdoc />
    public TLeft Map(TRight source)
    {
        return rightToLeftMapper.Map(source);
    }

    /// <inheritdoc />
    public TRight Map(TLeft source)
    {
        return leftToRightMapper.Map(source);
    }
}