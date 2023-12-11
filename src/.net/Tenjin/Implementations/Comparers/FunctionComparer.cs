using System;
using System.Collections;
using System.Collections.Generic;

namespace Tenjin.Implementations.Comparers;

/// <summary>
/// An IComparer and IComparer&lt;TValue&gt; implementation that uses functions as callbacks.
/// </summary>
/// <remarks>
/// Creates a new instance.
/// </remarks>
public class FunctionComparer<TValue>(Func<TValue, TValue, int> functionComparer) : IComparer, IComparer<TValue>
{
    /// <inheritdoc />
    public int Compare(object? x, object? y)
    {
        var result = PreCompare(x, y);

        return result ?? functionComparer((TValue)x!, (TValue)y!);
    }

    /// <inheritdoc />
    public int Compare(TValue? x, TValue? y)
    {
        var result = PreCompare(x, y);

        return result ?? functionComparer(x!, y!);
    }

    private static int? PreCompare(object? x, object? y)
    {
        switch (x)
        {
            case null when y == null: return 0;
            case null: return -1;
        }

        if (y == null)
        {
            return 1;
        }

        return null;
    }
}