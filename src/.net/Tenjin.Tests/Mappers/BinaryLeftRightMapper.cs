using Tenjin.Interfaces.Mappers;
using Tenjin.Tests.Models.Mappers;

namespace Tenjin.Tests.Mappers;

public class BinaryLeftRightMapper(
    IUnaryMapper<LeftModel, RightModel> leftToRightMapper,
    IUnaryMapper<RightModel, LeftModel> rightToLeftMapper) : IBinaryMapper<LeftModel, RightModel>
{
    public LeftModel Map(RightModel source)
    {
        return rightToLeftMapper.Map(source);
    }

    public RightModel Map(LeftModel source)
    {
        return leftToRightMapper.Map(source);
    }
}