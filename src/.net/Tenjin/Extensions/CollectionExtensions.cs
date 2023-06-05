using System.Collections.Generic;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for ICollection instances.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds a range of items to an ICollection instance.
    /// </summary>
    public static void AddRange<T>(this ICollection<T>? collection, IEnumerable<T>? items)
    {
        if (collection == null || items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// Removes a range of items from an ICollection instance.
    /// </summary>
    public static void RemoveRange<T>(this ICollection<T>? collection, IEnumerable<T>? items)
    {
        if (collection == null || items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            collection.Remove(item);
        }
    }
}
