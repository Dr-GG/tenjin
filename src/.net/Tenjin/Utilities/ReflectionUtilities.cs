using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tenjin.Models.Reflection;

namespace Tenjin.Utilities;

/// <summary>
/// A collection of utility methods for reflection.
/// </summary>
public static class ReflectionUtilities
{
    /// <summary>
    /// Gets the specified generic type definitions interfaces from a Type instance.
    /// </summary>
    public static IEnumerable<Type> GetGenericTypeDefinitions(Type type, Type genericInterfaceType)
    {
        return type
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType);
    }

    /// <summary>
    /// Gets all implementations that inherit from a specific interface from an assembly.
    /// </summary>
    public static IEnumerable<Type> GetImplementationInterfaceDefinitions(
        Assembly assembly,
        Type interfaceType)
    {
        return assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces().Contains(interfaceType));
    }

    /// <summary>
    /// Gets all implementations that inherit from a generic type definition interface from an assembly.
    /// </summary>
    public static IEnumerable<GenericTypeDefinitionImplementation> GetImplementationGenericTypeDefinitions(
        Assembly assembly,
        Type genericTypeDefinition)
    {
        return assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && GetGenericTypeDefinitions(t, genericTypeDefinition).Any())
            .Select(t => new GenericTypeDefinitionImplementation
            {
                Implementation = t,
                Interfaces = GetGenericTypeDefinitions(t, genericTypeDefinition)
            });
    }
}
