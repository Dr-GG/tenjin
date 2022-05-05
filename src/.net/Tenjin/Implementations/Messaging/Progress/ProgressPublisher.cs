using System;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Interfaces.Messaging.Progress;
using Tenjin.Models.Messaging.Configuration;
using Tenjin.Models.Messaging.Progress;

namespace Tenjin.Implementations.Messaging.Progress
{
    public class ProgressPublisher : Publisher<ProgressEvent>, IProgressPublisher
    {
        private const int PercentageDecimals = 5;

        private ulong? _fixedIntervalCounter;
        private double? _nextPercentageInterval;
        private ProgressPublisherConfiguration _configuration = new();

        private ulong _current;
        private ulong _total;

        private readonly object _root = new();

        public IProgressPublisher Configure(ProgressPublisherConfiguration configuration)
        {
            AssertConfiguration(configuration);
            InitialiseFromConfiguration(configuration);

            return this;
        }

        public Task Initialise(ulong total)
        {
            lock (_root)
            {
                _current = 0;
                _total = total;
            }

            return Task.CompletedTask;
        }

        public Task Tick()
        {
            return Tick(1);
        }

        public async Task Tick(ulong ticks)
        {
            ProgressEvent? publishEvent = null;

            lock (_root)
            {
                var newCurrent = _current + ticks;

                _current = newCurrent > _total ? _total : newCurrent;

                if (_current == _total || PublishProgressEvent(ticks))
                {
                    publishEvent = GetProgressEvent();
                }
            }

            if (publishEvent != null)
            {
                await Publish(publishEvent);
            }
        }

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

        private ProgressEvent GetProgressEvent()
        {
            return new ProgressEvent(_current, _total);
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
