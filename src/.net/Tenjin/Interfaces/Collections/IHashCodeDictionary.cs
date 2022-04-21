using System.Collections.Generic;

namespace Tenjin.Interfaces.Collections
{
    public interface IHashCodeDictionary<T> : IDictionary<int, T> where T : notnull
    {
        T this[T key] { get; set; }

        void Add(T item);
        bool Contains(T item);
        void CopyTo(IHashCodeDictionary<T> destination);
        bool Remove(T item);
    }
}
