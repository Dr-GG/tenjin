using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tenjin.Implementations.Collections;
using Tenjin.Interfaces.Collections;
using Tenjin.Tests.Models.HashCodeDictionary;

namespace Tenjin.Tests.ImplementationsTests.CollectionsTests
{
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

                Assert.AreEqual(key, value.GetHashCode());
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

                Assert.AreEqual(compareModel, dictionaryModel);
                Assert.AreEqual(compareModel.GetHashCode(), dictionaryModel.GetHashCode());
            }
        }

        [Test]
        public void ArrayIndexWithKey_WhenPopulatingTheDictionaryAndUsingNonExistingKeys_ThrowsKeyNotFoundException()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
            {
                var compareModel = GetHashCodeModel(i);

                Assert.Throws<KeyNotFoundException>(() =>
                {
                    var _ = dictionary[compareModel.GetHashCode()];
                });
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

                Assert.AreEqual(compareModel, dictionaryModel);
                Assert.AreEqual(compareModel.GetHashCode(), dictionaryModel.GetHashCode());
            }
        }

        [Test]
        public void ArrayIndexWithModel_WhenPopulatingTheDictionaryAndUsingNonExistingModels_ThrowsKeyNotFoundException()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
            {
                var compareModel = GetHashCodeModel(i);

                Assert.Throws<KeyNotFoundException>(() =>
                {
                    var _ = dictionary[compareModel];
                });
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

                Assert.AreEqual(key, model.GetHashCode());
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

                Assert.AreEqual(value.GetHashCode(), model.GetHashCode());
                Assert.AreEqual(value, model);
            }
        }

        [Test]
        public void Clear_WhenPopulatingTheDictionary_ClearsTheEntireDictionary()
        {
            var dictionary = GetDefaultDictionary();

            Assert.AreEqual(NumberOfModels, dictionary.Count);

            dictionary.Clear();

            Assert.IsEmpty(dictionary);
            Assert.IsEmpty(dictionary.Keys);
            Assert.IsEmpty(dictionary.Values);

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

                Assert.IsTrue(dictionary.Contains(pair));
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

                Assert.IsFalse(dictionary.Contains(pair));
            }
        }

        [TestCase]
        public void ContainsKey_WhenPopulatingTheDictionaryAndUsingExistingKeys_ReturnsTrue()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = 0; i < NumberOfModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.IsTrue(dictionary.ContainsKey(model.GetHashCode()));
            }
        }

        [TestCase]
        public void ContainsKey_WhenPopulatingTheDictionaryAndUsingNonExistingKeys_ReturnsFalse()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.IsFalse(dictionary.ContainsKey(model.GetHashCode()));
            }
        }

        [TestCase]
        public void ContainsItem_WhenPopulatingTheDictionaryAndUsingExistingItems_ReturnsTrue()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = 0; i < NumberOfModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.IsTrue(dictionary.Contains(model));
            }
        }

        [TestCase]
        public void ContainsItem_WhenPopulatingTheDictionaryAndUsingNonExistingItems_ReturnsFalse()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.IsFalse(dictionary.Contains(model));
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

                Assert.AreEqual(key, model.GetHashCode());
                Assert.AreEqual(value, model);
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

                Assert.IsTrue(dictionary.Remove(model.GetHashCode()));
                Assert.IsFalse(dictionary.Contains(model));
                Assert.AreEqual(--count, dictionary.Count);
            }
        }

        [Test]
        public void RemoveKey_FromThePopulatedDictionaryWithNonExistingKeys_RemovesNothingAndReturnsFalse()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.IsFalse(dictionary.Remove(model.GetHashCode()));
                Assert.IsFalse(dictionary.Contains(model));
                Assert.AreEqual(NumberOfModels, dictionary.Count);
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

                Assert.IsTrue(dictionary.Remove(keyValuePair));
                Assert.IsFalse(dictionary.Contains(keyValuePair));
                Assert.AreEqual(--count, dictionary.Count);
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

                Assert.IsFalse(dictionary.Remove(keyValuePair));
                Assert.IsFalse(dictionary.Contains(keyValuePair));
                Assert.AreEqual(NumberOfModels, dictionary.Count);
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

                Assert.IsTrue(dictionary.Remove(model));
                Assert.IsFalse(dictionary.Contains(model));
                Assert.AreEqual(--count, dictionary.Count);
            }
        }

        [Test]
        public void RemoveItem_FromThePopulatedDictionaryWithNonExistingItems_RemovesNothingAndReturnsFalse()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = NonExistingModelsOffset; i < NumberOfNonExistingModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.IsFalse(dictionary.Remove(model));
                Assert.IsFalse(dictionary.Contains(model));
                Assert.AreEqual(NumberOfModels, dictionary.Count);
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

                Assert.AreEqual(++count, dictionary.Count);
                Assert.IsTrue(dictionary.ContainsKey(model.GetHashCode()));
            }
        }

        [Test]
        public void AddKeyAndValue_ToPopulatedDictionaryWithExistingKeysAndValues_ThrowsArgumentException()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = 0; i < NumberOfModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.Throws<ArgumentException>(() => dictionary.Add(model.GetHashCode(), model));
                Assert.AreEqual(NumberOfModels, dictionary.Count);
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

                Assert.AreEqual(++count, dictionary.Count);
                Assert.IsTrue(dictionary.ContainsKey(model.GetHashCode()));
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

                Assert.Throws<ArgumentException>(() => dictionary.Add(pair));
                Assert.AreEqual(NumberOfModels, dictionary.Count);
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

                Assert.AreEqual(++count, dictionary.Count);
                Assert.IsTrue(dictionary.ContainsKey(model.GetHashCode()));
            }
        }

        [Test]
        public void AddItem_ToPopulatedDictionaryWithExistingItems_ThrowsArgumentException()
        {
            var dictionary = GetDefaultDictionary();

            for (var i = 0; i < NumberOfModels; i++)
            {
                var model = GetHashCodeModel(i);

                Assert.Throws<ArgumentException>(() => dictionary.Add(model));
                Assert.AreEqual(NumberOfModels, dictionary.Count);
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

                Assert.IsTrue(result);
                Assert.AreEqual(model, dictionaryModel);
                Assert.AreEqual(model.GetHashCode(), dictionaryModel.GetHashCode());
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

                Assert.IsFalse(result);
                Assert.IsNull(dictionaryModel);
            }
        }

        private static void AssertTwoIdenticalDictionaries(
            IHashCodeDictionary<HashCodeModel> left,
            IHashCodeDictionary<HashCodeModel> right)
        {
            foreach (var key in left.Keys)
            {
                var sourceModel = right[key];
                var destinationModel = left[key];

                Assert.AreEqual(sourceModel.GetHashCode(), destinationModel.GetHashCode());
                Assert.AreEqual(sourceModel, destinationModel);
            }
        }

        private static HashCodeModel GetHashCodeModel(int baseValue)
        {
            return new HashCodeModel
            {
                Property1 = baseValue * 10,
                Property2 = (baseValue * 100) / 2
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
}
