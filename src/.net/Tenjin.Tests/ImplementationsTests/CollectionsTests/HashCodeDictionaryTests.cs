using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Implementations.Collections;
using Tenjin.Interfaces.Collections;
using Tenjin.Tests.Models.HashCodeDictionary;

namespace Tenjin.Tests.ImplementationsTests.CollectionsTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public class HashCodeDictionaryTests
{
    public const int NumberOfModels = 1000;
    public const int NumberOfNonExistingModels = 2001;
    public const int NumberModelsToRemove = 776;
    public const int NonExistingModelsOffset = NumberOfModels + 1;
    public const int ModelsToRemoveOffset = 225;

    [Test]
    public void GetEnumerator_WhenPopulatingTheDictionary_EnumeratesAsExpected()
    {
        var dictionary = GetDefaultDictionary();
        using var enumerator = dictionary.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var (key, value) = enumerator.Current;

            key.Should().Be(value.GetHashCode());
        }
    }

    [Test]
    public void ArrayIndexWithKey_WhenPopulatingTheDictionaryAndUsingExistingKeys_RetrievesTheObjectCorrectly()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var compareModel = GetHashCodeModel(i);
            var dictionaryModel = dictionary[compareModel.GetHashCode()];

            compareModel.Should().Be(dictionaryModel);
            compareModel.GetHashCode().Should().Be(dictionaryModel.GetHashCode());
        }
    }

    [Test]
    public void ArrayIndexWithKey_WhenPopulatingTheDictionaryAndUsingNonExistingKeys_ThrowsKeyNotFoundException()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var compareModel = GetHashCodeModel(i);
            var error = Assert.Throws<KeyNotFoundException>(() =>
            {
                var _ = dictionary[compareModel.GetHashCode()];
            })!;

            error.Should().NotBeNull();
            error.Message.Should().Be($"The given key '{compareModel.GetHashCode()}' was not present in the dictionary.");
        }
    }

    [Test]
    public void ArrayIndexWithModel_WhenPopulatingTheDictionary_RetrievesTheObjectCorrectly()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var compareModel = GetHashCodeModel(i);
            var dictionaryModel = dictionary[compareModel];

            compareModel.Should().Be(dictionaryModel);
            compareModel.GetHashCode().Should().Be(dictionaryModel.GetHashCode());
        }
    }

    [Test]
    public void ArrayIndexWithModel_WhenPopulatingTheDictionaryAndUsingNonExistingModels_ThrowsKeyNotFoundException()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var compareModel = GetHashCodeModel(i);
            var error = Assert.Throws<KeyNotFoundException>(() =>
            {
                var _ = dictionary[compareModel.GetHashCode()];
            })!;

            error.Should().NotBeNull();
            error.Message.Should().Be($"The given key '{compareModel.GetHashCode()}' was not present in the dictionary.");
        }
    }

    [Test]
    public void Keys_WhenPopulatingTheDictionary_RetrievesTheKeysCorrectly()
    {
        var dictionary = GetDefaultDictionary();
        var keys = dictionary.Keys.ToList();

        Assert.AreEqual(NumberOfModels, keys.Count);

        for (var i = 0; i < keys.Count; i++)
        {
            var model = GetHashCodeModel(i);
            var key = keys[i];

            key.Should().Be(model.GetHashCode());
        }
    }

    [Test]
    public void Values_WhenPopulatingTheDictionary_RetrievesTheValuesCorrectly()
    {
        var dictionary = GetDefaultDictionary();
        var values = dictionary.Values.ToList();

        Assert.AreEqual(NumberOfModels, values.Count);

        for (var i = 0; i < values.Count; i++)
        {
            var model = GetHashCodeModel(i);
            var value = values[i];

            value.GetHashCode().Should().Be(model.GetHashCode());
            value.Should().Be(model);
        }
    }

    [Test]
    public void Clear_WhenPopulatingTheDictionary_ClearsTheEntireDictionary()
    {
        var dictionary = GetDefaultDictionary();

        dictionary.Should().HaveCount(NumberOfModels);

        dictionary.Clear();

        dictionary.Should().BeEmpty();
        dictionary.Keys.Should().BeEmpty();
        dictionary.Values.Should().BeEmpty();

        using var enumerator = dictionary.GetEnumerator();
        var enumeratorCounter = 0;

        while (enumerator.MoveNext())
        {
            enumeratorCounter++;
        }

        Assert.AreEqual(0, enumeratorCounter);
    }

    [TestCase]
    public void ContainsKeyValuePair_WhenPopulatingTheDictionaryAndUsingExistingPairs_ReturnsTrue()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);
            var pair = new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model);

            dictionary.Contains(pair).Should().BeTrue();
        }
    }

    [TestCase]
    public void ContainsKeyValuePair_WhenPopulatingTheDictionaryAndUsingNonExistingPairs_ReturnsFalse()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);
            var pair = new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model);

            dictionary.Contains(pair).Should().BeFalse();
        }
    }

    [TestCase]
    public void ContainsKey_WhenPopulatingTheDictionaryAndUsingExistingKeys_ReturnsTrue()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.ContainsKey(model.GetHashCode()).Should().BeTrue();
        }
    }

    [TestCase]
    public void ContainsKey_WhenPopulatingTheDictionaryAndUsingNonExistingKeys_ReturnsFalse()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.ContainsKey(model.GetHashCode()).Should().BeFalse();
        }
    }

    [TestCase]
    public void ContainsItem_WhenPopulatingTheDictionaryAndUsingExistingItems_ReturnsTrue()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Contains(model).Should().BeTrue();
        }
    }

    [TestCase]
    public void ContainsItem_WhenPopulatingTheDictionaryAndUsingNonExistingItems_ReturnsFalse()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Contains(model).Should().BeFalse();
        }
    }

    [TestCase]
    public void CopyToKeyValuePairs_FromThePopulatedDictionary_PopulatesTheArrayCorrectly()
    {
        var dictionary = GetDefaultDictionary();
        var keyValuePairs = new KeyValuePair<int, HashCodeModel>[NumberOfModels];

        dictionary.CopyTo(keyValuePairs, 0);

        for (var i = 0; i < keyValuePairs.Length; i++)
        {
            var (key, value) = keyValuePairs[i];
            var model = GetHashCodeModel(i);

            key.Should().Be(model.GetHashCode());
            value.Should().Be(model);
        }
    }

    [TestCase]
    public void CopyToHashCodeDictionary_FromThePopulatedDictionary_PopulatesTheDestinationDictionaryCorrectly()
    {
        var source = GetDefaultDictionary();
        var destination = new HashCodeDictionary<HashCodeModel>();

        source.CopyTo(destination);

        AssertTwoIdenticalDictionaries(source, destination);
    }

    [Test]
    public void RemoveKey_FromThePopulatedDictionaryWithExistingKeys_RemovesAndReturnsTrue()
    {
        var dictionary = GetDefaultDictionary();
        var count = dictionary.Count;

        for (var i = ModelsToRemoveOffset; i < NumberModelsToRemove; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Remove(model.GetHashCode()).Should().BeTrue();
            dictionary.Contains(model).Should().BeFalse();
            dictionary.Should().HaveCount(--count);
        }
    }

    [Test]
    public void RemoveKey_FromThePopulatedDictionaryWithNonExistingKeys_RemovesNothingAndReturnsFalse()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Remove(model.GetHashCode()).Should().BeFalse();
            dictionary.Contains(model).Should().BeFalse();
            dictionary.Should().HaveCount(NumberOfModels);
        }
    }

    [Test]
    public void RemoveKeyValuePair_FromThePopulatedDictionaryWithExistingKeyValuePairs_RemovesAndReturnsTrue()
    {
        var dictionary = GetDefaultDictionary();
        var count = dictionary.Count;

        for (var i = ModelsToRemoveOffset; i < NumberModelsToRemove; i++)
        {
            var model = GetHashCodeModel(i);
            var keyValuePair = new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model);

            dictionary.Remove(keyValuePair).Should().BeTrue();
            dictionary.Contains(keyValuePair).Should().BeFalse();
            dictionary.Should().HaveCount(--count);
        }
    }

    [Test]
    public void RemoveKeyValuePair_FromThePopulatedDictionaryWithNonExistingKeyValuePairs_RemovesNothingAndReturnsFalse()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);
            var keyValuePair = new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model);

            dictionary.Remove(model.GetHashCode()).Should().BeFalse();
            dictionary.Contains(model).Should().BeFalse();
            dictionary.Should().HaveCount(NumberOfModels);
        }
    }

    [Test]
    public void RemoveItem_FromThePopulatedDictionaryWithExistingItems_RemovesAndReturnsTrue()
    {
        var dictionary = GetDefaultDictionary();
        var count = dictionary.Count;

        for (var i = ModelsToRemoveOffset; i < NumberModelsToRemove; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Remove(model).Should().BeTrue();
            dictionary.Contains(model).Should().BeFalse();
            dictionary.Should().HaveCount(--count);
        }
    }

    [Test]
    public void RemoveItem_FromThePopulatedDictionaryWithNonExistingItems_RemovesNothingAndReturnsFalse()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Remove(model.GetHashCode()).Should().BeFalse();
            dictionary.Contains(model).Should().BeFalse();
            dictionary.Should().HaveCount(NumberOfModels);
        }
    }

    [Test]
    public void AddKeyAndValue_ToPopulatedDictionaryWithNewKeysAndValues_AddsNewItemsAsExpected()
    {
        var dictionary = GetDefaultDictionary();
        var count = dictionary.Count;

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Add(model.GetHashCode(), model);

            dictionary.Should().HaveCount(++count);
            dictionary.ContainsKey(model.GetHashCode()).Should().BeTrue();
        }
    }

    [Test]
    public void AddKeyAndValue_ToPopulatedDictionaryWithExistingKeysAndValues_ThrowsArgumentException()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);

            var error = Assert.Throws<ArgumentException>(() => dictionary.Add(model.GetHashCode(), model))!;
            dictionary.Should().HaveCount(NumberOfModels);

            error.Should().NotBeNull();
            error.Message.Should().Be($"An item with the same key has already been added. Key: {model.GetHashCode()}");
        }
    }

    [Test]
    public void AddKeyValuePair_ToPopulatedDictionaryWithNewKeyValuePairs_AddsNewItemsAsExpected()
    {
        var dictionary = GetDefaultDictionary();
        var count = dictionary.Count;

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);
            var pair = new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model);

            dictionary.Add(pair);

            dictionary.Should().HaveCount(++count);
            dictionary.ContainsKey(model.GetHashCode()).Should().BeTrue();
        }
    }

    [Test]
    public void AddKeyValuePair_ToPopulatedDictionaryWithExistingKeyValuePairs_ThrowsArgumentException()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);
            var pair = new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model);

            var error = Assert.Throws<ArgumentException>(() => dictionary.Add(pair))!;
            dictionary.Should().HaveCount(NumberOfModels);

            error.Should().NotBeNull();
            error.Message.Should().Be($"An item with the same key has already been added. Key: {model.GetHashCode()}");
        }
    }

    [Test]
    public void AddItem_ToPopulatedDictionaryWithNewItems_AddsNewItemsAsExpected()
    {
        var dictionary = GetDefaultDictionary();
        var count = dictionary.Count;

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);

            dictionary.Add(model);

            dictionary.Should().HaveCount(++count);
            dictionary.ContainsKey(model.GetHashCode()).Should().BeTrue();
        }
    }

    [Test]
    public void AddItem_ToPopulatedDictionaryWithExistingItems_ThrowsArgumentException()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);

            var error = Assert.Throws<ArgumentException>(() => dictionary.Add(model))!;
            dictionary.Should().HaveCount(NumberOfModels);

            error.Should().NotBeNull();
            error.Message.Should().Be($"An item with the same key has already been added. Key: {model.GetHashCode()}");
        }
    }

    [Test]
    public void Constructor_CopyFromExistingDictionary_CopiesCorrectly()
    {
        var source = GetDefaultDictionary();
        var destination = new HashCodeDictionary<HashCodeModel>(source);

        AssertTwoIdenticalDictionaries(source, destination);
    }

    [Test]
    public void Constructor_CopyFromKeyValuePairs_CopiesCorrectly()
    {
        var source = GetDefaultDictionary();
        var keyValuePairs = new List<KeyValuePair<int, HashCodeModel>>();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);

            keyValuePairs.Add(new KeyValuePair<int, HashCodeModel>(model.GetHashCode(), model));
        }

        var destination = new HashCodeDictionary<HashCodeModel>(keyValuePairs);

        AssertTwoIdenticalDictionaries(source, destination);
    }

    [Test]
    public void TryGetValue_WithAPopulatedDictionaryAndExistingKeys_RetrievesTheCorrectValues()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = 0; i < NumberOfModels; i++)
        {
            var model = GetHashCodeModel(i);
            var result = dictionary.TryGetValue(model.GetHashCode(), out var dictionaryModel);

            result.Should().BeTrue();
            dictionaryModel.Should().Be(model);
            dictionaryModel.GetHashCode().Should().Be(model.GetHashCode());
        }
    }

    [Test]
    public void TryGetValue_WithAPopulatedDictionaryAndNonExistingKeys_RetrievesTheCorrectValues()
    {
        var dictionary = GetDefaultDictionary();

        for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
        {
            var model = GetHashCodeModel(i);
            var result = dictionary.TryGetValue(model.GetHashCode(), out var dictionaryModel);

            result.Should().BeFalse();
            dictionaryModel.Should().BeNull();
        }
    }

    private static void AssertTwoIdenticalDictionaries(
        IDictionary<int, HashCodeModel> left,
        IDictionary<int, HashCodeModel> right)
    {
        foreach (var key in left.Keys)
        {
            var sourceModel = right[key];
            var destinationModel = left[key];

            destinationModel.GetHashCode().Should().Be(sourceModel.GetHashCode());
            destinationModel.Should().Be(sourceModel);
        }
    }

    private static HashCodeModel GetHashCodeModel(int baseValue)
    {
        return new HashCodeModel
        {
            Property1 = baseValue * 10,
            Property2 = baseValue * 100 / 2
        };
    }

    private static IHashCodeDictionary<HashCodeModel> GetDefaultDictionary()
    {
        var result = new HashCodeDictionary<HashCodeModel>();

        for (var i = 0; i < NumberOfModels; i++)
        {
            result.Add(GetHashCodeModel(i));
        }

        return result;
    }
}