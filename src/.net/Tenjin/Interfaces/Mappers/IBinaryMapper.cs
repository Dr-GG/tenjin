namespace Tenjin.Interfaces.Mappers
{
    public interface IBinaryMapper<TLeft, TRight>
    {
        TLeft Map(TRight value);
        TRight Map(TLeft value);
    }
}
