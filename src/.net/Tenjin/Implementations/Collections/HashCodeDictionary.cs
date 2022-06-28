using System.Collections;
using System.Collections.Generic;
using Tenjin.Interfaces.Collections;

namespace Tenjin.Implementations.Collections;

public class HashCodeDictionary<T> : IHashCodeDictionary<T> where T : notnull
{
    private readonly IDictionary<int, T> _dictionary;

    public HashCodeDictionary()
    {
        _dictionary = new Dictionary<int, T>();
    }

    public HashCodeDictionary(IDictionary<int, T> dictionary)
    {
        _dictionary = new Dictionary<int, T>(dictionary);
    }

    public HashCodeDictionary(IEnumerable<KeyValuePair<int, T>> collection)
    {
        _dictionary = new Dictionary<int, T>(collection);
    }

    public bool IsReadOnly => _dictionary.IsReadOnly;
    public int Count => _dictionary.Count;
    public ICollection<int> Keys => _dictionary.Keys;
    public ICollection<T> Values => _dictionary.Values;

    public T this[int key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public T this[T key]
    {
        get => _dictionary[key.GetHashCode()];
        set => _dictionary[key.GetHashCode()] = value;
    }

    public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() 
    {
        return GetEnumerator();
    }

    public void Add(int key, T value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<int, T> item)
    {
        _dictionary.Add(item);
    }

    public void Add(T item)
    {
        Add(item.GetHashCode(), item);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<int, T> item)
    {
        return _dictionary.Contains(item);
    }

    public bool Contains(T item)
    {
        return _dictionary.ContainsKey(item.GetHashCode());
    }

    public bool ContainsKey(int key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex)
    {
        _dictionary.CopyTo(array, arrayIndex);
    }

    public void CopyTo(IHashCodeDictionary<T> destination)
    {
        using var enumerator = _dictionary.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var (key, value) = enumerator.Current;

            destination.Add(key, value);
        }
    }

    public bool Remove(KeyValuePair<int, T> item)
    {
        return _dictionary.Remove(item);
    }

    public bool Remove(T item)
    {
        return _dictionary.Remove(item.GetHashCode());
    }

    public bool Remove(int key)
    {
        return _dictionary.Remove(key);
    }

    public bool TryGetValue(int key, out T value)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        return _dictionary.TryGetValue(key, out value);
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}