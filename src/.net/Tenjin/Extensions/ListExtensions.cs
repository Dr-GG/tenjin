using System;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Implementations.Comparers;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of IList extensions.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Binary inserts an item into an IList instance.
    /// </summary>
    public static void BinaryInsert<T>(this IList<T>? collection,
        T item, Func<T, T, int> comparerAction,
        bool addIfFound = false)
    {
        var comparer = new FunctionComparer<T>(comparerAction);

        BinaryInsert(collection, item, comparer, addIfFound);
    }

    /// <summary>
    /// Binary inserts an item into an IList instance.
    /// </summary>
    public static void BinaryInsert<T>(this IList<T>? collection,
        T item, bool addIfFound = false)
    {
        BinaryInsert(collection, item, Comparer<T>.Default, addIfFound);
    }

    /// <summary>
    /// Binary inserts an item into an IList instance.
    /// </summary>
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

    /// <summary>
    /// Inserts a range of items into an IList instance.
    /// </summary>
    public static void InsertRange<T>(this IList<T>? collection, int index, IEnumerable<T>? items)
    {
        if (collection == null || items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            collection.Insert(index++, item);
        }
    }
}