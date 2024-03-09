using System.Collections;
using System.Collections.Generic;
using Tenjin.Interfaces.Collections;

namespace Tenjin.Implementations.Collections;

/// <summary>
/// The default implementation of the IHashCodeDictionary instance.
/// </summary>
public class HashCodeDictionary<T> : IHashCodeDictionary<T> where T : notnull
{
    private readonly IDictionary<int, T> _dictionary;

    /// <summary>
    /// Creates a new default instance.
    /// </summary>
    public HashCodeDictionary()
    {
        _dictionary = new Dictionary<int, T>();
    }

    /// <summary>
    /// Creates a new instance from an IDictionary.
    /// </summary>
    public HashCodeDictionary(IDictionary<int, T> dictionary)
    {
        _dictionary = new Dictionary<int, T>(dictionary);
    }

    /// <summary>
    /// Creates a new instance from an IEnumerable of KeyValuePair instances.
    /// </summary>
    public HashCodeDictionary(IEnumerable<KeyValuePair<int, T>> collection)
    {
        _dictionary = new Dictionary<int, T>(collection);
    }

    /// <inheritdoc />
    public bool IsReadOnly => _dictionary.IsReadOnly;

    /// <inheritdoc />
    public int Count => _dictionary.Count;

    /// <inheritdoc />
    public ICollection<int> Keys => _dictionary.Keys;

    /// <inheritdoc />
    public ICollection<T> Values => _dictionary.Values;

    /// <inheritdoc />
    public T this[int key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    /// <inheritdoc />
    public T this[T key]
    {
        get => _dictionary[key.GetHashCode()];
        set => _dictionary[key.GetHashCode()] = value;
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    /// <inheritdoc />
    public void Add(int key, T value)
    {
        _dictionary.Add(key, value);
    }

    /// <inheritdoc />
    public void Add(KeyValuePair<int, T> item)
    {
        _dictionary.Add(item);
    }

    /// <inheritdoc />
    public void Add(T item)
    {
        Add(item.GetHashCode(), item);
    }

    /// <inheritdoc />
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <inheritdoc />
    public bool Contains(KeyValuePair<int, T> item)
    {
        return _dictionary.Contains(item);
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
        return _dictionary.ContainsKey(item.GetHashCode());
    }

    /// <inheritdoc />
    public bool ContainsKey(int key)
    {
        return _dictionary.ContainsKey(key);
    }

    /// <inheritdoc />
    public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex)
    {
        _dictionary.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public void CopyTo(IHashCodeDictionary<T> destination)
    {
        using var enumerator = _dictionary.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var (key, value) = enumerator.Current;

            destination.Add(key, value);
        }
    }

    /// <inheritdoc />
    public bool Remove(KeyValuePair<int, T> item)
    {
        return _dictionary.Remove(item);
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        return _dictionary.Remove(item.GetHashCode());
    }

    /// <inheritdoc />
    public bool Remove(int key)
    {
        return _dictionary.Remove(key);
    }

    /// <inheritdoc />
    public bool TryGetValue(int key, out T value)
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        return _dictionary.TryGetValue(key, out value);
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}