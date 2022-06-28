namespace Tenjin.Extensions;

public static class BitwiseOperationExtensions
{
    public static bool IsBitwiseEqual<TEnum>(this TEnum rootEnum, int mask) where TEnum : struct
    {
        return ((int)(object)rootEnum & mask) == mask;
    }
}