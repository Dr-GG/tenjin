using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Exceptions.Random;
using Tenjin.Models.Random;
using Tenjin.Utilities;

namespace Tenjin.Tests.UtilitiesTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class RandomGenerationUtilitiesTests
{
    private const int DefaultSeed = 10;
    private const int Iterations = 5000;

    private static readonly RandomGenerationParameters TestRandomGenerationParameters = new()
    {
        // ReSharper disable once StringLiteralTypo
        AllowedRandomCharacters = "abcd012345",
        MaximumLength = 20,
        MinimumLength = 10
    };

    [TestCase(1u)]
    [TestCase(2u)]
    [TestCase(3u)]
    [TestCase(4u)]
    [TestCase(5u)]
    [TestCase(6u)]
    [TestCase(7u)]
    [TestCase(8u)]
    [TestCase(9u)]
    [TestCase(10u)]
    [TestCase(11u)]
    [TestCase(12u)]
    [TestCase(13u)]
    [TestCase(14u)]
    [TestCase(15u)]
    [TestCase(16u)]
    [TestCase(17u)]
    [TestCase(18u)]
    [TestCase(19u)]
    [TestCase(20u)]
    public void GenerateRandomString_WithGivenAFixedLength_GeneratesRandomStringsWithFixedLength(uint length)
    {
        var randomList = new List<string>();
        var parameters = TestRandomGenerationParameters with
        {
            MaximumLength = null,
            MinimumLength = null,
            Length = length
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(
                AssertRandomString(parameters, (int)length, (int)length));
        }

        randomList.Distinct().Should().HaveCountLessThanOrEqualTo(Iterations);
    }

