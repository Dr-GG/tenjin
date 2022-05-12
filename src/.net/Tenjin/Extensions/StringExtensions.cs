using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool EqualsOrdinalIgnoreCase(this string? value, string? compare)
        {
            return string.Equals(value, compare, StringComparison.OrdinalIgnoreCase);
        }
    }
}