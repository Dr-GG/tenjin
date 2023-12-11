using System;

namespace Tenjin.Interfaces.Messaging.Publishers;

/// <summary>
/// An interface that acts as a lock for ISubscriber instances provided by an IPublisher instance.
/// </summary>
public interface IPublisherLock : IDisposable, IAsyncDisposable;