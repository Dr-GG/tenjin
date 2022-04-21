using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Tenjin.Extensions;
using Tenjin.Tests.Services;

namespace Tenjin.Tests.ExtensionsTests
{
    [TestFixture]
    public class TaskExtensionsTests
    {
        public const int NumberOfThreads = 5;
        public const double TimingBuffer = 1500;

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

            var start = DateTime.Now;

            result.RunParallel();

            var timeMilliseconds = (DateTime.Now - start).TotalMilliseconds;
            var expectedMilliseconds = threadSleep + TimingBuffer;

            Assert.AreEqual(NumberOfThreads, counter.Counter);
            Assert.LessOrEqual(timeMilliseconds, expectedMilliseconds);
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
}
