using System;
using System.Text;
using Tenjin.Exceptions.Random;
using Tenjin.Extensions;
using Tenjin.Models.Random;

namespace Tenjin.Utilities;

public static class RandomGenerationUtilities
{
    public const double DefaultMinimumDouble = 0.0;
    public const double DefaultMaximumDouble = 1.0;

    public static double GetRandomDouble(RandomGenerationParameters parameters)
    {
        AssertRandomDoubleGenerationParameters(parameters);

        var random = GetRandom(parameters);
        var minimum = parameters.MinimumDouble ?? DefaultMinimumDouble;
        var maximum = parameters.MaximumDouble ?? DefaultMaximumDouble;

        return random.NextDouble() * (maximum - minimum) + minimum;
    }

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

        if (parameters.MinimumLength.HasValue
            && parameters.MaximumLength.HasValue)
        {
            return (uint)random.Next(
                (int)parameters.MinimumLength.Value,
                (int)parameters.MaximumLength.Value + 1);
        }

        throw new NotSupportedException(
            "Fatal error! Scenario not supported in generating random length");
    }

    private static Random GetRandom(RandomGenerationParameters parameters)
    {
        var seed = parameters.Seed ?? (int)(parameters.GetHashCode() ^ DateTime.Now.Ticks);

        return new Random(seed);
    }

    private static void AssertRandomDoubleGenerationParameters(RandomGenerationParameters parameters)
    {
        if (parameters.MinimumDouble == null
            && parameters.MaximumDouble != null
            ||
            parameters.MinimumDouble != null
            && parameters.MaximumDouble == null)
        {
            throw new RandomGenerationException(
                "When specifying minimum or maximum double, then both minimum and maximum double must be set");
        }

        if (parameters.MinimumDouble.HasValue
            && parameters.MaximumDouble.HasValue
            && parameters.MinimumDouble > parameters.MaximumDouble)
        {
            throw new RandomGenerationException("Minimum double cannot be greater than maximum double");
        }

        if (parameters.MinimumDouble.HasValue
            && parameters.MaximumDouble.HasValue
            && parameters.MinimumDouble.Value.ToleranceEquals(parameters.MaximumDouble.Value))
        {
            throw new RandomGenerationException("Minimum and maximum double cannot be of the same value");
        }
    }

    private static void AssertRandomStringGenerationParameters(RandomGenerationParameters parameters)
    {
        if (parameters.AllowedRandomCharacters.IsNullOrEmpty())
        {
            throw new RandomGenerationException("AllowedRandomCharacters not allowed to be null or empty");
        }

        if (parameters.MinimumLength == null
            && parameters.MaximumLength == null
            && parameters.Length == null)
        {
            throw new RandomGenerationException("Minimum/maximum or fixed lengths must be specified");
        }

        if (parameters.Length == null
            && (parameters.MinimumLength == null
                || parameters.MaximumLength == null))
        {
            throw new RandomGenerationException("Minimum and maximum length should both be set");
        }

        if (parameters.Length != null
            && (parameters.MinimumLength != null
                || parameters.MaximumLength != null))
        {
            throw new RandomGenerationException("Minimum and/or maximum and/or fixed lengths were specified");
        }

        if (parameters.MinimumLength.HasValue 
            && parameters.MinimumLength > parameters.MaximumLength)
        {
            throw new RandomGenerationException("Minimum length cannot be greater than maximum length");
        }
    }
}