using System.Collections.Generic;
using NUnit.Framework;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Tests.Models.Messaging;
using Tenjin.Tests.Services;

namespace Tenjin.Tests.ImplementationsTests.MessagingTests.PublishersTests;

[TestFixture]
public class PublisherRegistryTests
{
    private enum TestPublisherType
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

    private static readonly TestPublisherRegistryData<string> StringDiscoverablePublishersData = new()
    {
        PublisherIds = new[]
        {
            "key-1",
            "key-2",
            "key-3",
            "key-4",
            "key-5"
        },

        TestExistingPublisherIds = new []
        {
            "key-2",
            "key-1",
            "key-5",
            "key-4",
            "key-3"
        },

        NonExistingPublisherIds = new[]
        {
            "key--2",
            "key-",
            "keY-5",
            "kEY-4",
            "KEY-3",
            "bogus-key"
        }
    };

    private static readonly TestPublisherRegistryData<int> Int32DiscoverablePublishersData = new()
    {
        PublisherIds = new[]
        {
            1,
            2,
            3,
            4,
            5
        },

        TestExistingPublisherIds = new[]
        {
            2,
            1,
            5,
            4,
            3
        },

        NonExistingPublisherIds = new[]
        {
            11,
            222,
            3333,
            44444,
            555555,
            -1
        }
    };

    private static readonly TestPublisherRegistryData<TestPublisherType> EnumDiscoverablePublishersData = new()
    {
        PublisherIds = new[]
        {
            TestPublisherType.One,
            TestPublisherType.Two,
            TestPublisherType.Three,
            TestPublisherType.Four,
            TestPublisherType.Five
        },

        TestExistingPublisherIds = new[]
        {
            TestPublisherType.Two,
            TestPublisherType.One,
            TestPublisherType.Five,
            TestPublisherType.Four,
            TestPublisherType.Three
        },

        NonExistingPublisherIds = new[]
        {
            (TestPublisherType)0,
            (TestPublisherType)25,
            (TestPublisherType)50,
            (TestPublisherType)75,
            (TestPublisherType)100
        }
    };

