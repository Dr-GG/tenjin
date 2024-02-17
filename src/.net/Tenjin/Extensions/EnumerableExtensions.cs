using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Tenjin.Models.Enumerables;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for IEnumerable instances.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns an enumerated version of an IEnumerable as an ICollection.
    /// </summary>
    public static ICollection Enumerate(this IEnumerable collection)
    {
        if (collection is ICollection list)
        {
            return list;
        }

        return collection.Cast<object>().ToList();
    }

    /// <summary>
    /// Returns an enumerated version of an IEnumerable as an ICollection.
    /// </summary>
    public static ICollection<T> Enumerate<T>(this IEnumerable<T> collection)
    {
        if (collection is ICollection<T> list)
        {
            return list;
        }

        return collection.ToList();
    }

    /// <summary>
    /// Returns an enumerated version of an IEnumerable as an IList.
    /// </summary>
    public static IList EnumerateToList(this IEnumerable collection)
    {
        if (collection is IList list)
        {
            return list;
        }

        return collection.Cast<object>().ToList();
    }

    /// <summary>
    /// Returns an enumerated version of an IEnumerable as an IList.
    /// </summary>
    public static IList<T> EnumerateToList<T>(this IEnumerable<T> collection)
    {
        if (collection is List<T> list)
        {
            return list;
        }

        return collection.ToList();
    }

    /// <summary>
    /// Returns an enumerated version of an IEnumerable as an Array.
    /// </summary>
    public static T[] EnumerateToArray<T>(this IEnumerable<T> collection)
    {
        if (collection is T[] array)
        {
            return array;
        }

        return collection.ToArray();
    }

    /// <summary>
    /// Determines if an IEnumerable is not empty.
    /// </summary>
    public static bool IsNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable)
    {
        return enumerable?.Any() ?? false;
    }

    /// <summary>
    /// Determines if an IEnumerable is empty.
    /// </summary>
    public static bool IsEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
    {
        return !enumerable?.Any() ?? true;
    }

    /// <summary>
    /// Provides the last index of an IEnumerable instance.
    /// </summary>
    public static int LastIndex<T>(this IEnumerable<T>? collection)
    {
        if (collection == null)
        {
            return -1;
        }

        return collection.Count() - 1;
    }

    /// <summary>
    /// Transforms an IEnumerable into a number of batches of equal or near-equal size.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T>? collection, int numberOfBatches)
    {
        if (collection == null)
        {
            return Enumerable.Empty<IEnumerable<T>>();
        }

        switch (numberOfBatches)
        {
            case < 1:
                throw new ArgumentOutOfRangeException(
                    nameof(numberOfBatches), "Argument cannot be zero or less.");
            case 1:
                return new[] { collection };
        }

        var list = (List<T>)collection.EnumerateToList();

        if (list.Count == 0)
        {
            return Enumerable.Empty<IEnumerable<T>>();
        }

        var finalNumberOfBatches = Math.Min(numberOfBatches, list.Count);
        var batchSize = (int)Math.Floor(list.Count / (double)finalNumberOfBatches);
        var lastBatchSize = batchSize + list.Count % finalNumberOfBatches;
        var batches = new List<IEnumerable<T>>(finalNumberOfBatches);

        for (var i = 0; i < finalNumberOfBatches; i++)
        {
            var index = i * batchSize;
            var arrayBatchSize = i == finalNumberOfBatches - 1
                ? lastBatchSize
                : batchSize;
            var batch = new T[arrayBatchSize];

            list.CopyTo(index, batch, 0, arrayBatchSize);

            batches.Add(batch);
        }

        return batches;
    }

    /// <summary>
    /// Executes a ForEach on each item within an IEnumerable instance.
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T>? collection, Action<T> action)
    {
        if (collection == null)
        {
            return;
        }

        foreach (var item in collection)
        {
            action(item);
        }
    }

    /// <summary>
    /// Determines if an IEnumerable does not contain a specific item.
    /// </summary>
    public static bool DoesNotContain<T>(this IEnumerable<T> collection, T item)
    {
        return !collection.Contains(item);
    }

    /// <summary>
    /// Iterates over an IEnumerable with a given action, allowing for access to index position.
    /// </summary>
    public static void ForLoop<T>(this IEnumerable<T> collection, Action<int, T> action)
    {
        var index = 0;

        foreach (var item in collection)
        {
            action(index++, item);
        }
    }

    /// <summary>
    /// Iterates over an IEnumerable with a given action, allowing for access to index position and a contextual object about the index.
    /// </summary>
    public static void ForLoopWithContext<T>(this IEnumerable<T> collection, Action<ForLoopContext, T> action)
    {
        var enumerated = collection.Enumerate();
        var index = 0;
        var lastIndex = enumerated.LastIndex();
        var context = new ForLoopContext();

        foreach (var item in enumerated)
        {
            context.IsFirst = index == 0;
            context.IsLast = index == lastIndex;
            context.IsInBetween = index > 0 && index < lastIndex;
            context.Index = index++;

            action(context, item);
        }
    }
}