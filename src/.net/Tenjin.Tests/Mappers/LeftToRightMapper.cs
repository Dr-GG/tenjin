using Tenjin.Interfaces.Mappers;
using Tenjin.Tests.Models;

namespace Tenjin.Tests.Mappers
{
    public class LeftToRightMapper : IUnaryMapper<LeftModel, RightModel>
    {
        public RightModel Map(LeftModel source)
        {
            return new()
            {
                Property1 = source.Property1,
                Property2 = source.Property2
            };
        }
    }
}
