using System;
using System.Collections;
using System.Collections.Generic;

namespace Tenjin.Implementations.Comparers;

public class FunctionComparer<TValue> : IComparer, IComparer<TValue>
{
    private readonly Func<TValue, TValue, int> _functionComparer;

    public FunctionComparer(Func<TValue, TValue, int> functionComparer)
    {
        _functionComparer = functionComparer;
    }

    public int Compare(object? x, object? y)
    {
        var result = PreCompare(x, y);

        return result ?? _functionComparer((TValue)x!, (TValue) y!);
    }

    public int Compare(TValue? x, TValue? y)
    {
        var result = PreCompare(x, y);

        return result ?? _functionComparer(x!, y!);
    }

    private static int? PreCompare(object? x, object? y)
    {
        // if statements read easier here as switch statements.

        // ReSharper disable once ConvertIfStatementToSwitchStatement
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