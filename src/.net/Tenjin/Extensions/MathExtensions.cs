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
    public const float SingleTolerance = 0.0000001f;

    /// <summary>
    /// The double tolerance level or constant to be used between two float instances.
    /// </summary>
    public const double DoubleTolerance = 0.000000001;

    /// <summary>
    /// Determines if two float instances are equal to one another using a tolerance level.
    /// </summary>
    public static bool ToleranceEquals(this float left, float right, float tolerance = SingleTolerance)
    {
        return Math.Abs(left - right) < tolerance;
    }

    /// <summary>
    /// Determines if two double instances are equal to one another using a tolerance level.
    /// </summary>
    public static bool ToleranceEquals(this double left, double right, double tolerance = DoubleTolerance)
    {
        return Math.Abs(left - right) < tolerance;
    }

    /// <summary>
    /// Determines if two double instances are not equal to one another using a tolerance level.
    /// </summary>
    public static bool NoToleranceEquals(this double left, double right, double tolerance = DoubleTolerance)
    {
        return !left.ToleranceEquals(right, tolerance);
    }

    /// <summary>
    /// Determines if two float instances are not equal to one another using a tolerance level.
    /// </summary>
    public static bool NoToleranceEquals(this float left, float right, float tolerance = SingleTolerance)
    {
        return !left.ToleranceEquals(right, tolerance);
    }
}