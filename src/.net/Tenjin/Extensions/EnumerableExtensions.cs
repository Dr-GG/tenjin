using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
    }
}