    [TestCase(1u, 10)]
    [TestCase(2u, 20)]
    [TestCase(3u, 1000)]
    [TestCase(4u, 20000)]
    [TestCase(5u, 123)]
    [TestCase(6u, 321)]
    [TestCase(7u, 987)]
    [TestCase(8u, 789)]
    [TestCase(9u, 1)]
    [TestCase(10u, 2)]
    [TestCase(11u, 3)]
    [TestCase(12u, 5)]
    [TestCase(13u, 8)]
    [TestCase(14u, 111)]
    [TestCase(15u, 222)]
    [TestCase(16u, 333)]
    [TestCase(17u, 444)]
    [TestCase(18u, 555)]
    [TestCase(19u, -1)]
    [TestCase(20u, -1000)]
    public void GenerateRandomString_WithGivenAFixedLengthAndTheSameSeed_GeneratesTheExactSameValue(uint length, int seed)
    {
        var randomList = new List<string>();
        var parameters = TestRandomGenerationParameters with
        {
            Seed = seed,
            MaximumLength = null,
            MinimumLength = null,
            Length = length
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(
                AssertRandomString(parameters, (int)length, (int)length));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase(1u)]
    [TestCase(2u)]
    [TestCase(3u)]
    [TestCase(4u)]
    [TestCase(5u)]
    [TestCase(6u)]
    [TestCase(7u)]
    [TestCase(8u)]
    [TestCase(9u)]
    [TestCase(10u)]
    public void GenerateRandomString_WithGivenAFixedLengthAndTheSameRandomObject_GeneratesTheExactSameValue(uint length)
    {
        var randomList = new List<string>();

        for (var i = 0; i < Iterations; i++)
        {
            var parameters = TestRandomGenerationParameters with
            {
                Random = new System.Random(DefaultSeed),
                MaximumLength = null,
                MinimumLength = null,
                Length = length
            };

            randomList.Add(
                AssertRandomString(parameters, (int)length, (int)length));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase(1u, 2u)]
    [TestCase(2u, 4u)]
    [TestCase(3u, 6u)]
    [TestCase(4u, 8u)]
    [TestCase(5u, 10u)]
    [TestCase(6u, 12u)]
    [TestCase(7u, 14u)]
    [TestCase(8u, 16u)]
    [TestCase(9u, 18u)]
    [TestCase(10u, 20u)]
    [TestCase(11u, 22u)]
    [TestCase(12u, 24u)]
    [TestCase(13u, 26u)]
    [TestCase(14u, 28u)]
    [TestCase(15u, 30u)]
    [TestCase(16u, 32u)]
    [TestCase(17u, 34u)]
    [TestCase(18u, 36u)]
    [TestCase(19u, 38u)]
    [TestCase(20u, 40u)]
    public void GenerateRandomString_WithMinimumAndMaximumLengths_GeneratesRandomStringsWithVariablesLength(
        uint minimumLength,
        uint maximumLength)
    {
        var randomList = new List<string>();
        var parameters = TestRandomGenerationParameters with
        {
            MinimumLength = minimumLength,
            MaximumLength = maximumLength
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(
                AssertRandomString(parameters, (int)minimumLength, (int)maximumLength));
        }

        randomList.Distinct().Should().HaveCountLessThanOrEqualTo(Iterations);
    }

    [TestCase(1u, 2u, 1)]
    [TestCase(2u, 4u, 2)]
    [TestCase(3u, 6u, 3)]
    [TestCase(4u, 8u, 123)]
    [TestCase(5u, 10u, 321)]
    [TestCase(6u, 12u, 987)]
    [TestCase(7u, 14u, 789)]
    [TestCase(8u, 16u, 10)]
    [TestCase(9u, 18u, 200)]
    [TestCase(10u, 20u, 10000)]
    [TestCase(11u, 22u, 200000)]
    [TestCase(12u, 24u, -1)]
    [TestCase(13u, 26u, -10000)]
    [TestCase(14u, 28u, 111)]
    [TestCase(15u, 30u, 222)]
    [TestCase(16u, 32u, 333)]
    [TestCase(17u, 34u, 444)]
    [TestCase(18u, 36u, 555)]
    [TestCase(19u, 38u, 456)]
    [TestCase(20u, 40u, 654)]
    public void GenerateRandomString_WithMinimumAndMaximumLengthsAndTheSameSeed_GeneratesRandomStringsWithVariablesLength(
        uint minimumLength,
        uint maximumLength,
        int seed)
    {
        var randomList = new List<string>();
        var parameters = TestRandomGenerationParameters with
        {
            Seed = seed,
            MinimumLength = minimumLength,
            MaximumLength = maximumLength,
            Length = null
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(
                AssertRandomString(parameters, (int)minimumLength, (int)maximumLength));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase(1u, 2u)]
    [TestCase(2u, 4u)]
    [TestCase(3u, 6u)]
    [TestCase(4u, 8u)]
    [TestCase(5u, 10u)]
    [TestCase(6u, 12u)]
    [TestCase(7u, 14u)]
    [TestCase(8u, 16u)]
    [TestCase(9u, 18u)]
    [TestCase(10u, 20u)]
    [TestCase(11u, 22u)]
    [TestCase(12u, 24u)]
    [TestCase(13u, 26u)]
    [TestCase(14u, 28u)]
    [TestCase(15u, 30u)]
    [TestCase(16u, 32u)]
    [TestCase(17u, 34u)]
    [TestCase(18u, 36u)]
    [TestCase(19u, 38u)]
    [TestCase(20u, 40u)]
    public void GenerateRandomString_WithMinimumAndMaximumLengthsAndTheSameRandomObject_GeneratesRandomStringsWithVariablesLength(
        uint minimumLength,
        uint maximumLength)
    {
        var randomList = new List<string>();

        for (var i = 0; i < Iterations; i++)
        {
            var parameters = TestRandomGenerationParameters with
            {
                Random = new System.Random(DefaultSeed),
                MinimumLength = minimumLength,
                MaximumLength = maximumLength,
                Length = null
            };

            randomList.Add(
                AssertRandomString(parameters, (int)minimumLength, (int)maximumLength));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase("")]
    public void GenerateRandomString_WhenAllowedRandomCharactersIsNullOrEmpty_ThrowsAnError(string characters)
    {
        var parameters = TestRandomGenerationParameters with
        {
            AllowedRandomCharacters = characters
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("AllowedRandomCharacters not allowed to be null or empty.");
    }

    [Test]
    public void GenerateRandomString_WhenNoLengthIsSpecified_ThrowsAnError()
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumLength = null,
            MaximumLength = null,
            Length = null
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum/maximum or fixed lengths must be specified.");
    }

    [TestCase(1u, null)]
    [TestCase(1u, 1u)]
    [TestCase(null, 1u)]
    public void GenerateRandomString_WhenLengthIsSpecifiedWithMinimumAndOrMaximumLength_ThrowsAnError(
        uint? minimumLength,
        uint? maximumLength)
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumLength = minimumLength,
            MaximumLength = maximumLength,
            Length = 10
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum and/or maximum and/or fixed lengths were specified.");
    }

    [TestCase(1u, null)]
    [TestCase(null, 1u)]
    public void GenerateRandomString_WhenLengthIsNullWithMinimumOrMaximumLength_ThrowsAnError(
        uint? minimumLength,
        uint? maximumLength)
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumLength = minimumLength,
            MaximumLength = maximumLength,
            Length = null
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum and maximum length should both be set.");
    }

    [Test]
    public void GenerateRandomString_WhenMinimumLengthIsLargerThanMaximumLength_ThrowsAnError()
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumLength = 10,
            MaximumLength = 5,
            Length = null
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum length cannot be greater than maximum length.");
    }

    [TestCase(1, null)]
    [TestCase(null, 1)]
    public void GenerateRandomDouble_WhenMinimumOrMaximumIsNullAndTheOtherNot_ThrowsAnError(double? minimum, double? maximum)
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumDouble = minimum,
            MaximumDouble = maximum
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomDouble(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("When specifying minimum or maximum double, then both minimum and maximum double must be set.");
    }

    [Test]
    public void GenerateRandomDouble_WhenMinimumIsLargerThanMaximum_ThrowsAnError()
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumDouble = 10,
            MaximumDouble = 5
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomDouble(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum double cannot be greater than maximum double.");
    }

    [Test]
    public void GenerateRandomDouble_WhenMinimumEqualsThanMaximum_ThrowsAnError()
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumDouble = 10,
            MaximumDouble = 10
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomDouble(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum and maximum double cannot be of the same value.");
    }

    [TestCase(0, 1)]
    [TestCase(0.5, 1)]
    [TestCase(0.75, 1)]
    [TestCase(0.90, 1)]
    [TestCase(0.999, 1)]
    [TestCase(0.9999, 1)]
    [TestCase(1, 10)]
    [TestCase(1.5, 10)]
    [TestCase(1.75, 10)]
    [TestCase(1.90, 10)]
    [TestCase(1.999, 10)]
    [TestCase(1.9999, 10)]
    [TestCase(10, 100)]
    [TestCase(10.5, 100)]
    [TestCase(10.75, 100)]
    [TestCase(10.90, 100)]
    [TestCase(10.999, 100)]
    [TestCase(10.9999, 100)]
    public void GenerateRandomDouble_WithMinimumAndMaximum_GeneratesValidDouble(double minimum, double maximum)
    {
        var randomList = new List<double>();
        var parameters = TestRandomGenerationParameters with
        {
            MinimumDouble = minimum,
            MaximumDouble = maximum
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomDouble(parameters, minimum, maximum));
        }

        randomList.Distinct().Should().HaveCountLessThanOrEqualTo(Iterations);
    }

