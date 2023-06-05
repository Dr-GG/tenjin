using System;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of String extension methods.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines if a String instance is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines if a String instance is not null or empty.
    /// </summary>
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines if two String instances equals one another with StringComparison.OrdinalIgnoreCase.
    /// </summary>
    public static bool EqualsOrdinalIgnoreCase(this string? value, string? compare)
    {
        return string.Equals(value, compare, StringComparison.OrdinalIgnoreCase);
    }
}