using System;
using System.Collections;
using System.Collections.Generic;

namespace Tenjin.Implementations.Comparers;

/// <summary>
/// An IComparer and IComparer<TValue> implementation that uses functions as callbacks.
/// </summary>
public class FunctionComparer<TValue> : IComparer, IComparer<TValue>
{
    private readonly Func<TValue, TValue, int> _functionComparer;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public FunctionComparer(Func<TValue, TValue, int> functionComparer)
    {
        _functionComparer = functionComparer;
    }

    /// <inheritdoc />
    public int Compare(object? x, object? y)
    {
        var result = PreCompare(x, y);

        return result ?? _functionComparer((TValue)x!, (TValue)y!);
    }

    /// <inheritdoc />
    public int Compare(TValue? x, TValue? y)
    {
        var result = PreCompare(x, y);

        return result ?? _functionComparer(x!, y!);
    }

    /// <inheritdoc />
    private static int? PreCompare(object? x, object? y)
    {
        if (x == null && y == null)
        {
            return 0;
        }

        if (x == null && y != null)
        {
            return -1;
        }

        if (x != null && y == null)
        {
            return 1;
        }

        return null;
    }
}