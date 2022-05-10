using System;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Interfaces.Messaging.PublishSubscriber.Progress;
using Tenjin.Models.Messaging.PublisherSubscriber.Configuration;
using Tenjin.Models.Messaging.PublisherSubscriber.Progress;

namespace Tenjin.Implementations.Messaging.PublisherSubscriber.Progress
{
    public abstract class ProgressPublisher<TProgressEvent> : Publisher<TProgressEvent>, IProgressPublisher<TProgressEvent>
        where TProgressEvent : ProgressEvent
    {
        private const int PercentageDecimals = 5;

        private ulong? _fixedIntervalCounter;
        private double? _nextPercentageInterval;
        private ProgressPublisherConfiguration _configuration = new();

        private ulong _current;
        private ulong _total;

        private readonly object _root = new();

        public IProgressPublisher<TProgressEvent> Configure(ProgressPublisherConfiguration configuration)
        {
            AssertConfiguration(configuration);
            InitialiseFromConfiguration(configuration);

            return this;
        }

        public async Task Initialise(ulong total, bool publish = true)
        {
            TProgressEvent? progressEvent = null;

            lock (_root)
            {
                _current = 0;
                _total = total;

                if (publish)
                {
                    progressEvent = CreateProgressEvent(_current, _total);
                }
            }

            if (progressEvent != null)
            {
                await Publish(progressEvent);
            }
        }

        public Task Tick()
        {
            return Tick(1);
        }

        public async Task Tick(ulong ticks)
        {
            TProgressEvent? publishEvent = null;

            lock (_root)
            {
                var newCurrent = _current + ticks;

                _current = newCurrent > _total ? _total : newCurrent;

                if (_current == _total || PublishProgressEvent(ticks))
                {
                    publishEvent = CreateProgressEvent(_current, _total);
                }
            }

            if (publishEvent != null)
            {
                await Publish(publishEvent);
            }
        }

        protected abstract TProgressEvent CreateProgressEvent(ulong current, ulong total);

        private static void AssertConfiguration(ProgressPublisherConfiguration configuration)
        {
            switch (configuration.Interval)
            {
                case ProgressNotificationInterval.FixedInterval: 
                    AssertFixedIntervalConfiguration(configuration);
                    break;

                case ProgressNotificationInterval.PercentageInterval:
                    AssertPercentageIntervalConfiguration(configuration);
                    break;
            }
        }

        private static void AssertFixedIntervalConfiguration(ProgressPublisherConfiguration configuration)
        {
            if (configuration.FixedInterval == null)
            {
                throw new InvalidOperationException("Fixed Interval property not set for fixed interval configuration");
            }
        }

        private static void AssertPercentageIntervalConfiguration(ProgressPublisherConfiguration configuration)
        {
            if (configuration.PercentageInterval == null)
            {
                throw new InvalidOperationException("Percentage Interval property not set for fixed percentage interval configuration");
            }
        }

        private void InitialiseFromConfiguration(ProgressPublisherConfiguration configuration)
        {
            _configuration = configuration;
            _fixedIntervalCounter = null;
            _nextPercentageInterval = null;

            switch (_configuration.Interval)
            {
                case ProgressNotificationInterval.FixedInterval:
                    InitialiseFixedInterval();
                    break;

                case ProgressNotificationInterval.PercentageInterval:
                    InitialisePercentageInterval();
                    break;
            }
        }

        private void InitialisePercentageInterval()
        {
            _nextPercentageInterval = _configuration.PercentageInterval;
        }

        private void InitialiseFixedInterval()
        {
            _fixedIntervalCounter = _configuration.FixedInterval;
        }

        private bool PublishProgressEvent(ulong ticks)
        {
            return _configuration.Interval switch
            {
                ProgressNotificationInterval.None => true,
                ProgressNotificationInterval.FixedInterval => PublishFixedInterval(ticks),
                ProgressNotificationInterval.PercentageInterval => PublishPercentageInterval(),
                _ => false
            };
        }

        private bool PublishFixedInterval(ulong ticks)
        {
            _fixedIntervalCounter = ticks > _fixedIntervalCounter
                ? 0
                : (_fixedIntervalCounter - ticks);

            if (_fixedIntervalCounter > 0)
            {
                return false;
            }

            _fixedIntervalCounter = _configuration.FixedInterval;

            return true;
        }

        private bool PublishPercentageInterval()
        {
            var currentPercentage = Math.Round(((double) _current / _total) * 100.0, PercentageDecimals);

            if (currentPercentage < _nextPercentageInterval)
            {
                return false;
            }

            _nextPercentageInterval += _configuration.PercentageInterval;

            return true;
        }
    }
}
