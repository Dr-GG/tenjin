using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of Task extension methods.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Runs a collection of Func&lt;Task&gt; instances in parallel.
    /// </summary>
    public static void RunParallel(
        this IEnumerable<Func<Task>> tasks,
        CancellationToken cancellationToken = default)
    {
        var runningTasks = tasks
            .Select(Task.Run)
            .ToArray();

        Task.WaitAll(runningTasks, cancellationToken);
    }
}