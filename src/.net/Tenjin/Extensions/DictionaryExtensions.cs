using System.Collections.Generic;

namespace Tenjin.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool DoesNotContainKey<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, 
            TKey key)
        {
            return !dictionary.ContainsKey(key);
        }
    }
}
