﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tenjin.Extensions;
using Tenjin.Tests.Services;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture]
public class TaskExtensionsTests
{
    public const int NumberOfThreads = 5;

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(50)]
    [TestCase(100)]
    [TestCase(250)]
    [TestCase(500)]
    [TestCase(750)]
    [TestCase(1000)]
    [TestCase(2000)]
    public void RunParallel_WhenRunningMultipleThreadConcurrently_FinishedAllThreadsAsExpected(int threadSleep)
    {
        var counter = new ThreadCounterMonitor();
        var result = new List<Func<Task>>();

        for (var i = 0; i < NumberOfThreads; i++)
        {
            result.Add(() => IncrementCounter(counter, threadSleep));
        }

        result.RunParallel();

        counter.Counter.Should().Be(NumberOfThreads);
    }

    private static Task IncrementCounter(ThreadCounterMonitor counter, int threadSleep = 0)
    {
        if (threadSleep > 0)
        {
            Thread.Sleep(threadSleep);
        }

        counter.Increment();

        return Task.CompletedTask;
    }
}