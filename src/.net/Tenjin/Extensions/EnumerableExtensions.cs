using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Tenjin.Implementations.Comparers;

namespace Tenjin.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable)
        {
            return enumerable?.Any() ?? false;
        }

        public static bool IsEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
        {
            return !enumerable?.Any() ?? true;
        }

        public static int LastIndex<T>(this IEnumerable<T>? collection)
        {
            if (collection == null)
            {
                return -1;
            }

            return collection.Count() - 1;
        }

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
                        nameof(numberOfBatches), "Argument cannot be zero or less");
                case 1:
                    return new[] { collection };
            }

            var list = collection.ToList();

            if (list.Count == 0)
            {
                return Enumerable.Empty<IEnumerable<T>>();
            }

            var finalNumberOfBatches = Math.Min(numberOfBatches, list.Count);
            var batchSize = (int)Math.Floor(list.Count / (double)finalNumberOfBatches);
            var lastBatchSize = batchSize + (list.Count % finalNumberOfBatches);
            var batches = new List<IEnumerable<T>>(finalNumberOfBatches);

            for (var i = 0; i < finalNumberOfBatches; i++)
            {
                var index = i * batchSize;
                var arrayBatchSize = i == (finalNumberOfBatches - 1)
                    ? lastBatchSize
                    : batchSize;
                var batch = new T[arrayBatchSize];

                list.CopyTo(index, batch, 0, arrayBatchSize);

                batches.Add(batch);
            }

            return batches;
        }

        public static IEnumerable<T> BinaryInsert<T>(this IEnumerable<T>? collection,
            T item, bool addIfFound = false)
        {
            return BinaryInsert(collection, item, Comparer<T>.Default, addIfFound);
        }

        public static IEnumerable<T> BinaryInsert<T>(this IEnumerable<T>? collection,
            T item, Func<T, T, int> comparerAction, bool addIfFound = false)
        {
            var comparer = new FunctionComparer<T>(comparerAction);

            return BinaryInsert(collection, item, comparer, addIfFound);
        }

        public static IEnumerable<T> BinaryInsert<T>(this IEnumerable<T>? collection,
            T item, IComparer<T> comparer, bool addIfFound = false)
        {
            if (collection == null)
            {
                return Enumerable.Empty<T>();
            }

            var array = collection.ToArray();
            var binaryIndex = Array.BinarySearch(array, item, comparer);

            if (binaryIndex < 0)
            {
                return BinaryInsertMerge(array, item, ~binaryIndex);
            }

            return addIfFound
                ? BinaryInsertMerge(array, item, binaryIndex)
                : array;
        }

        private static IEnumerable<T> BinaryInsertMerge<T>(IEnumerable<T> source, T item, int index)
        {
            var result = new List<T>(source);

            result.Insert(index, item);

            return result;
        }
    }
}
