using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tenjin.Models.Mappers;

/// <summary>
/// The data structure used in MapperService implementation.
/// </summary>
[ExcludeFromCodeCoverage]
internal record MapperServiceRecord
{
    /// <summary>
    /// The map method to invoke.
    /// </summary>
    public required MethodInfo Method { get; init; }

    /// <summary>
    /// The mapper.
    /// </summary>
    public required object Mapper { get; init; }
}
