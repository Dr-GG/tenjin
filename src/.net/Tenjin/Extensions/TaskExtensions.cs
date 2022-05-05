using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tenjin.Extensions
{
    public static class TaskExtensions
    {
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
}
