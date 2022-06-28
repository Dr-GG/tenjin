using System;

namespace Tenjin.Extensions;

public static class MathExtensions
{
    public const float DefaultSingleTolerance = 0.0000001f;
    public const double DefaultDoubleTolerance = 0.000000001;

    public static bool ToleranceEquals(this float left, float right, float tolerance = DefaultSingleTolerance)
    {
        return Math.Abs(left - right) < tolerance;
    }

    public static bool NoToleranceEquals(this float left, float right, float tolerance = DefaultSingleTolerance)
    {
        return !left.ToleranceEquals(right, tolerance);
    }

    public static bool ToleranceEquals(this double left, double right, double tolerance = DefaultDoubleTolerance)
    {
        return Math.Abs(left - right) < tolerance;
    }

    public static bool NoToleranceEquals(this double left, double right, double tolerance = DefaultDoubleTolerance)
    {
        return !left.ToleranceEquals(right, tolerance);
    }
}