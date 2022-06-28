using System;
using System.Collections.Generic;

namespace Tenjin.Tests.Utilities;

public static class ThreadingUtilities
{
    public static void IncreaseThreadIdDictionary(IDictionary<int, int> threadIds)
    {
        var threadId = Environment.CurrentManagedThreadId;

        if (threadIds.TryGetValue(threadId, out var count))
        {
            count++;
        }
        else
        {
            count = 1;
        }

        threadIds[threadId] = count;
    }
}