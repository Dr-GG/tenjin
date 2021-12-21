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
    }
}
