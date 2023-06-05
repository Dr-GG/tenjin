using Tenjin.Interfaces.Mappers;

namespace Tenjin.Implementations.Mappers;

/// <summary>
/// The default implementation of the IBinaryMapper that uses two IUnaryMapper instances.
/// </summary>
public class BinaryMapper<TLeft, TRight> : IBinaryMapper<TLeft, TRight>
{
    private readonly IUnaryMapper<TLeft, TRight> _leftToRightMapper;
    private readonly IUnaryMapper<TRight, TLeft> _rightToLeftMapper;

    /// <summary>
    /// Creates a new BinaryMapper instance using two IUnaryMapper instances.
    /// </summary>
    public BinaryMapper(
        IUnaryMapper<TLeft, TRight> leftToRightMapper,
        IUnaryMapper<TRight, TLeft> rightToLeftMapper)
    {
        _leftToRightMapper = leftToRightMapper;
        _rightToLeftMapper = rightToLeftMapper;
    }

    /// <inheritdoc />
    public TLeft Map(TRight source)
    {
        return _rightToLeftMapper.Map(source);
    }

    /// <inheritdoc />
    public TRight Map(TLeft source)
    {
        return _leftToRightMapper.Map(source);
    }
}