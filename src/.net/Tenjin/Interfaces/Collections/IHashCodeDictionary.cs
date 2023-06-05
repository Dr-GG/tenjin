using System.Collections.Generic;

namespace Tenjin.Interfaces.Collections;

/// <summary>
/// An IDictionary instance that uses the hashcode of an object as its key.
/// </summary>
public interface IHashCodeDictionary<T> : IDictionary<int, T> where T : notnull
{
    /// <summary>
    /// Gets the dictionary instance using the object as an index.
    /// </summary>
    T this[T key] { get; set; }

    /// <summary>
    /// Adds a new instance to the dictionary using the hashcode of the instance.
    /// </summary>
    void Add(T item);

    /// <summary>
    /// Determines if an instance exists in the dictionary using the hashcode of the instance.
    /// </summary>
    bool Contains(T item);

    /// <summary>
    /// Copies the current dictionary instances to a new IHashCodeDictionary instance.
    /// </summary>
    void CopyTo(IHashCodeDictionary<T> destination);

    /// <summary>
    /// Attempts to remove an instance from the dictionary using the hashcode of the instance.
    /// </summary>
    bool Remove(T item);
}