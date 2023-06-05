using System.Collections.Generic;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for the IDictionary interface.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// A method that is used to determine if a key is not contained within an IDictionary interface.
    /// </summary>
    public static bool DoesNotContainKey<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key)
    {
        return !dictionary.ContainsKey(key);
    }
}