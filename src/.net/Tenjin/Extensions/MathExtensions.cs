using System;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of math based extensions.
/// </summary>
public static class MathExtensions
{
    /// <summary>
    /// The float tolerance level or constant to be used between two float instances.
    /// </summary>
    public const float DefaultSingleTolerance = 0.0000001f;

    /// <summary>
    /// The double tolerance level or constant to be used between two float instances.
    /// </summary>
    public const double DefaultDoubleTolerance = 0.000000001;

    /// <summary>
    /// Determines if two float intances are equal to one another using a tolerance level.
    /// </summary>
    public static bool ToleranceEquals(this float left, float right, float tolerance = DefaultSingleTolerance)
    {
        return Math.Abs(left - right) < tolerance;
    }

    /// <summary>
    /// Determines if two double intances are equal to one another using a tolerance level.
    /// </summary>
    public static bool ToleranceEquals(this double left, double right, double tolerance = DefaultDoubleTolerance)
    {
        return Math.Abs(left - right) < tolerance;
    }

    /// <summary>
    /// Determines if two double intances are not equal to one another using a tolerance level.
    /// </summary>
    public static bool NoToleranceEquals(this double left, double right, double tolerance = DefaultDoubleTolerance)
    {
        return !left.ToleranceEquals(right, tolerance);
    }

    /// <summary>
    /// Determines if two float intances are not equal to one another using a tolerance level.
    /// </summary>
    public static bool NoToleranceEquals(this float left, float right, float tolerance = DefaultSingleTolerance)
    {
        return !left.ToleranceEquals(right, tolerance);
    }
}