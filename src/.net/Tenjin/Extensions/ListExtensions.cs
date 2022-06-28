using System;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Implementations.Comparers;

namespace Tenjin.Extensions;

public static class ListExtensions
{
    public static void BinaryInsert<T>(this IList<T>? collection,
        T item, Func<T, T, int> comparerAction, bool addIfFound = false)
    {
        var comparer = new FunctionComparer<T>(comparerAction);

        BinaryInsert(collection, item, comparer, addIfFound);
    }

    public static void BinaryInsert<T>(this IList<T>? collection,
        T item, bool addIfFound = false)
    {
        BinaryInsert(collection, item, Comparer<T>.Default, addIfFound);
    }

    public static void BinaryInsert<T>(this IList<T>? collection,
        T item, IComparer<T> comparer, bool addIfFound = false)
    {
        if (collection == null)
        {
            return;
        }

        var array = collection.ToArray();
        var binaryIndex = Array.BinarySearch(array, item, comparer);

        if (binaryIndex < 0)
        {
            collection.Insert(~binaryIndex, item);
        }
        else if (addIfFound)
        {
            collection.Insert(binaryIndex, item);
        }
    }
}