namespace Tenjin.Interfaces.Mappers
{
    public interface IUnaryMapper<in TSource, out TDestination>
    {
        TDestination Map(TSource value);
    }
}
