﻿using Tenjin.Implementations.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Tests.Models.Messaging;

namespace Tenjin.Tests.Services;

public class TestDiscoverablePublisher<TKey>(TKey key) : Publisher<TestPublishData>, IDiscoverablePublisher<TKey, TestPublishData>
{
    public TKey Id { get; } = key;
}

public class TestDiscoverablePublisherStringId01 : Publisher<TestPublishData>, IDiscoverablePublisher<string, TestPublishData>
{
    public string Id => "01";
}

public class TestDiscoverablePublisherStringId02 : Publisher<TestPublishData>, IDiscoverablePublisher<string, TestPublishData>
{
    public string Id => "02";
}

