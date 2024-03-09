using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Tenjin.Interfaces.Mappers;
using Tenjin.Models.Reflection;
using Tenjin.Tests.Mappers;
using Tenjin.Tests.Models.Mappers;
using Tenjin.Utilities;

namespace Tenjin.Tests.UtilitiesTests;

[TestFixture, Parallelizable(ParallelScope.Children)]
public static class ReflectionUtilitiesTests
{
    [TestCase(typeof(ComplexStringUnaryMapper), typeof(IUnaryMapper<,>), new[] { typeof(IUnaryMapper<string, int>), typeof(IUnaryMapper<string, double>), typeof(IUnaryMapper<string, bool>) })]
    [TestCase(typeof(ComplexIntegerUnaryMapper), typeof(IUnaryMapper<,>), new[] { typeof(IUnaryMapper<short, int>), typeof(IUnaryMapper<long, int>) })]
    [TestCase(typeof(ComplexStringBinaryMapper), typeof(IBinaryMapper<,>), new[] { typeof(IBinaryMapper<string, int>), typeof(IBinaryMapper<string, double>), typeof(IBinaryMapper<string, bool>) })]
    [TestCase(typeof(ComplexStringBinaryMapper), typeof(string), new Type[0])]
    public static void GetGenericTypeDefinitions_WhenProvidedWithInputParameters_ReturnsTheExpectedOutput(
        Type baseType,
        Type genericTypeDefinition,
        Type[] expectedTypes)
    {
        ReflectionUtilities.GetGenericTypeDefinitions(baseType, genericTypeDefinition);

        expectedTypes.Should().BeEquivalentTo(expectedTypes);
    }

    [Test]
    public static void GetImplementationInterfaceDefinitions_WhenProvidingAnAssemblyAndAnInterface_ReturnsTheExpectedOutput()
    {
        var result = ReflectionUtilities
            .GetImplementationInterfaceDefinitions(typeof(ReflectionUtilitiesTests).Assembly, typeof(IDependencyInjectionUnaryMapper))
            .OrderBy(t => t.Name);

        var expected = new[]
        {
            typeof(ComplexIntegerUnaryMapper),
            typeof(ComplexStringUnaryMapper),
            typeof(LeftToRightMapper),
            typeof(RightToLeftMapper)
        };

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public static void
        IUnaryMapper_GetImplementationGenericTypeDefinitions_WhenProvidingATypeWithAGenericTypeDefinition_ReturnsTheExpectedOutput()
    {
        var result = ReflectionUtilities
            .GetImplementationGenericTypeDefinitions(typeof(ReflectionUtilitiesTests).Assembly, typeof(IUnaryMapper<,>))
            .OrderBy(t => t.Implementation.Name);
        var expected = new[]
        {
            new GenericTypeDefinitionImplementation
            {
                Implementation = typeof(ComplexIntegerUnaryMapper),
                Interfaces = new[] { typeof(IUnaryMapper<short, int>), typeof(IUnaryMapper<long, int>) }
            },
            new GenericTypeDefinitionImplementation
            {
                Implementation = typeof(ComplexStringUnaryMapper),
                Interfaces = new[] { typeof(IUnaryMapper<string, int>), typeof(IUnaryMapper<string, double>), typeof(IUnaryMapper<string, bool>) }
            },
            new GenericTypeDefinitionImplementation
            {
                Implementation = typeof(LeftToRightMapper),
                Interfaces = new[] { typeof(IUnaryMapper<LeftModel, RightModel>) }
            },
            new GenericTypeDefinitionImplementation
            {
                Implementation = typeof(RightToLeftMapper),
                Interfaces = new[] { typeof(IUnaryMapper<RightModel, LeftModel>) }
            }
        };

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public static void IBinaryMapper_GetImplementationGenericTypeDefinitions_WhenProvidingATypeWithAGenericTypeDefinition_ReturnsTheExpectedOutput()
    {
        var result = ReflectionUtilities
            .GetImplementationGenericTypeDefinitions(typeof(ReflectionUtilitiesTests).Assembly, typeof(IBinaryMapper<,>))
            .OrderBy(t => t.Implementation.Name);
        var expected = new[]
        {
            new GenericTypeDefinitionImplementation
            {
                Implementation = typeof(BinaryLeftRightMapper),
                Interfaces = new[] { typeof(IBinaryMapper<LeftModel, RightModel>) }
            },
            new GenericTypeDefinitionImplementation
            {
                Implementation = typeof(ComplexStringBinaryMapper),
                Interfaces = new[] { typeof(IBinaryMapper<string, int>), typeof(IBinaryMapper<string, double>), typeof(IBinaryMapper<string, bool>) }
            }
        };

        result.Should().BeEquivalentTo(expected);
    }
}
