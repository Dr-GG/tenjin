using System;
using System.Text;
using Tenjin.Exceptions.Random;
using Tenjin.Extensions;
using Tenjin.Models.Random;

namespace Tenjin.Utilities;

/// <summary>
/// A collection of methods used for random generation.
/// </summary>
public static class RandomGenerationUtilities
{
    public const int DefaultMinimumInt32 = int.MinValue;
    public const int DefaultMaximumInt32 = int.MaxValue - 1;

    public const double DefaultMinimumDouble = 0.0;
    public const double DefaultMaximumDouble = 1.0;

    /// <summary>
    /// Generates a random double.
    /// </summary>
    public static double GetRandomDouble(RandomGenerationParameters parameters)
    {
        AssertRandomDoubleGenerationParameters(parameters);

        var random = GetRandom(parameters);
        var minimum = parameters.MinimumDouble ?? DefaultMinimumDouble;
        var maximum = parameters.MaximumDouble ?? DefaultMaximumDouble;

        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    /// <summary>
    /// Generates a random Int32 value.
    /// </summary>
    public static int GetRandomInt32(RandomGenerationParameters parameters)
    {
        AssertRandomInt32GenerationParameters(parameters);

        var random = GetRandom(parameters);
        var minimum = parameters.MinimumInt32 ?? DefaultMinimumInt32;
        var maximum = parameters.MaximumInt32 ?? DefaultMaximumInt32;

        return random.Next(minimum, maximum + 1);
    }

    /// <summary>
    /// Generates a random string.
    /// </summary>
    public static string GenerateRandomString(RandomGenerationParameters parameters)
    {
        AssertRandomStringGenerationParameters(parameters);

        var random = GetRandom(parameters);
        var length = GetRandomLength(parameters, random);
        var output = new StringBuilder((int)length);

        for (var i = 0u; i < length; i++)
        {
            output.Append(GetRandomCharacter(parameters, random));
        }

        return output.ToString();
    }

    private static char GetRandomCharacter(RandomGenerationParameters parameters, Random random)
    {
        var index = random.Next(0, parameters.AllowedRandomCharacters.Length);

        return parameters.AllowedRandomCharacters[index];
    }

    private static uint GetRandomLength(RandomGenerationParameters parameters, Random random)
    {
        if (parameters.Length.HasValue)
        {
            return parameters.Length.Value;
        }

        if (parameters is {MinimumLength: not null, MaximumLength: not null})
        {
            return (uint)random.Next(
                (int)parameters.MinimumLength.Value,
                (int)parameters.MaximumLength.Value + 1);
        }

        throw new NotSupportedException(
            "Fatal error! Scenario not supported in generating random length.");
    }

    private static Random GetRandom(RandomGenerationParameters parameters)
    {
        if (parameters.Random != null)
        {
            return parameters.Random;
        }

        var seed = parameters.Seed ?? (int)(parameters.GetHashCode() ^ DateTime.Now.Ticks);

        return new Random(seed);
    }

    private static void AssertRandomDoubleGenerationParameters(RandomGenerationParameters parameters)
    {
        if (parameters.MinimumDouble == null
            && parameters.MaximumDouble != null
            ||
            parameters is {MinimumDouble: not null, MaximumDouble: null})
        {
            throw new RandomGenerationException(
                "When specifying minimum or maximum double, then both minimum and maximum double must be set.");
        }

        switch (parameters)
        {
            case {MinimumDouble: not null, MaximumDouble: not null} 
            when parameters.MinimumDouble > parameters.MaximumDouble:
                throw new RandomGenerationException("Minimum double cannot be greater than maximum double.");

            case {MinimumDouble: not null, MaximumDouble: not null} 
            when parameters.MinimumDouble.Value.ToleranceEquals(parameters.MaximumDouble.Value):
                throw new RandomGenerationException("Minimum and maximum double cannot be of the same value.");
        }
    }

    private static void AssertRandomInt32GenerationParameters(RandomGenerationParameters parameters)
    {
        if (parameters.MinimumInt32 == null
            && parameters.MaximumInt32 != null
            ||
            parameters is {MinimumInt32: not null, MaximumInt32: null})
        {
            throw new RandomGenerationException(
                "When specifying minimum or maximum Int32, then both minimum and maximum Int32 must be set.");
        }

        switch (parameters)
        {
            case {MinimumInt32: not null, MaximumInt32: not null} 
            when parameters.MinimumInt32 > parameters.MaximumInt32:
                throw new RandomGenerationException("Minimum Int32 cannot be greater than maximum Int32.");
            case {MinimumInt32: not null, MaximumInt32: not null} 
            when parameters.MinimumInt32.Value == parameters.MaximumInt32.Value:
                throw new RandomGenerationException("Minimum and maximum Int32 cannot be of the same value.");
        }
    }

    private static void AssertRandomStringGenerationParameters(RandomGenerationParameters parameters)
    {
        if (parameters.AllowedRandomCharacters.IsNullOrEmpty())
        {
            throw new RandomGenerationException("AllowedRandomCharacters not allowed to be null or empty.");
        }

        if (parameters.MinimumLength == null
            && parameters.MaximumLength == null
            && parameters.Length == null)
        {
            throw new RandomGenerationException("Minimum/maximum or fixed lengths must be specified.");
        }

        if (parameters.Length == null
            && (parameters.MinimumLength == null
                || parameters.MaximumLength == null))
        {
            throw new RandomGenerationException("Minimum and maximum length should both be set.");
        }

        if (parameters.Length != null
            && (parameters.MinimumLength != null
                || parameters.MaximumLength != null))
        {
            throw new RandomGenerationException("Minimum and/or maximum and/or fixed lengths were specified.");
        }

        if (parameters.MinimumLength.HasValue
            && parameters.MinimumLength > parameters.MaximumLength)
        {
            throw new RandomGenerationException("Minimum length cannot be greater than maximum length.");
        }
    }
}