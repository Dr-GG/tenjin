using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Extensions;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers;
using Tenjin.Models.Messaging.Publishers.Configuration;

namespace Tenjin.Implementations.Messaging.Publishers;

/// <summary>
/// The default implementation of the IPublisher interface.
/// </summary>\
public class Publisher<TData> : IPublisher<TData>
{
    private PublisherConfiguration _configuration = new();

    private readonly Lock _root = new();
    private readonly IDictionary<string, ISubscriber<TData>> _subscribers = new Dictionary<string, ISubscriber<TData>>();

    /// <inheritdoc />
    public IPublisher<TData> Configure(PublisherConfiguration configuration)
    {
        lock (_root)
        {
            _configuration = configuration;
        }

        return this;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IPublisherLock>> Subscribe(params ISubscriber<TData>[] subscribers)
    {
        if (subscribers.IsEmpty())
        {
            return [];
        }

        var result = new List<IPublisherLock>(subscribers.Length);

        foreach (var subscriber in subscribers)
        {
            result.Add(await Subscribe(subscriber));
        }

        return result;
    }

    /// <inheritdoc />
    public Task<IPublisherLock> Subscribe(ISubscriber<TData> subscriber)
    {
        AddNewSubscriber(subscriber);

        return Task.FromResult(GetLock(subscriber));
    }

    /// <inheritdoc />
    public Task Unsubscribe(params ISubscriber<TData>[] subscribers)
    {
        lock (_root)
        {
            foreach (var subscriber in subscribers)
            {
                _subscribers.Remove(subscriber.Id);
            }
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<Guid> Publish(TData data)
    {
        var publishEvent = CreatePublishEvent(data);

        lock (_root)
        {
            InternalPublish(publishEvent);
        }

        return Task.FromResult(publishEvent.Id);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Dispose(true);

        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the Publisher instance.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        DisposeSubscribers();
    }

    /// <summary>
    /// The method that is invoked when an event was published.
    /// </summary>
    protected virtual PublishEvent<TData> CreatePublishEvent(TData data)
    {
        return new PublishEvent<TData>(this, data);
    }

    /// <summary>
    /// The method that is invoked when the IPublisher instance is disposing.
    /// </summary>
    protected virtual PublishEvent<TData> CreateDisposeEvent()
    {
        return new PublishEvent<TData>(this, PublishEventType.Disposing);
    }

    /// <summary>
    /// Gets a new IPublisherLock instance.
    /// </summary>
    protected virtual IPublisherLock GetLock(ISubscriber<TData> subscriber)
    {
        return new PublisherLock<TData>(this, subscriber);
    }

    private void InternalPublish(PublishEvent<TData> publishEvent)
    {
        switch (_configuration.Threading.Mode)
        {
            case PublisherThreadMode.Multi: MultiThreadPublish(publishEvent); break;
            case PublisherThreadMode.Single: SingleThreadPublish(publishEvent); break;
            default: throw new NotSupportedException($"No dispatch method found for threading mode {_configuration.Threading.Mode}.");
        }
    }

    private void SingleThreadPublish(PublishEvent<TData> publishEvent)
    {
        ConfigurePrePublish(publishEvent);

        foreach (var subscriber in _subscribers.Values)
        {
            subscriber.Receive(publishEvent).GetAwaiter().GetResult();
        }
    }

    private void MultiThreadPublish(PublishEvent<TData> publishEvent)
    {
        var batchSize = _configuration.Threading.NumberOfThreads ?? Environment.ProcessorCount;

        _subscribers.Values
            .Batch(batchSize)
            .Select(s => s.ToFunctionTask(() => Publish(publishEvent, s)))
            .RunParallel();
    }

    private static async Task Publish(PublishEvent<TData> publishEvent, IEnumerable<ISubscriber<TData>> subscribers)
    {
        ConfigurePrePublish(publishEvent);

        foreach (var subscriber in subscribers)
        {
            await subscriber.Receive(publishEvent);
        }
    }

    private static void ConfigurePrePublish(PublishEvent<TData> publishEvent)
    {
        publishEvent.DispatchTimestamp = DateTime.UtcNow;
    }

    private void DisposeSubscribers()
    {
        var publishEvent = CreateDisposeEvent();

        lock (_root)
        {
            InternalPublish(publishEvent);

            _subscribers.Clear();
        }
    }

    private void AddNewSubscriber(ISubscriber<TData> subscriber)
    {
        lock (_root)
        {
            if (_subscribers.DoesNotContainKey(subscriber.Id))
            {
                _subscribers.Add(subscriber.Id, subscriber);
            }
        }
    }
}