    [Test]
    public void GetString_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        Get_WhenProvidedTheCorrectKey_ReturnsThePublisher(StringDiscoverablePublishersData);
    }

    [Test]
    public void GetString_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        Get_WhenProvidedTheIncorrectKey_ThrowsAnError(StringDiscoverablePublishersData);
    }

    [Test]
    public void TryGetString_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        TryGet_WhenProvidedTheCorrectKey_ReturnsThePublisher(StringDiscoverablePublishersData);
    }

    [Test]
    public void TryGetString_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        TryGet_WhenProvidedTheIncorrectKey_ReturnsThePublisher(StringDiscoverablePublishersData);
    }

    [Test]
    public void ThisIndexString_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        ThisIndex_WhenProvidedTheCorrectKey_ReturnsThePublisher(StringDiscoverablePublishersData);
    }

    [Test]
    public void ThisIndexString_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        ThisIndex_WhenProvidedTheIncorrectKey_ThrowsAnError(StringDiscoverablePublishersData);
    }

    [Test]
    public void GetInt32_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        Get_WhenProvidedTheCorrectKey_ReturnsThePublisher(Int32DiscoverablePublishersData);
    }

    [Test]
    public void GetInt32_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        Get_WhenProvidedTheIncorrectKey_ThrowsAnError(Int32DiscoverablePublishersData);
    }

    [Test]
    public void TryGetInt32_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        TryGet_WhenProvidedTheCorrectKey_ReturnsThePublisher(Int32DiscoverablePublishersData);
    }

    [Test]
    public void TryGetInt32_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        TryGet_WhenProvidedTheIncorrectKey_ReturnsThePublisher(Int32DiscoverablePublishersData);
    }

    [Test]
    public void ThisIndexInt32_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        ThisIndex_WhenProvidedTheCorrectKey_ReturnsThePublisher(Int32DiscoverablePublishersData);
    }

    [Test]
    public void ThisIndexInt32_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        ThisIndex_WhenProvidedTheIncorrectKey_ThrowsAnError(Int32DiscoverablePublishersData);
    }

    [Test]
    public void GetEnum_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        Get_WhenProvidedTheCorrectKey_ReturnsThePublisher(EnumDiscoverablePublishersData);
    }

    [Test]
    public void GetEnum_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        Get_WhenProvidedTheIncorrectKey_ThrowsAnError(EnumDiscoverablePublishersData);
    }

    [Test]
    public void TryGetEnum_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        TryGet_WhenProvidedTheCorrectKey_ReturnsThePublisher(EnumDiscoverablePublishersData);
    }

    [Test]
    public void TryGetEnum_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        TryGet_WhenProvidedTheIncorrectKey_ReturnsThePublisher(EnumDiscoverablePublishersData);
    }

    [Test]
    public void ThisIndexEnum_WhenProvidedTheCorrectKey_ReturnsThePublisher()
    {
        ThisIndex_WhenProvidedTheCorrectKey_ReturnsThePublisher(EnumDiscoverablePublishersData);
    }

    [Test]
    public void ThisIndexEnum_WhenProvidedTheIncorrectKey_ThrowsAnError()
    {
        ThisIndex_WhenProvidedTheIncorrectKey_ThrowsAnError(EnumDiscoverablePublishersData);
    }

    private static void TryGet_WhenProvidedTheCorrectKey_ReturnsThePublisher<TKey>(TestPublisherRegistryData<TKey> data)
    {
        var registry = new TestPublisherRegistry<TKey>(data);

        foreach (var id in data.TestExistingPublisherIds)
        {
            var gotPublisher = registry.TryGet(id, out var publisher);

            Assert.IsTrue(gotPublisher);
            Assert.IsNotNull(publisher);
            Assert.AreEqual(id, ((IDiscoverablePublisher<TKey, TestPublishData>)publisher).Id);
        }
    }

    private static void TryGet_WhenProvidedTheIncorrectKey_ReturnsThePublisher<TKey>(TestPublisherRegistryData<TKey> data)
    {
        var registry = new TestPublisherRegistry<TKey>(data);

        foreach (var id in data.NonExistingPublisherIds)
        {
            var gotPublisher = registry.TryGet(id, out var publisher);

            Assert.IsFalse(gotPublisher);
            Assert.IsNull(publisher);
        }
    }

    private static void Get_WhenProvidedTheCorrectKey_ReturnsThePublisher<TKey>(TestPublisherRegistryData<TKey> data)
    {
        var registry = new TestPublisherRegistry<TKey>(data);

        foreach (var id in data.TestExistingPublisherIds)
        {
            var publisher = registry.Get(id);

            Assert.IsNotNull(publisher);
            Assert.AreEqual(id, ((IDiscoverablePublisher<TKey, TestPublishData>)publisher).Id);
        }
    }

    private static void Get_WhenProvidedTheIncorrectKey_ThrowsAnError<TKey>(TestPublisherRegistryData<TKey> data)
    {
        var registry = new TestPublisherRegistry<TKey>(data);

        foreach (var id in data.NonExistingPublisherIds)
        {
            Assert.Throws<KeyNotFoundException>(() => registry.Get(id));
        }
    }

    private static void ThisIndex_WhenProvidedTheCorrectKey_ReturnsThePublisher<TKey>(TestPublisherRegistryData<TKey> data)
    {
        var registry = new TestPublisherRegistry<TKey>(data);

        foreach (var id in data.TestExistingPublisherIds)
        {
            var publisher = registry[id];

            Assert.IsNotNull(publisher);
            Assert.AreEqual(id, ((IDiscoverablePublisher<TKey, TestPublishData>)publisher).Id);
        }
    }

    private static void ThisIndex_WhenProvidedTheIncorrectKey_ThrowsAnError<TKey>(TestPublisherRegistryData<TKey> data)
    {
        var registry = new TestPublisherRegistry<TKey>(data);

        foreach (var id in data.NonExistingPublisherIds)
        {
            Assert.Throws<KeyNotFoundException>(() => registry[id].Dispose());
        }
    }
}