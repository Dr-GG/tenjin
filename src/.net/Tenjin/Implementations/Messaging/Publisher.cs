using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Extensions;
using Tenjin.Interfaces.Messaging;
using Tenjin.Models.Messaging;
using Tenjin.Models.Messaging.Configuration;

namespace Tenjin.Implementations.Messaging
{
    public class Publisher<TData> : IPublisher<TData>
    {
        private bool _disposed;
        private PublisherConfiguration _configuration = new();

        private readonly object _root = new();
        private readonly IDictionary<string, ISubscriber<TData>> _subscribers = new Dictionary<string, ISubscriber<TData>>();

        public IPublisher<TData> Configure(PublisherConfiguration configuration)
        {
            lock (_root)
            {
                _configuration = configuration;
            }

            return this;
        }

        public async Task<IEnumerable<IPublisherLock>> Subscribe(params ISubscriber<TData>[] subscribers)
        {
            if (subscribers.IsEmpty())
            {
                return Enumerable.Empty<IPublisherLock>();
            }

            var result = new List<IPublisherLock>(subscribers.Length);

            foreach (var subscriber in subscribers)
            {
                result.Add(await Subscribe(subscriber));
            }

            return result;
        }

        public Task<IPublisherLock> Subscribe(ISubscriber<TData> subscriber)
        {
            AddNewSubscriber(subscriber);

            return Task.FromResult(GetLock(subscriber));
        }

        public Task Unsubscribe(params ISubscriber<TData>[] subscribers)
        {
            lock (_root)
            {
                AssertDisposeState();

                foreach (var subscriber in subscribers)
                {
                    _subscribers.Remove(subscriber.Id);
                }
            }

            return Task.CompletedTask;
        }

        public Task<Guid> Publish(TData data)
        {
            var publishEvent = new PublishEvent<TData>(this, data);

            lock (_root)
            {
                InternalPublish(publishEvent, true);
            }

            return Task.FromResult(publishEvent.Id);
        }

        public ValueTask DisposeAsync()
        {
            Dispose();

            return ValueTask.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            DisposeSubscribers();
        }

        protected virtual IPublisherLock GetLock(ISubscriber<TData> subscriber)
        {
            return new PublisherLock<TData>(this, subscriber);
        }

        private void InternalPublish(PublishEvent<TData> publishEvent, bool checkDisposeState)
        {
            switch (_configuration.Threading.Mode)
            {
                case PublisherThreadMode.Multi: MultiThreadPublish(publishEvent, checkDisposeState); break;
                case PublisherThreadMode.Single: SingleThreadPublish(publishEvent, checkDisposeState); break;
                default: throw new NotSupportedException($"No dispatch method found for threading mode {_configuration.Threading.Mode}");
            }
        }

        private void SingleThreadPublish(PublishEvent<TData> publishEvent, bool checkDisposeState = true)
        {
            if (checkDisposeState)
            {
                AssertDisposeState();
            }

            ConfigurePrePublish(publishEvent);

            foreach (var subscriber in _subscribers.Values)
            {
                subscriber.Receive(publishEvent).GetAwaiter().GetResult();
            }
        }

        private void MultiThreadPublish(PublishEvent<TData> publishEvent, bool checkDisposeState = true)
        {
            if (checkDisposeState)
            {
                AssertDisposeState();
            }

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
            var publishEvent = new PublishEvent<TData>(this, PublishEventType.Disposing);

            lock (_root)
            {
                InternalPublish(publishEvent, false);

                _subscribers.Clear();
                _disposed = true;
            }
        }

        private void AddNewSubscriber(ISubscriber<TData> subscriber)
        {
            lock (_root)
            {
                AssertDisposeState();

                if (_subscribers.DoesNotContainKey(subscriber.Id))
                {
                    _subscribers.Add(subscriber.Id, subscriber);
                }
            }
        }

        private void AssertDisposeState()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Published is disposed");
            }
        }
    }
}
