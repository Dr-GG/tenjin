﻿using System;
using Tenjin.Models.Messaging.Publishers.Progress;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for the ProgressEvent class.
/// </summary>
public static class ProgressEventExtensions
{
    /// <summary>
    /// Calculates the percentage value of a ProgressEvent.
    /// </summary>
    public static double Percentage(this ProgressEvent? progressEvent, int numberOfDecimals = 2)
    {
        if (progressEvent == null
            || progressEvent.Current == 0
            || progressEvent.Total == 0)
        {
            return 0;
        }

        return Math.Round((double)progressEvent.Current / progressEvent.Total * 100, numberOfDecimals);
    }
}