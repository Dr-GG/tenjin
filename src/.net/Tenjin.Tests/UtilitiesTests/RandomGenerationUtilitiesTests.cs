using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tenjin.Exceptions.Random;
using Tenjin.Models.Random;
using Tenjin.Utilities;

namespace Tenjin.Tests.UtilitiesTests
{
    [TestFixture, Parallelizable(ParallelScope.Children)]
    public class RandomGenerationUtilitiesTests
    {
        private const int Iterations = 5000;

        private static readonly RandomGenerationParameters DefaultRandomGenerationParameters = new()
        {
            AllowedRandomCharacters = "abcd012345",
            MaximumLength = 20,
            MinimumLength = 10,
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
            var parameters = DefaultRandomGenerationParameters with
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

            Assert.LessOrEqual(randomList.Distinct().Count(), Iterations);
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
            var parameters = DefaultRandomGenerationParameters with
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

            Assert.AreEqual(1, randomList.Distinct().Count());
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
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumLength = minimumLength,
                MaximumLength = maximumLength
            };

            for (var i = 0; i < Iterations; i++)
            {
                randomList.Add(
                    AssertRandomString(parameters, (int)minimumLength, (int)maximumLength));
            }

            Assert.LessOrEqual(randomList.Distinct().Count(), Iterations);
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
            var parameters = DefaultRandomGenerationParameters with
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

            Assert.AreEqual(1, randomList.Distinct().Count());
        }

        [TestCase(null)]
        [TestCase("")]
        public void GenerateRandomString_WhenAllowedRandomCharactersIsNullOrEmpty_ThrowsAnError(string? characters)
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                AllowedRandomCharacters = characters
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters));
        }

        [Test]
        public void GenerateRandomString_WhenNoLengthIsSpecified_ThrowsAnError()
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumLength = null,
                MaximumLength = null,
                Length = null
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters));
        }

        [TestCase(1u, null)]
        [TestCase(1u, 1u)]
        [TestCase(null, 1u)]
        public void GenerateRandomString_WhenLengthIsSpecifiedWithMinimumAndOrMaximumLength_ThrowsAnError(
            uint? minimumLength,
            uint? maximumLength)
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumLength = minimumLength,
                MaximumLength = maximumLength,
                Length = 10
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters));
        }

        [TestCase(1u, null)]
        [TestCase(null, 1u)]
        public void GenerateRandomString_WhenLengthIsNullWithMinimumOrMaximumLength_ThrowsAnError(
            uint? minimumLength,
            uint? maximumLength)
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumLength = minimumLength,
                MaximumLength = maximumLength,
                Length = null
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters));
        }

        [Test]
        public void GenerateRandomString_WhenMinimumLengthIsLargerThanMaximumLength_ThrowsAnError()
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumLength = 10,
                MaximumLength = 5,
                Length = null
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GenerateRandomString(parameters));
        }

        [TestCase(1, null)]
        [TestCase(null, 1)]
        public void GenerateRandomDouble_WhenMinimumOrMaximumIsNullAndTheOtherNot_ThrowsAnError(double? minimum, double? maximum)
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumDouble = minimum,
                MaximumDouble = maximum
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomDouble(parameters));
        }

        [Test]
        public void GenerateRandomDouble_WhenMinimumIsLargerThanMaximum_ThrowsAnError()
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumDouble = 10,
                MaximumDouble = 5
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomDouble(parameters));
        }

        [Test]
        public void GenerateRandomDouble_WhenMinimumEqualsThanMaximum_ThrowsAnError()
        {
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumDouble = 10,
                MaximumDouble = 10
            };

            Assert.Throws<RandomGenerationException>(() => RandomGenerationUtilities.GetRandomDouble(parameters));
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
            var parameters = DefaultRandomGenerationParameters with
            {
                MinimumDouble = minimum,
                MaximumDouble = maximum
            };

            for (var i = 0; i < Iterations; i++)
            {
                randomList.Add(AssertRandomDouble(parameters, minimum, maximum));
            }

            Assert.LessOrEqual(randomList.Distinct().Count(), Iterations);
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
            var parameters = DefaultRandomGenerationParameters with
            {
                Seed = seed,
                MinimumDouble = minimum,
                MaximumDouble = maximum
            };

            for (var i = 0; i < Iterations; i++)
            {
                randomList.Add(AssertRandomDouble(parameters, minimum, maximum));
            }

            Assert.AreEqual(1, randomList.Distinct().Count());
        }

        [Test]
        public void GenerateRandomDouble_SpecifyingNoMinimumOrMaximum_GeneratesBetween0And1()
        {
            var randomList = new List<double>();

            for (var i = 0; i < Iterations; i++)
            {
                randomList.Add(AssertRandomDouble(DefaultRandomGenerationParameters, 0, 1));
            }

            Assert.LessOrEqual(randomList.Distinct().Count(), Iterations);
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
            var parameters = DefaultRandomGenerationParameters with
            {
                Seed = seed
            };

            for (var i = 0; i < Iterations; i++)
            {
                randomList.Add(AssertRandomDouble(parameters, 0, 1));
            }

            Assert.AreEqual(1, randomList.Distinct().Count());
        }

        private static double AssertRandomDouble(
            RandomGenerationParameters parameters,
            double minimum,
            double maximum)
        {
            var random = RandomGenerationUtilities.GetRandomDouble(parameters);

            Assert.LessOrEqual(random, maximum);
            Assert.GreaterOrEqual(random, minimum);

            return random;
        }

        private static string AssertRandomString(
            RandomGenerationParameters parameters,
            int expectedMinimumLength,
            int expectedMaximumLength)
        {
            var random = RandomGenerationUtilities.GenerateRandomString(parameters);

            Assert.LessOrEqual(random.Length, expectedMaximumLength);
            Assert.GreaterOrEqual(random.Length, expectedMinimumLength);

            Assert.IsTrue(random.ToCharArray().All(c => DefaultRandomGenerationParameters
                .AllowedRandomCharacters.ToCharArray().Contains(c)));

            return random;
        }
    }
}
