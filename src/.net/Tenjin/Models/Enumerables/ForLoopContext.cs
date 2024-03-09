using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Models.Enumerables;

/// <summary>
/// A record that provides context for a for loop.
/// </summary>
[ExcludeFromCodeCoverage]
public record ForLoopContext
{
    /// <summary>
    /// The value indicating if this is the first index.
    /// </summary>
    public bool IsFirst { get; set; }

    /// <summary>
    /// The value indicating if this is an index in between the first and last index.
    /// </summary>
    public bool IsInBetween { get; set; }

    /// <summary>
    /// The value indicating if this is the last index.
    /// </summary>
    public bool IsLast { get; set; }

    /// <summary>
    /// The current index.
    /// </summary>
    public int Index { get; set; }
}
