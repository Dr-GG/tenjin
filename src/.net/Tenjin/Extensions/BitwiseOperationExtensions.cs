namespace Tenjin.Extensions;

/// <summary>
/// Extension methods that supports bitwise operations.
/// </summary>
public static class BitwiseOperationExtensions
{
    /// <summary>
    /// Performs a bitwise operation on an enum and a mask.
    /// </summary>
    public static bool IsBitwiseEqual<TEnum>(this TEnum rootEnum, int mask) where TEnum : struct
    {
        return ((int)(object)rootEnum & mask) == mask;
    }
}