    [TestCase(0, 1, 10)]
    [TestCase(0.5, 1, 200)]
    [TestCase(0.75, 1, 3000)]
    [TestCase(0.90, 1, 123)]
    [TestCase(0.999, 1, 321)]
    [TestCase(0.9999, 1, 987)]
    [TestCase(1, 10, 789)]
    [TestCase(1.5, 10, 111)]
    [TestCase(1.75, 10, 222)]
    [TestCase(1.90, 10, 333)]
    [TestCase(1.999, 10, 444)]
    [TestCase(1.9999, 10, 555)]
    [TestCase(10, 100, -1)]
    [TestCase(10.5, 100, -2)]
    [TestCase(10.75, 100, -3)]
    [TestCase(10.90, 100, 7766)]
    [TestCase(10.999, 100, 9900)]
    [TestCase(10.9999, 100, 8833)]
    public void GenerateRandomDouble_WithMinimumAndMaximumAndAFixedSeed_GeneratesTheSameValues(
        double minimum,
        double maximum,
        int seed)
    {
        var randomList = new List<double>();
        var parameters = TestRandomGenerationParameters with
        {
            Seed = seed,
            MinimumDouble = minimum,
            MaximumDouble = maximum
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomDouble(parameters, minimum, maximum));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase(0, 1)]
    [TestCase(0.5, 1)]
    [TestCase(0.75, 1)]
    [TestCase(0.90, 1)]
    [TestCase(0.999, 1)]
    [TestCase(0.9999, 1)]
    [TestCase(1, 10)]
    [TestCase(1.5, 10)]
    [TestCase(1.75, 10)]
    [TestCase(1.90, 10)]
    [TestCase(1.999, 10)]
    [TestCase(1.9999, 10)]
    [TestCase(10, 100)]
    [TestCase(10.5, 100)]
    [TestCase(10.75, 100)]
    [TestCase(10.90, 100)]
    [TestCase(10.999, 100)]
    [TestCase(10.9999, 100)]
    public void GenerateRandomDouble_WithMinimumAndMaximumAndTheSameRandomObject_GeneratesTheSameValues(
        double minimum,
        double maximum)
    {
        var randomList = new List<double>();

        for (var i = 0; i < Iterations; i++)
        {
            var parameters = TestRandomGenerationParameters with
            {
                Random = new System.Random(DefaultSeed),
                MinimumDouble = minimum,
                MaximumDouble = maximum
            };

            randomList.Add(AssertRandomDouble(parameters, minimum, maximum));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [Test]
    public void GenerateRandomDouble_SpecifyingNoMinimumOrMaximum_GeneratesBetween0And1()
    {
        var randomList = new List<double>();

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomDouble(TestRandomGenerationParameters, 0, 1));
        }

        randomList.Distinct().Should().HaveCountLessThanOrEqualTo(Iterations);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(111)]
    [TestCase(222)]
    [TestCase(333)]
    [TestCase(444)]
    [TestCase(555)]
    [TestCase(123)]
    [TestCase(321)]
    [TestCase(456)]
    [TestCase(654)]
    [TestCase(987)]
    [TestCase(789)]
    [TestCase(1199)]
    [TestCase(9911)]
    [TestCase(8822)]
    [TestCase(2288)]
    [TestCase(3366)]
    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-3)]
    [TestCase(-4)]
    [TestCase(-5)]
    [TestCase(-111)]
    [TestCase(-222)]
    [TestCase(-333)]
    [TestCase(-444)]
    [TestCase(-555)]
    [TestCase(-123)]
    [TestCase(-321)]
    [TestCase(-456)]
    [TestCase(-654)]
    [TestCase(-987)]
    [TestCase(-789)]
    [TestCase(-1199)]
    [TestCase(-9911)]
    [TestCase(-8822)]
    [TestCase(-2288)]
    [TestCase(-3366)]
    public void GenerateRandomDouble_SpecifyingNoMinimumOrMaximumWithAFixedSeed_GeneratesBetween0And1WithTheSameValues(int seed)
    {
        var randomList = new List<double>();
        var parameters = TestRandomGenerationParameters with
        {
            Seed = seed
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomDouble(parameters, 0, 1));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [Test]
    public void GenerateRandomDouble_SpecifyingNoMinimumOrMaximumWithTheSameRandomObject_GeneratesBetween0And1WithTheSameValues()
    {
        var randomList = new List<double>();

        for (var i = 0; i < Iterations; i++)
        {
            var parameters = TestRandomGenerationParameters with
            {
                Random = new System.Random(DefaultSeed)
            };

            randomList.Add(AssertRandomDouble(parameters, 0, 1));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase(1, null)]
    [TestCase(null, 1)]
    public void GenerateRandomInt32_WhenMinimumOrMaximumIsNullAndTheOtherNot_ThrowsAnError(int? minimum, int? maximum)
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumInt32 = minimum,
            MaximumInt32 = maximum
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomInt32(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("When specifying minimum or maximum Int32, then both minimum and maximum Int32 must be set.");
    }

    [Test]
    public void GenerateRandomInt32_WhenMinimumIsLargerThanMaximum_ThrowsAnError()
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumInt32 = 10,
            MaximumInt32 = 5
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomInt32(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum Int32 cannot be greater than maximum Int32.");
    }

    [Test]
    public void GenerateRandomInt32_WhenMinimumEqualsThanMaximum_ThrowsAnError()
    {
        var parameters = TestRandomGenerationParameters with
        {
            MinimumInt32 = 10,
            MaximumInt32 = 10
        };

        var error = Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomInt32(parameters))!;

        error.Should().NotBeNull();
        error.Message.Should().Be("Minimum and maximum Int32 cannot be of the same value.");
    }

    [TestCase(0, 1)]
    [TestCase(1, 10)]
    [TestCase(2, 10)]
    [TestCase(5, 10)]
    [TestCase(8, 10)]
    [TestCase(10, 100)]
    [TestCase(25, 100)]
    [TestCase(50, 100)]
    [TestCase(75, 100)]
    [TestCase(100, 1000)]
    [TestCase(250, 1000)]
    [TestCase(500, 1000)]
    [TestCase(750, 1000)]
    public void GenerateRandomInt32_WithMinimumAndMaximum_GeneratesValidInt32(int minimum, int maximum)
    {
        var randomList = new List<double>();
        var parameters = TestRandomGenerationParameters with
        {
            MinimumInt32 = minimum,
            MaximumInt32 = maximum
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomInt32(parameters, minimum, maximum));
        }

        randomList.Distinct().Should().HaveCountLessThanOrEqualTo(Iterations);
    }

    [TestCase(0, 1, 10)]
    [TestCase(1, 10, 30)]
    [TestCase(2, 10, 45)]
    [TestCase(5, 10, 66)]
    [TestCase(8, 10, -10)]
    [TestCase(10, 100, -100)]
    [TestCase(25, 100, 775)]
    [TestCase(50, 100, 5521)]
    [TestCase(75, 100, 199)]
    [TestCase(100, 1000, 788)]
    [TestCase(250, 1000, 899)]
    [TestCase(500, 1000, -88)]
    [TestCase(750, 1000, 3)]
    public void GenerateRandomInt32_WithMinimumAndMaximumAndAFixedSeed_GeneratesTheSameValues(
        int minimum,
        int maximum,
        int seed)
    {
        var randomList = new List<double>();
        var parameters = TestRandomGenerationParameters with
        {
            Seed = seed,
            MinimumInt32 = minimum,
            MaximumInt32 = maximum
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomInt32(parameters, minimum, maximum));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [TestCase(0, 1)]
    [TestCase(1, 10)]
    [TestCase(2, 10)]
    [TestCase(5, 10)]
    [TestCase(8, 10)]
    [TestCase(10, 100)]
    [TestCase(25, 100)]
    [TestCase(50, 100)]
    [TestCase(75, 100)]
    [TestCase(100, 1000)]
    [TestCase(250, 1000)]
    [TestCase(500, 1000)]
    [TestCase(750, 1000)]
    public void GenerateRandomInt32_WithMinimumAndMaximumAndTheSameRandomObject_GeneratesTheSameValues(
        int minimum,
        int maximum)
    {
        var randomList = new List<double>();

        for (var i = 0; i < Iterations; i++)
        {
            var parameters = TestRandomGenerationParameters with
            {
                Random = new System.Random(DefaultSeed),
                MinimumInt32 = minimum,
                MaximumInt32 = maximum
            };

            randomList.Add(AssertRandomInt32(parameters, minimum, maximum));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [Test]
    public void GenerateRandomInt32_SpecifyingNoMinimumOrMaximum_GeneratesBetweenMinInt32AndMaxInt32()
    {
        var randomList = new List<double>();

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomInt32(TestRandomGenerationParameters, int.MinValue, int.MaxValue));
        }

        randomList.Distinct().Should().HaveCountLessThanOrEqualTo(Iterations);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(111)]
    [TestCase(222)]
    [TestCase(333)]
    [TestCase(444)]
    [TestCase(555)]
    [TestCase(123)]
    [TestCase(321)]
    [TestCase(456)]
    [TestCase(654)]
    [TestCase(987)]
    [TestCase(789)]
    [TestCase(1199)]
    [TestCase(9911)]
    [TestCase(8822)]
    [TestCase(2288)]
    [TestCase(3366)]
    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-3)]
    [TestCase(-4)]
    [TestCase(-5)]
    [TestCase(-111)]
    [TestCase(-222)]
    [TestCase(-333)]
    [TestCase(-444)]
    [TestCase(-555)]
    [TestCase(-123)]
    [TestCase(-321)]
    [TestCase(-456)]
    [TestCase(-654)]
    [TestCase(-987)]
    [TestCase(-789)]
    [TestCase(-1199)]
    [TestCase(-9911)]
    [TestCase(-8822)]
    [TestCase(-2288)]
    [TestCase(-3366)]
    public void GenerateRandomInt32_SpecifyingNoMinimumOrMaximumWithAFixedSeed_GeneratesBetweenMinInt32AndMaxInt32WithTheSameValues(int seed)
    {
        var randomList = new List<double>();
        var parameters = TestRandomGenerationParameters with
        {
            Seed = seed
        };

        for (var i = 0; i < Iterations; i++)
        {
            randomList.Add(AssertRandomInt32(parameters, int.MinValue, int.MaxValue));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    [Test]
    public void GenerateRandomInt32_SpecifyingNoMinimumOrMaximumWithTheSameRandomObject_GeneratesBetweenMinInt32AndMaxInt32WithTheSameValues()
    {
        var randomList = new List<double>();

        for (var i = 0; i < Iterations; i++)
        {
            var parameters = TestRandomGenerationParameters with
            {
                Random = new System.Random(DefaultSeed)
            };

            randomList.Add(AssertRandomInt32(parameters, int.MinValue, int.MaxValue));
        }

        randomList.Distinct().Should().HaveCount(1);
    }

    private static double AssertRandomInt32(
        RandomGenerationParameters parameters,
        int minimum,
        int maximum)
    {
        var random = RandomGenerationUtilities.GetRandomInt32(parameters);

        random.Should().BeLessThanOrEqualTo(maximum);
        random.Should().BeGreaterThanOrEqualTo(minimum);

        return random;
    }

    private static double AssertRandomDouble(
        RandomGenerationParameters parameters,
        double minimum,
        double maximum)
    {
        var random = RandomGenerationUtilities.GetRandomDouble(parameters);

        random.Should().BeLessThanOrEqualTo(maximum);
        random.Should().BeGreaterThanOrEqualTo(minimum);

        return random;
    }

    private static string AssertRandomString(
        RandomGenerationParameters parameters,
        int expectedMinimumLength,
        int expectedMaximumLength)
    {
        var random = RandomGenerationUtilities.GenerateRandomString(parameters);

        random.Length.Should().BeLessThanOrEqualTo(expectedMaximumLength);
        random.Length.Should().BeGreaterThanOrEqualTo(expectedMinimumLength);

        random
            .ToCharArray()
            .All(c => TestRandomGenerationParameters
                .AllowedRandomCharacters
                .ToCharArray()
                .Contains(c))
            .Should()
            .BeTrue();

        return random;
    }
}