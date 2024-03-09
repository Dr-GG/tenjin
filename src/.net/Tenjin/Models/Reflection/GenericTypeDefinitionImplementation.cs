using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tenjin.Models.Reflection;

/// <summary>
/// The data structure that holds an implementation type and its generic type definitions.
/// </summary>
[ExcludeFromCodeCoverage]
public record GenericTypeDefinitionImplementation
{
    /// <summary>
    /// The implementation Type of the generic type definition.
    /// </summary>
    public required Type Implementation { get; init; }

    /// <summary>
    /// The collection of generic type definitions.
    /// </summary>
    public required IEnumerable<Type> Interfaces { get; init; }
}
