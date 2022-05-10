using System;
using Tenjin.Models.Messaging.PublisherSubscriber.Progress;

namespace Tenjin.Extensions
{
    public static class ProgressEventExtensions
    {
        public static double Percentage(this ProgressEvent? progressEvent, int numberOfDecimals = 2)
        {
            if (progressEvent == null
                || progressEvent.Current == 0
                || progressEvent.Total == 0)
            {
                return 0;
            }

            return Math.Round(((double)progressEvent.Current / progressEvent.Total) * 100, numberOfDecimals);
        }
    }
}
