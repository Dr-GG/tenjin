using Tenjin.Interfaces.Mappers;
using Tenjin.Tests.Models.Mappers;

namespace Tenjin.Tests.Mappers;

public class RightToLeftMapper : IUnaryMapper<RightModel, LeftModel>
{
    public LeftModel Map(RightModel source)
    {
        return new LeftModel
        {
            Property1 = source.Property1,
            Property2 = source.Property2
        };
    }
}