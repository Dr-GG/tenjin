using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tenjin.Extensions;
using Tenjin.Tests.Enums;
using Tenjin.Tests.Models.Console;
using Tenjin.Tests.Services;
using Tenjin.Tests.Utilities;

namespace Tenjin.Tests.ExtensionsTests;

[TestFixture]
public class ObjectExtensionsTests
{
    private const int NumberOfTaskThreads = 30;

    private const string DateTimeWriteLineInputFormat = "yyyy-MM-dd";
    private const string DateTimeWriteLineOutputFormat = "dd--MM--yyyy";

    [TestCase(null, "")]
    [TestCase("", null)]
    [TestCase(null, 1)]
    [TestCase(1, null)]
    [TestCase(null, 1.0)]
    [TestCase(1.0, null)]
    [TestCase(null, 1.0f)]
    [TestCase(1.0f, null)]
    [TestCase(null, (long)1)]
    [TestCase((long)1, null)]
    [TestCase(null, (short)1)]
    [TestCase((short)1, null)]
    [TestCase(null, (byte)1)]
    [TestCase((byte)1, null)]
    [TestCase(null, (ulong)1)]
    [TestCase((ulong)1, null)]
    [TestCase(null, (uint)1)]
    [TestCase((uint)1, null)]
    [TestCase(null, (ushort)1)]
    [TestCase((ushort)1, null)]
    [TestCase(null, (char)1)]
    [TestCase((char)1, null)]
    [TestCase(null, true)]
    [TestCase(true, null)]
    [TestCase(null, false)]
    [TestCase(false, null)]
    [TestCase("1", "2")]
    [TestCase("2", "1")]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(1, 2)]
    [TestCase(2, 1)]
    [TestCase(1.0, 2.0)]
    [TestCase(2.0, 1.0)]
    [TestCase(1.0f, 2.0f)]
    [TestCase(2.0f, 1.0f)]
    [TestCase((long)1, (long)2)]
    [TestCase((long)2, (long)1)]
    [TestCase((short)1, (short)2)]
    [TestCase((short)2, (short)1)]
    [TestCase((byte)1, (byte)2)]
    [TestCase((byte)2, (byte)1)]
    [TestCase((ulong)1, (long)2)]
    [TestCase((ulong)2, (long)1)]
    [TestCase((uint)1, (uint)2)]
    [TestCase((uint)2, (uint)1)]
    [TestCase((ushort)1, (short)2)]
    [TestCase((ushort)2, (short)1)]
    [TestCase((char)1, (char)2)]
    [TestCase((char)2, (char)1)]
    [TestCase((long)1, 1)]
    [TestCase((long)1, 1.0)]
    [TestCase((long)1, 1.0f)]
    [TestCase((long)1, (byte)1)]
    [TestCase((long)1, (char)1)]
    [TestCase((long)1, (short)1)]
    [TestCase((long)1, (ulong)1)]
    [TestCase((long)1, (uint)1)]
    [TestCase((long)1, (ushort)1)]
    [TestCase((ulong)1, 1)]
    [TestCase((ulong)1, 1.0)]
    [TestCase((ulong)1, 1.0f)]
    [TestCase((ulong)1, (long)1)]
    [TestCase((ulong)1, (byte)1)]
    [TestCase((ulong)1, (char)1)]
    [TestCase((ulong)1, (short)1)]
    [TestCase((ulong)1, (uint)1)]
    [TestCase((ulong)1, (ushort)1)]
    [TestCase(1, 1.0)]
    [TestCase(1, 1.0f)]
    [TestCase(1, (long)1)]
    [TestCase(1, (byte)1)]
    [TestCase(1, (char)1)]
    [TestCase(1, (short)1)]
    [TestCase(1, (uint)1)]
    [TestCase(1, (ushort)1)]
    [TestCase((uint)1, 1)]
    [TestCase((uint)1, 1.0)]
    [TestCase((uint)1, 1.0f)]
    [TestCase((uint)1, (long)1)]
    [TestCase((uint)1, (byte)1)]
    [TestCase((uint)1, (char)1)]
    [TestCase((uint)1, (short)1)]
    [TestCase((uint)1, (ulong)1)]
    [TestCase((uint)1, (ushort)1)]
    [TestCase((short)1, 1)]
    [TestCase((short)1, 1.0)]
    [TestCase((short)1, 1.0f)]
    [TestCase((short)1, (long)1)]
    [TestCase((short)1, (byte)1)]
    [TestCase((short)1, (char)1)]
    [TestCase((short)1, (ulong)1)]
    [TestCase((short)1, (ushort)1)]
    [TestCase((ushort)1, 1)]
    [TestCase((ushort)1, 1.0)]
    [TestCase((ushort)1, 1.0f)]
    [TestCase((ushort)1, (long)1)]
    [TestCase((ushort)1, (byte)1)]
    [TestCase((ushort)1, (char)1)]
    [TestCase((ushort)1, (short)1)]
    [TestCase((ushort)1, (ulong)1)]
    [TestCase((byte)1, 1)]
    [TestCase((byte)1, 1.0)]
    [TestCase((byte)1, 1.0f)]
    [TestCase((byte)1, (long)1)]
    [TestCase((byte)1, (char)1)]
    [TestCase((byte)1, (short)1)]
    [TestCase((byte)1, (ulong)1)]
    [TestCase((byte)1, (ushort)1)]
    [TestCase((char)1, 1)]
    [TestCase((char)1, 1.0f)]
    [TestCase((char)1, (long)1)]
    [TestCase((char)1, (byte)1)]
    [TestCase((char)1, (short)1)]
    [TestCase((char)1, (ulong)1)]
    [TestCase((char)1, (ushort)1)]
    [TestCase(1.0, 1)]
    [TestCase(1.0, 1.0f)]
    [TestCase(1.0, (long)1)]
    [TestCase(1.0, (byte)1)]
    [TestCase(1.0, (char)1)]
    [TestCase(1.0, (short)1)]
    [TestCase(1.0, (ulong)1)]
    [TestCase(1.0, (ushort)1)]
    [TestCase(1.0f, 1)]
    [TestCase(1.0f, 1.0)]
    [TestCase(1.0f, (long)1)]
    [TestCase(1.0f, (byte)1)]
    [TestCase(1.0f, (char)1)]
    [TestCase(1.0f, (short)1)]
    [TestCase(1.0f, (ulong)1)]
    [TestCase(1.0f, (ushort)1)]
    [TestCase(BooleanEnum.False, BooleanEnum.True)]
    [TestCase(BooleanEnum.True, BooleanEnum.False)]
    [TestCase(BooleanEnum.True, 1)]
    [TestCase(BooleanEnum.True, 1.0f)]
    [TestCase(BooleanEnum.True, (long)1)]
    [TestCase(BooleanEnum.True, (byte)1)]
    [TestCase(BooleanEnum.True, (char)1)]
    [TestCase(BooleanEnum.True, (ulong)1)]
    [TestCase(BooleanEnum.True, (short)1)]
    [TestCase(BooleanEnum.True, (ushort)1)]
    public void DoesNotEqual_WhenProvidingNotEqualObjects_ReturnsTrue(object? left, object? right)
    {
        left.DoesNotEqual(right).Should().BeTrue();
    }

    [TestCase(null, new object?[] { null, null })]
    [TestCase(1, new object?[] { 1, 1 })]
    [TestCase(1.0, new object?[] { 1.0, 1.0 })]
    [TestCase(1.0f, new object?[] { 1.0f, 1.0f })]
    [TestCase((long)1, new object?[] { (long)1, (long)1 })]
    [TestCase((short)1, new object?[] { (short)1, (short)1 })]
    [TestCase((byte)1, new object?[] { (byte)1, (byte)1 })]
    [TestCase((char)1, new object?[] { (char)1, (char)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)1 })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)1 })]
    [TestCase(true, new object?[] { true, true })]
    [TestCase(false, new object?[] { false, false })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, BooleanEnum.False })]
    public void EqualsAll_WhenProvidingMatchingObjects_ReturnsTrue(
        object? root,
        object?[] objects)
    {
        root.EqualsAll(objects).Should().BeTrue();
    }

    [TestCase(null, new object?[] { 1, null })]
    [TestCase(null, new object?[] { null, 1 })]
    [TestCase(1, new object?[] { 2, 1 })]
    [TestCase(1, new object?[] { 2.0, 1.0 })]
    [TestCase(1, new object?[] { 2.0f, 1.0f })]
    [TestCase(1, new object?[] { (byte)2, 1 })]
    [TestCase(1, new object?[] { (char)2, 1 })]
    [TestCase(1, new object?[] { (uint)2, 1 })]
    [TestCase(1, new object?[] { (ushort)2, 1 })]
    [TestCase(1, new object?[] { (ulong)2, 1 })]
    [TestCase(1, new object?[] { (short)2, 1 })]
    [TestCase(1, new object?[] { (long)2, 1 })]
    [TestCase(1, new object?[] { 1, 2 })]
    [TestCase(1, new object?[] { 1.0, 2.0 })]
    [TestCase(1, new object?[] { 1.0f, 2.0f })]
    [TestCase(1, new object?[] { (byte)1, 2 })]
    [TestCase(1, new object?[] { (char)1, 2 })]
    [TestCase(1, new object?[] { (uint)1, 2 })]
    [TestCase(1, new object?[] { (ushort)1, 2 })]
    [TestCase(1, new object?[] { (ulong)1, 2 })]
    [TestCase(1, new object?[] { (short)1, 2 })]
    [TestCase(1, new object?[] { (long)1, 2 })]
    [TestCase(1.0, new object?[] { 2, 1 })]
    [TestCase(1.0, new object?[] { 2.0, 1.0 })]
    [TestCase(1.0, new object?[] { 2.0f, 1.0f })]
    [TestCase(1.0, new object?[] { (byte)2, 1 })]
    [TestCase(1.0, new object?[] { (char)2, 1 })]
    [TestCase(1.0, new object?[] { (uint)2, 1 })]
    [TestCase(1.0, new object?[] { (ushort)2, 1 })]
    [TestCase(1.0, new object?[] { (ulong)2, 1 })]
    [TestCase(1.0, new object?[] { (short)2, 1 })]
    [TestCase(1.0, new object?[] { (long)2, 1 })]
    [TestCase(1.0, new object?[] { 1, 2 })]
    [TestCase(1.0, new object?[] { 1.0, 2.0 })]
    [TestCase(1.0, new object?[] { 1.0f, 2.0f })]
    [TestCase(1.0, new object?[] { (byte)1, 2 })]
    [TestCase(1.0, new object?[] { (char)1, 2 })]
    [TestCase(1.0, new object?[] { (uint)1, 2 })]
    [TestCase(1.0, new object?[] { (ushort)1, 2 })]
    [TestCase(1.0, new object?[] { (ulong)1, 2 })]
    [TestCase(1.0, new object?[] { (short)1, 2 })]
    [TestCase(1.0, new object?[] { (long)1, 2 })]
    [TestCase(1.0f, new object?[] { 2, 1 })]
    [TestCase(1.0f, new object?[] { 2.0, 1.0 })]
    [TestCase(1.0f, new object?[] { 2.0f, 1.0f })]
    [TestCase(1.0f, new object?[] { (byte)2, 1 })]
    [TestCase(1.0f, new object?[] { (char)2, 1 })]
    [TestCase(1.0f, new object?[] { (uint)2, 1 })]
    [TestCase(1.0f, new object?[] { (ushort)2, 1 })]
    [TestCase(1.0f, new object?[] { (ulong)2, 1 })]
    [TestCase(1.0f, new object?[] { (short)2, 1 })]
    [TestCase(1.0f, new object?[] { (long)2, 1 })]
    [TestCase(1.0f, new object?[] { 1, 2 })]
    [TestCase(1.0f, new object?[] { 1.0, 2.0 })]
    [TestCase(1.0f, new object?[] { 1.0f, 2.0f })]
    [TestCase(1.0f, new object?[] { (byte)1, 2 })]
    [TestCase(1.0f, new object?[] { (char)1, 2 })]
    [TestCase(1.0f, new object?[] { (uint)1, 2 })]
    [TestCase(1.0f, new object?[] { (ushort)1, 2 })]
    [TestCase(1.0f, new object?[] { (ulong)1, 2 })]
    [TestCase(1.0f, new object?[] { (short)1, 2 })]
    [TestCase(1.0f, new object?[] { (long)1, 2 })]
    [TestCase((long)1, new object?[] { 2, 1 })]
    [TestCase((long)1, new object?[] { 2.0, 1.0 })]
    [TestCase((long)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((long)1, new object?[] { (byte)2, 1 })]
    [TestCase((long)1, new object?[] { (char)2, 1 })]
    [TestCase((long)1, new object?[] { (uint)2, 1 })]
    [TestCase((long)1, new object?[] { (ushort)2, 1 })]
    [TestCase((long)1, new object?[] { (ulong)2, 1 })]
    [TestCase((long)1, new object?[] { (short)2, 1 })]
    [TestCase((long)1, new object?[] { (long)2, 1 })]
    [TestCase((long)1, new object?[] { 1, 2 })]
    [TestCase((long)1, new object?[] { 1.0, 2.0 })]
    [TestCase((long)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((long)1, new object?[] { (byte)1, 2 })]
    [TestCase((long)1, new object?[] { (char)1, 2 })]
    [TestCase((long)1, new object?[] { (uint)1, 2 })]
    [TestCase((long)1, new object?[] { (ushort)1, 2 })]
    [TestCase((long)1, new object?[] { (ulong)1, 2 })]
    [TestCase((long)1, new object?[] { (short)1, 2 })]
    [TestCase((long)1, new object?[] { (long)1, 2 })]
    [TestCase((short)1, new object?[] { 2, 1 })]
    [TestCase((short)1, new object?[] { 2.0, 1.0 })]
    [TestCase((short)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((short)1, new object?[] { (byte)2, 1 })]
    [TestCase((short)1, new object?[] { (char)2, 1 })]
    [TestCase((short)1, new object?[] { (uint)2, 1 })]
    [TestCase((short)1, new object?[] { (ushort)2, 1 })]
    [TestCase((short)1, new object?[] { (ulong)2, 1 })]
    [TestCase((short)1, new object?[] { (short)2, 1 })]
    [TestCase((short)1, new object?[] { (long)2, 1 })]
    [TestCase((short)1, new object?[] { 1, 2 })]
    [TestCase((short)1, new object?[] { 1.0, 2.0 })]
    [TestCase((short)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((short)1, new object?[] { (byte)1, 2 })]
    [TestCase((short)1, new object?[] { (char)1, 2 })]
    [TestCase((short)1, new object?[] { (uint)1, 2 })]
    [TestCase((short)1, new object?[] { (ushort)1, 2 })]
    [TestCase((short)1, new object?[] { (ulong)1, 2 })]
    [TestCase((short)1, new object?[] { (short)1, 2 })]
    [TestCase((short)1, new object?[] { (long)1, 2 })]
    [TestCase((byte)1, new object?[] { 2, 1 })]
    [TestCase((byte)1, new object?[] { 2.0, 1.0 })]
    [TestCase((byte)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((byte)1, new object?[] { (byte)2, 1 })]
    [TestCase((byte)1, new object?[] { (char)2, 1 })]
    [TestCase((byte)1, new object?[] { (uint)2, 1 })]
    [TestCase((byte)1, new object?[] { (ushort)2, 1 })]
    [TestCase((byte)1, new object?[] { (ulong)2, 1 })]
    [TestCase((byte)1, new object?[] { (short)2, 1 })]
    [TestCase((byte)1, new object?[] { (long)2, 1 })]
    [TestCase((byte)1, new object?[] { 1, 2 })]
    [TestCase((byte)1, new object?[] { 1.0, 2.0 })]
    [TestCase((byte)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((byte)1, new object?[] { (byte)1, 2 })]
    [TestCase((byte)1, new object?[] { (char)1, 2 })]
    [TestCase((byte)1, new object?[] { (uint)1, 2 })]
    [TestCase((byte)1, new object?[] { (ushort)1, 2 })]
    [TestCase((byte)1, new object?[] { (ulong)1, 2 })]
    [TestCase((byte)1, new object?[] { (short)1, 2 })]
    [TestCase((byte)1, new object?[] { (long)1, 2 })]
    [TestCase((char)1, new object?[] { (byte)1, (char)1 })]
    [TestCase((char)1, new object?[] { 2, 1 })]
    [TestCase((char)1, new object?[] { 2.0, 1.0 })]
    [TestCase((char)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((char)1, new object?[] { (byte)2, 1 })]
    [TestCase((char)1, new object?[] { (char)2, 1 })]
    [TestCase((char)1, new object?[] { (uint)2, 1 })]
    [TestCase((char)1, new object?[] { (ushort)2, 1 })]
    [TestCase((char)1, new object?[] { (ulong)2, 1 })]
    [TestCase((char)1, new object?[] { (short)2, 1 })]
    [TestCase((char)1, new object?[] { (long)2, 1 })]
    [TestCase((char)1, new object?[] { 1, 2 })]
    [TestCase((char)1, new object?[] { 1.0, 2.0 })]
    [TestCase((char)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((char)1, new object?[] { (byte)1, 2 })]
    [TestCase((char)1, new object?[] { (char)1, 2 })]
    [TestCase((char)1, new object?[] { (uint)1, 2 })]
    [TestCase((char)1, new object?[] { (ushort)1, 2 })]
    [TestCase((char)1, new object?[] { (ulong)1, 2 })]
    [TestCase((char)1, new object?[] { (short)1, 2 })]
    [TestCase((char)1, new object?[] { (long)1, 2 })]
    [TestCase((ulong)1, new object?[] { (long)1, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { 2, 1 })]
    [TestCase((ulong)1, new object?[] { 2.0, 1.0 })]
    [TestCase((ulong)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((ulong)1, new object?[] { (byte)2, 1 })]
    [TestCase((ulong)1, new object?[] { (char)2, 1 })]
    [TestCase((ulong)1, new object?[] { (uint)2, 1 })]
    [TestCase((ulong)1, new object?[] { (ushort)2, 1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, 1 })]
    [TestCase((ulong)1, new object?[] { (short)2, 1 })]
    [TestCase((ulong)1, new object?[] { (long)2, 1 })]
    [TestCase((ulong)1, new object?[] { 1, 2 })]
    [TestCase((ulong)1, new object?[] { 1.0, 2.0 })]
    [TestCase((ulong)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((ulong)1, new object?[] { (byte)1, 2 })]
    [TestCase((ulong)1, new object?[] { (char)1, 2 })]
    [TestCase((ulong)1, new object?[] { (uint)1, 2 })]
    [TestCase((ulong)1, new object?[] { (ushort)1, 2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2 })]
    [TestCase((ulong)1, new object?[] { (short)1, 2 })]
    [TestCase((ulong)1, new object?[] { (long)1, 2 })]
    [TestCase((uint)1, new object?[] { 1, (uint)1 })]
    [TestCase((uint)1, new object?[] { 2, 1 })]
    [TestCase((uint)1, new object?[] { 2.0, 1.0 })]
    [TestCase((uint)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((uint)1, new object?[] { (byte)2, 1 })]
    [TestCase((uint)1, new object?[] { (char)2, 1 })]
    [TestCase((uint)1, new object?[] { (uint)2, 1 })]
    [TestCase((uint)1, new object?[] { (ushort)2, 1 })]
    [TestCase((uint)1, new object?[] { (ulong)2, 1 })]
    [TestCase((uint)1, new object?[] { (short)2, 1 })]
    [TestCase((uint)1, new object?[] { (long)2, 1 })]
    [TestCase((uint)1, new object?[] { 1, 2 })]
    [TestCase((uint)1, new object?[] { 1.0, 2.0 })]
    [TestCase((uint)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((uint)1, new object?[] { (byte)1, 2 })]
    [TestCase((uint)1, new object?[] { (char)1, 2 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2 })]
    [TestCase((uint)1, new object?[] { (ushort)1, 2 })]
    [TestCase((uint)1, new object?[] { (ulong)1, 2 })]
    [TestCase((uint)1, new object?[] { (short)1, 2 })]
    [TestCase((uint)1, new object?[] { (long)1, 2 })]
    [TestCase((ushort)1, new object?[] { (short)1, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { 2, 1 })]
    [TestCase((ushort)1, new object?[] { 2.0, 1.0 })]
    [TestCase((ushort)1, new object?[] { 2.0f, 1.0f })]
    [TestCase((ushort)1, new object?[] { (byte)2, 1 })]
    [TestCase((ushort)1, new object?[] { (char)2, 1 })]
    [TestCase((ushort)1, new object?[] { (uint)2, 1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, 1 })]
    [TestCase((ushort)1, new object?[] { (ulong)2, 1 })]
    [TestCase((ushort)1, new object?[] { (short)2, 1 })]
    [TestCase((ushort)1, new object?[] { (long)2, 1 })]
    [TestCase((ushort)1, new object?[] { 1, 2 })]
    [TestCase((ushort)1, new object?[] { 1.0, 2.0 })]
    [TestCase((ushort)1, new object?[] { 1.0f, 2.0f })]
    [TestCase((ushort)1, new object?[] { (byte)1, 2 })]
    [TestCase((ushort)1, new object?[] { (char)1, 2 })]
    [TestCase((ushort)1, new object?[] { (uint)1, 2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2 })]
    [TestCase((ushort)1, new object?[] { (ulong)1, 2 })]
    [TestCase((ushort)1, new object?[] { (short)1, 2 })]
    [TestCase((ushort)1, new object?[] { (long)1, 2 })]
    [TestCase(true, new object?[] { false, true })]
    [TestCase(true, new object?[] { 2, 1 })]
    [TestCase(true, new object?[] { 2.0, 1.0 })]
    [TestCase(true, new object?[] { 2.0f, 1.0f })]
    [TestCase(true, new object?[] { (byte)2, 1 })]
    [TestCase(true, new object?[] { (char)2, 1 })]
    [TestCase(true, new object?[] { (uint)2, 1 })]
    [TestCase(true, new object?[] { (ushort)2, 1 })]
    [TestCase(true, new object?[] { (ulong)2, 1 })]
    [TestCase(true, new object?[] { (short)2, 1 })]
    [TestCase(true, new object?[] { (long)2, 1 })]
    [TestCase(true, new object?[] { 1, 2 })]
    [TestCase(true, new object?[] { 1.0, 2.0 })]
    [TestCase(true, new object?[] { 1.0f, 2.0f })]
    [TestCase(true, new object?[] { (byte)1, 2 })]
    [TestCase(true, new object?[] { (char)1, 2 })]
    [TestCase(true, new object?[] { (uint)1, 2 })]
    [TestCase(true, new object?[] { (ushort)1, 2 })]
    [TestCase(true, new object?[] { (ulong)1, 2 })]
    [TestCase(true, new object?[] { (short)1, 2 })]
    [TestCase(true, new object?[] { (long)1, 2 })]
    [TestCase(false, new object?[] { true, false })]
    [TestCase(false, new object?[] { 2, 1 })]
    [TestCase(false, new object?[] { 2.0, 1.0 })]
    [TestCase(false, new object?[] { 2.0f, 1.0f })]
    [TestCase(false, new object?[] { (byte)2, 1 })]
    [TestCase(false, new object?[] { (char)2, 1 })]
    [TestCase(false, new object?[] { (uint)2, 1 })]
    [TestCase(false, new object?[] { (ushort)2, 1 })]
    [TestCase(false, new object?[] { (ulong)2, 1 })]
    [TestCase(false, new object?[] { (short)2, 1 })]
    [TestCase(false, new object?[] { (long)2, 1 })]
    [TestCase(false, new object?[] { 1, 2 })]
    [TestCase(false, new object?[] { 1.0, 2.0 })]
    [TestCase(false, new object?[] { 1.0f, 2.0f })]
    [TestCase(false, new object?[] { (byte)1, 2 })]
    [TestCase(false, new object?[] { (char)1, 2 })]
    [TestCase(false, new object?[] { (uint)1, 2 })]
    [TestCase(false, new object?[] { (ushort)1, 2 })]
    [TestCase(false, new object?[] { (ulong)1, 2 })]
    [TestCase(false, new object?[] { (short)1, 2 })]
    [TestCase(false, new object?[] { (long)1, 2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, BooleanEnum.False })]
    public void EqualsAll_WhenProvidingNonMatchingObjects_ReturnsFalse(
        object? root,
        object?[] objects)
    {
        root.EqualsAll(objects).Should().BeFalse();
    }

    [TestCase(null, new object?[] { null, null })]
    [TestCase(null, new object?[] { null, 1 })]
    [TestCase(1, new object?[] { 2, 1 })]
    [TestCase(1, new object?[] { 2.0, 1 })]
    [TestCase(1, new object?[] { 2.0f, 1 })]
    [TestCase(1, new object?[] { (byte)2, 1 })]
    [TestCase(1, new object?[] { (char)2, 1 })]
    [TestCase(1, new object?[] { (uint)2, 1 })]
    [TestCase(1, new object?[] { (ushort)2, 1 })]
    [TestCase(1, new object?[] { (ulong)2, 1 })]
    [TestCase(1, new object?[] { (short)2, 1 })]
    [TestCase(1, new object?[] { (long)2, 1 })]
    [TestCase(1, new object?[] { 1, 2 })]
    [TestCase(1, new object?[] { 1, 2.0 })]
    [TestCase(1, new object?[] { 1, 2.0f })]
    [TestCase(1, new object?[] { 1, (byte)2 })]
    [TestCase(1, new object?[] { 1, (char)2 })]
    [TestCase(1, new object?[] { 1, (uint)2 })]
    [TestCase(1, new object?[] { 1, (ushort)2 })]
    [TestCase(1, new object?[] { 1, (ulong)2 })]
    [TestCase(1, new object?[] { 1, (short)2 })]
    [TestCase(1, new object?[] { 1, (long)2 })]
    [TestCase(1.0, new object?[] { 2, 1.0 })]
    [TestCase(1.0, new object?[] { 2.0, 1.0 })]
    [TestCase(1.0, new object?[] { 2.0f, 1.0 })]
    [TestCase(1.0, new object?[] { (byte)2, 1.0 })]
    [TestCase(1.0, new object?[] { (char)2, 1.0 })]
    [TestCase(1.0, new object?[] { (uint)2, 1.0 })]
    [TestCase(1.0, new object?[] { (ushort)2, 1.0 })]
    [TestCase(1.0, new object?[] { (ulong)2, 1.0 })]
    [TestCase(1.0, new object?[] { (short)2, 1.0 })]
    [TestCase(1.0, new object?[] { (long)2, 1.0 })]
    [TestCase(1.0, new object?[] { 1.0, 2 })]
    [TestCase(1.0, new object?[] { 1.0, 2.0 })]
    [TestCase(1.0, new object?[] { 1.0, 2.0f })]
    [TestCase(1.0, new object?[] { 1.0, (byte)2 })]
    [TestCase(1.0, new object?[] { 1.0, (char)2 })]
    [TestCase(1.0, new object?[] { 1.0, (uint)2 })]
    [TestCase(1.0, new object?[] { 1.0, (ushort)2 })]
    [TestCase(1.0, new object?[] { 1.0, (ulong)2 })]
    [TestCase(1.0, new object?[] { 1.0, (short)2 })]
    [TestCase(1.0, new object?[] { 1.0, (long)2 })]
    [TestCase(1.0f, new object?[] { 2, 1.0f })]
    [TestCase(1.0f, new object?[] { 2.0, 1.0f })]
    [TestCase(1.0f, new object?[] { 2.0f, 1.0f })]
    [TestCase(1.0f, new object?[] { (byte)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (char)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (uint)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (ushort)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (ulong)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (short)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (long)2, 1.0f })]
    [TestCase(1.0f, new object?[] { 1.0f, 2 })]
    [TestCase(1.0f, new object?[] { 1.0f, 2.0 })]
    [TestCase(1.0f, new object?[] { 1.0f, 2.0f })]
    [TestCase(1.0f, new object?[] { 1.0f, (byte)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (char)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (uint)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (ushort)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (ulong)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (short)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (long)2 })]
    [TestCase((long)1, new object?[] { 2, (long)1 })]
    [TestCase((long)1, new object?[] { 2.0, (long)1 })]
    [TestCase((long)1, new object?[] { 2.0f, (long)1 })]
    [TestCase((long)1, new object?[] { (byte)2, (long)1 })]
    [TestCase((long)1, new object?[] { (char)2, (long)1 })]
    [TestCase((long)1, new object?[] { (uint)2, (long)1 })]
    [TestCase((long)1, new object?[] { (ushort)2, (long)1 })]
    [TestCase((long)1, new object?[] { (ulong)2, (long)1 })]
    [TestCase((long)1, new object?[] { (short)2, (long)1 })]
    [TestCase((long)1, new object?[] { (long)2, (long)1 })]
    [TestCase((long)1, new object?[] { (long)1, 2 })]
    [TestCase((long)1, new object?[] { (long)1, 2.0 })]
    [TestCase((long)1, new object?[] { (long)1, 2.0f })]
    [TestCase((long)1, new object?[] { (long)1, (byte)2 })]
    [TestCase((long)1, new object?[] { (long)1, (char)2 })]
    [TestCase((long)1, new object?[] { (long)1, (uint)2 })]
    [TestCase((long)1, new object?[] { (long)1, (ushort)2 })]
    [TestCase((long)1, new object?[] { (long)1, (ulong)2 })]
    [TestCase((long)1, new object?[] { (long)1, (short)2 })]
    [TestCase((long)1, new object?[] { (long)1, (long)2 })]
    [TestCase((short)1, new object?[] { 2, (short)1 })]
    [TestCase((short)1, new object?[] { 2.0, (short)1 })]
    [TestCase((short)1, new object?[] { 2.0f, (short)1 })]
    [TestCase((short)1, new object?[] { (byte)2, (short)1 })]
    [TestCase((short)1, new object?[] { (char)2, (short)1 })]
    [TestCase((short)1, new object?[] { (uint)2, (short)1 })]
    [TestCase((short)1, new object?[] { (ushort)2, (short)1 })]
    [TestCase((short)1, new object?[] { (ulong)2, (short)1 })]
    [TestCase((short)1, new object?[] { (short)2, (short)1 })]
    [TestCase((short)1, new object?[] { (long)2, (short)1 })]
    [TestCase((short)1, new object?[] { (short)1, 2 })]
    [TestCase((short)1, new object?[] { (short)1, 2.0 })]
    [TestCase((short)1, new object?[] { (short)1, 2.0f })]
    [TestCase((short)1, new object?[] { (short)1, (byte)2 })]
    [TestCase((short)1, new object?[] { (short)1, (char)2 })]
    [TestCase((short)1, new object?[] { (short)1, (uint)2 })]
    [TestCase((short)1, new object?[] { (short)1, (ushort)2 })]
    [TestCase((short)1, new object?[] { (short)1, (ulong)2 })]
    [TestCase((short)1, new object?[] { (short)1, (short)2 })]
    [TestCase((short)1, new object?[] { (short)1, (long)2 })]
    [TestCase((byte)1, new object?[] { 2, (byte)1 })]
    [TestCase((byte)1, new object?[] { 2.0, (byte)1 })]
    [TestCase((byte)1, new object?[] { 2.0f, (byte)1 })]
    [TestCase((byte)1, new object?[] { (byte)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (char)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (uint)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (ushort)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (ulong)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (byte)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (long)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (byte)1, 2 })]
    [TestCase((byte)1, new object?[] { (byte)1, 2.0 })]
    [TestCase((byte)1, new object?[] { (byte)1, 2.0f })]
    [TestCase((byte)1, new object?[] { (byte)1, (byte)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (char)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (uint)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (ushort)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (ulong)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (byte)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (long)2 })]
    [TestCase((char)1, new object?[] { 2, (char)1 })]
    [TestCase((char)1, new object?[] { 2.0, (char)1 })]
    [TestCase((char)1, new object?[] { 2.0f, (char)1 })]
    [TestCase((char)1, new object?[] { (char)2, (char)1 })]
    [TestCase((char)1, new object?[] { (char)2, (char)1 })]
    [TestCase((char)1, new object?[] { (uint)2, (char)1 })]
    [TestCase((char)1, new object?[] { (ushort)2, (char)1 })]
    [TestCase((char)1, new object?[] { (ulong)2, (char)1 })]
    [TestCase((char)1, new object?[] { (char)2, (char)1 })]
    [TestCase((char)1, new object?[] { (long)2, (char)1 })]
    [TestCase((char)1, new object?[] { (char)1, 2 })]
    [TestCase((char)1, new object?[] { (char)1, 2.0 })]
    [TestCase((char)1, new object?[] { (char)1, 2.0f })]
    [TestCase((char)1, new object?[] { (char)1, (char)2 })]
    [TestCase((char)1, new object?[] { (char)1, (char)2 })]
    [TestCase((char)1, new object?[] { (char)1, (uint)2 })]
    [TestCase((char)1, new object?[] { (char)1, (ushort)2 })]
    [TestCase((char)1, new object?[] { (char)1, (ulong)2 })]
    [TestCase((char)1, new object?[] { (char)1, (char)2 })]
    [TestCase((char)1, new object?[] { (char)1, (long)2 })]
    [TestCase((ulong)1, new object?[] { 2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { 2.0, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { 2.0f, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (char)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (uint)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ushort)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (long)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2.0 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2.0f })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (char)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (uint)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ushort)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (long)2 })]
    [TestCase((uint)1, new object?[] { 2, (uint)1 })]
    [TestCase((uint)1, new object?[] { 2.0, (uint)1 })]
    [TestCase((uint)1, new object?[] { 2.0f, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (char)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (ushort)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (ulong)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (long)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2.0 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2.0f })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (char)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (ushort)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (ulong)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (long)2 })]
    [TestCase((ushort)1, new object?[] { 2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { 2.0, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { 2.0f, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (char)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (uint)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ulong)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (long)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2.0 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2.0f })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (char)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (uint)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ulong)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (long)2 })]
    [TestCase(true, new object?[] { 2, true })]
    [TestCase(true, new object?[] { 2.0, true })]
    [TestCase(true, new object?[] { 2.0f, true })]
    [TestCase(true, new object?[] { (byte)2, true })]
    [TestCase(true, new object?[] { (char)2, true })]
    [TestCase(true, new object?[] { (uint)2, true })]
    [TestCase(true, new object?[] { (ushort)2, true })]
    [TestCase(true, new object?[] { (ulong)2, true })]
    [TestCase(true, new object?[] { (byte)2, true })]
    [TestCase(true, new object?[] { (long)2, true })]
    [TestCase(true, new object?[] { true, 2 })]
    [TestCase(true, new object?[] { true, 2.0 })]
    [TestCase(true, new object?[] { true, 2.0f })]
    [TestCase(true, new object?[] { true, (byte)2 })]
    [TestCase(true, new object?[] { true, (char)2 })]
    [TestCase(true, new object?[] { true, (uint)2 })]
    [TestCase(true, new object?[] { true, (ushort)2 })]
    [TestCase(true, new object?[] { true, (ulong)2 })]
    [TestCase(true, new object?[] { true, (byte)2 })]
    [TestCase(true, new object?[] { true, (long)2 })]
    [TestCase(false, new object?[] { 2, false })]
    [TestCase(false, new object?[] { 2.0, false })]
    [TestCase(false, new object?[] { 2.0f, false })]
    [TestCase(false, new object?[] { (byte)2, false })]
    [TestCase(false, new object?[] { (char)2, false })]
    [TestCase(false, new object?[] { (uint)2, false })]
    [TestCase(false, new object?[] { (ushort)2, false })]
    [TestCase(false, new object?[] { (ulong)2, false })]
    [TestCase(false, new object?[] { (byte)2, false })]
    [TestCase(false, new object?[] { (long)2, false })]
    [TestCase(false, new object?[] { false, 2 })]
    [TestCase(false, new object?[] { false, 2.0 })]
    [TestCase(false, new object?[] { false, 2.0f })]
    [TestCase(false, new object?[] { false, (byte)2 })]
    [TestCase(false, new object?[] { false, (char)2 })]
    [TestCase(false, new object?[] { false, (uint)2 })]
    [TestCase(false, new object?[] { false, (ushort)2 })]
    [TestCase(false, new object?[] { false, (ulong)2 })]
    [TestCase(false, new object?[] { false, (byte)2 })]
    [TestCase(false, new object?[] { false, (long)2 })]
    [TestCase(BooleanEnum.True, new object?[] { 2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { 2.0, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { 2.0f, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (byte)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (char)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (uint)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (ushort)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (ulong)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (byte)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (long)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, 2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, 2.0 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, 2.0f })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (byte)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (char)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (uint)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (ushort)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (ulong)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (byte)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (long)2 })]
    [TestCase(BooleanEnum.False, new object?[] { 2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { 2.0, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { 2.0f, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (byte)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (char)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (uint)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (ushort)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (ulong)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (byte)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (long)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, 2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, 2.0 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, 2.0f })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (byte)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (char)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (uint)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (ushort)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (ulong)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (byte)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (long)2 })]
    public void EqualsAll_WhenProvidingSomeMatchingObjects_ReturnsTrue(
        object? root,
        object?[] objects)
    {
        root.EqualsAny(objects).Should().BeTrue();
    }

    [TestCase(null, new object?[] { 1, null })]
    [TestCase(null, new object?[] { null, 1 })]
    [TestCase(1, new object?[] { 2, 1 })]
    [TestCase(1, new object?[] { 2.0, 1 })]
    [TestCase(1, new object?[] { 2.0f, 1 })]
    [TestCase(1, new object?[] { (byte)2, 1 })]
    [TestCase(1, new object?[] { (char)2, 1 })]
    [TestCase(1, new object?[] { (uint)2, 1 })]
    [TestCase(1, new object?[] { (ushort)2, 1 })]
    [TestCase(1, new object?[] { (ulong)2, 1 })]
    [TestCase(1, new object?[] { (short)2, 1 })]
    [TestCase(1, new object?[] { (long)2, 1 })]
    [TestCase(1, new object?[] { 1, 2 })]
    [TestCase(1, new object?[] { 1, 2.0 })]
    [TestCase(1, new object?[] { 1, 2.0f })]
    [TestCase(1, new object?[] { 1, (byte)2 })]
    [TestCase(1, new object?[] { 1, (char)2 })]
    [TestCase(1, new object?[] { 1, (uint)2 })]
    [TestCase(1, new object?[] { 1, (ushort)2 })]
    [TestCase(1, new object?[] { 1, (ulong)2 })]
    [TestCase(1, new object?[] { 1, (short)2 })]
    [TestCase(1, new object?[] { 1, (long)2 })]
    [TestCase(1.0, new object?[] { 2, 1.0 })]
    [TestCase(1.0, new object?[] { 2.0, 1.0 })]
    [TestCase(1.0, new object?[] { 2.0f, 1.0 })]
    [TestCase(1.0, new object?[] { (byte)2, 1.0 })]
    [TestCase(1.0, new object?[] { (char)2, 1.0 })]
    [TestCase(1.0, new object?[] { (uint)2, 1.0 })]
    [TestCase(1.0, new object?[] { (ushort)2, 1.0 })]
    [TestCase(1.0, new object?[] { (ulong)2, 1.0 })]
    [TestCase(1.0, new object?[] { (short)2, 1.0 })]
    [TestCase(1.0, new object?[] { (long)2, 1.0 })]
    [TestCase(1.0, new object?[] { 1.0, 2 })]
    [TestCase(1.0, new object?[] { 1.0, 2.0 })]
    [TestCase(1.0, new object?[] { 1.0, 2.0f })]
    [TestCase(1.0, new object?[] { 1.0, (byte)2 })]
    [TestCase(1.0, new object?[] { 1.0, (char)2 })]
    [TestCase(1.0, new object?[] { 1.0, (uint)2 })]
    [TestCase(1.0, new object?[] { 1.0, (ushort)2 })]
    [TestCase(1.0, new object?[] { 1.0, (ulong)2 })]
    [TestCase(1.0, new object?[] { 1.0, (short)2 })]
    [TestCase(1.0, new object?[] { 1.0, (long)2 })]
    [TestCase(1.0f, new object?[] { 2, 1.0f })]
    [TestCase(1.0f, new object?[] { 2.0, 1.0f })]
    [TestCase(1.0f, new object?[] { 2.0f, 1.0f })]
    [TestCase(1.0f, new object?[] { (byte)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (char)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (uint)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (ushort)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (ulong)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (short)2, 1.0f })]
    [TestCase(1.0f, new object?[] { (long)2, 1.0f })]
    [TestCase(1.0f, new object?[] { 1.0f, 2 })]
    [TestCase(1.0f, new object?[] { 1.0f, 2.0 })]
    [TestCase(1.0f, new object?[] { 1.0f, 2.0f })]
    [TestCase(1.0f, new object?[] { 1.0f, (byte)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (char)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (uint)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (ushort)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (ulong)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (short)2 })]
    [TestCase(1.0f, new object?[] { 1.0f, (long)2 })]
    [TestCase((long)1, new object?[] { 2, (long)1 })]
    [TestCase((long)1, new object?[] { 2.0, (long)1 })]
    [TestCase((long)1, new object?[] { 2.0f, (long)1 })]
    [TestCase((long)1, new object?[] { (byte)2, (long)1 })]
    [TestCase((long)1, new object?[] { (char)2, (long)1 })]
    [TestCase((long)1, new object?[] { (uint)2, (long)1 })]
    [TestCase((long)1, new object?[] { (ushort)2, (long)1 })]
    [TestCase((long)1, new object?[] { (ulong)2, (long)1 })]
    [TestCase((long)1, new object?[] { (short)2, (long)1 })]
    [TestCase((long)1, new object?[] { (long)2, (long)1 })]
    [TestCase((long)1, new object?[] { (long)1, 2 })]
    [TestCase((long)1, new object?[] { (long)1, 2.0 })]
    [TestCase((long)1, new object?[] { (long)1, 2.0f })]
    [TestCase((long)1, new object?[] { (long)1, (byte)2 })]
    [TestCase((long)1, new object?[] { (long)1, (char)2 })]
    [TestCase((long)1, new object?[] { (long)1, (uint)2 })]
    [TestCase((long)1, new object?[] { (long)1, (ushort)2 })]
    [TestCase((long)1, new object?[] { (long)1, (ulong)2 })]
    [TestCase((long)1, new object?[] { (long)1, (short)2 })]
    [TestCase((long)1, new object?[] { (long)1, (long)2 })]
    [TestCase((short)1, new object?[] { 2, (short)1 })]
    [TestCase((short)1, new object?[] { 2.0, (short)1 })]
    [TestCase((short)1, new object?[] { 2.0f, (short)1 })]
    [TestCase((short)1, new object?[] { (byte)2, (short)1 })]
    [TestCase((short)1, new object?[] { (char)2, (short)1 })]
    [TestCase((short)1, new object?[] { (uint)2, (short)1 })]
    [TestCase((short)1, new object?[] { (ushort)2, (short)1 })]
    [TestCase((short)1, new object?[] { (ulong)2, (short)1 })]
    [TestCase((short)1, new object?[] { (short)2, (short)1 })]
    [TestCase((short)1, new object?[] { (long)2, (short)1 })]
    [TestCase((short)1, new object?[] { (short)1, 2 })]
    [TestCase((short)1, new object?[] { (short)1, 2.0 })]
    [TestCase((short)1, new object?[] { (short)1, 2.0f })]
    [TestCase((short)1, new object?[] { (short)1, (byte)2 })]
    [TestCase((short)1, new object?[] { (short)1, (char)2 })]
    [TestCase((short)1, new object?[] { (short)1, (uint)2 })]
    [TestCase((short)1, new object?[] { (short)1, (ushort)2 })]
    [TestCase((short)1, new object?[] { (short)1, (ulong)2 })]
    [TestCase((short)1, new object?[] { (short)1, (short)2 })]
    [TestCase((short)1, new object?[] { (short)1, (long)2 })]
    [TestCase((byte)1, new object?[] { 2, (byte)1 })]
    [TestCase((byte)1, new object?[] { 2.0, (byte)1 })]
    [TestCase((byte)1, new object?[] { 2.0f, (byte)1 })]
    [TestCase((byte)1, new object?[] { (byte)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (char)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (uint)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (ushort)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (ulong)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (byte)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (long)2, (byte)1 })]
    [TestCase((byte)1, new object?[] { (byte)1, 2 })]
    [TestCase((byte)1, new object?[] { (byte)1, 2.0 })]
    [TestCase((byte)1, new object?[] { (byte)1, 2.0f })]
    [TestCase((byte)1, new object?[] { (byte)1, (byte)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (char)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (uint)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (ushort)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (ulong)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (byte)2 })]
    [TestCase((byte)1, new object?[] { (byte)1, (long)2 })]
    [TestCase((char)1, new object?[] { 2, (char)1 })]
    [TestCase((char)1, new object?[] { 2.0, (char)1 })]
    [TestCase((char)1, new object?[] { 2.0f, (char)1 })]
    [TestCase((char)1, new object?[] { (char)2, (char)1 })]
    [TestCase((char)1, new object?[] { (char)2, (char)1 })]
    [TestCase((char)1, new object?[] { (uint)2, (char)1 })]
    [TestCase((char)1, new object?[] { (ushort)2, (char)1 })]
    [TestCase((char)1, new object?[] { (ulong)2, (char)1 })]
    [TestCase((char)1, new object?[] { (char)2, (char)1 })]
    [TestCase((char)1, new object?[] { (long)2, (char)1 })]
    [TestCase((char)1, new object?[] { (char)1, 2 })]
    [TestCase((char)1, new object?[] { (char)1, 2.0 })]
    [TestCase((char)1, new object?[] { (char)1, 2.0f })]
    [TestCase((char)1, new object?[] { (char)1, (char)2 })]
    [TestCase((char)1, new object?[] { (char)1, (char)2 })]
    [TestCase((char)1, new object?[] { (char)1, (uint)2 })]
    [TestCase((char)1, new object?[] { (char)1, (ushort)2 })]
    [TestCase((char)1, new object?[] { (char)1, (ulong)2 })]
    [TestCase((char)1, new object?[] { (char)1, (char)2 })]
    [TestCase((char)1, new object?[] { (char)1, (long)2 })]
    [TestCase((ulong)1, new object?[] { 2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { 2.0, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { 2.0f, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (char)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (uint)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ushort)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (long)2, (ulong)1 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2.0 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, 2.0f })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (char)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (uint)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ushort)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)1, (long)2 })]
    [TestCase((uint)1, new object?[] { 2, (uint)1 })]
    [TestCase((uint)1, new object?[] { 2.0, (uint)1 })]
    [TestCase((uint)1, new object?[] { 2.0f, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (char)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (ushort)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (ulong)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (long)2, (uint)1 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2.0 })]
    [TestCase((uint)1, new object?[] { (uint)1, 2.0f })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (char)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (ushort)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (ulong)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)1, (long)2 })]
    [TestCase((ushort)1, new object?[] { 2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { 2.0, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { 2.0f, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (char)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (uint)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ulong)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (long)2, (ushort)1 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2.0 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, 2.0f })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (char)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (uint)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ulong)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)1, (long)2 })]
    [TestCase(true, new object?[] { 2, true })]
    [TestCase(true, new object?[] { 2.0, true })]
    [TestCase(true, new object?[] { 2.0f, true })]
    [TestCase(true, new object?[] { (byte)2, true })]
    [TestCase(true, new object?[] { (char)2, true })]
    [TestCase(true, new object?[] { (uint)2, true })]
    [TestCase(true, new object?[] { (ushort)2, true })]
    [TestCase(true, new object?[] { (ulong)2, true })]
    [TestCase(true, new object?[] { (byte)2, true })]
    [TestCase(true, new object?[] { (long)2, true })]
    [TestCase(true, new object?[] { true, 2 })]
    [TestCase(true, new object?[] { true, 2.0 })]
    [TestCase(true, new object?[] { true, 2.0f })]
    [TestCase(true, new object?[] { true, (byte)2 })]
    [TestCase(true, new object?[] { true, (char)2 })]
    [TestCase(true, new object?[] { true, (uint)2 })]
    [TestCase(true, new object?[] { true, (ushort)2 })]
    [TestCase(true, new object?[] { true, (ulong)2 })]
    [TestCase(true, new object?[] { true, (byte)2 })]
    [TestCase(true, new object?[] { true, (long)2 })]
    [TestCase(false, new object?[] { 2, false })]
    [TestCase(false, new object?[] { 2.0, false })]
    [TestCase(false, new object?[] { 2.0f, false })]
    [TestCase(false, new object?[] { (byte)2, false })]
    [TestCase(false, new object?[] { (char)2, false })]
    [TestCase(false, new object?[] { (uint)2, false })]
    [TestCase(false, new object?[] { (ushort)2, false })]
    [TestCase(false, new object?[] { (ulong)2, false })]
    [TestCase(false, new object?[] { (byte)2, false })]
    [TestCase(false, new object?[] { (long)2, false })]
    [TestCase(false, new object?[] { false, 2 })]
    [TestCase(false, new object?[] { false, 2.0 })]
    [TestCase(false, new object?[] { false, 2.0f })]
    [TestCase(false, new object?[] { false, (byte)2 })]
    [TestCase(false, new object?[] { false, (char)2 })]
    [TestCase(false, new object?[] { false, (uint)2 })]
    [TestCase(false, new object?[] { false, (ushort)2 })]
    [TestCase(false, new object?[] { false, (ulong)2 })]
    [TestCase(false, new object?[] { false, (byte)2 })]
    [TestCase(false, new object?[] { false, (long)2 })]
    [TestCase(BooleanEnum.True, new object?[] { 2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { 2.0, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { 2.0f, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (byte)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (char)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (uint)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (ushort)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (ulong)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (byte)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { (long)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, 2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, 2.0 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, 2.0f })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (byte)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (char)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (uint)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (ushort)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (ulong)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (byte)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, (long)2 })]
    [TestCase(BooleanEnum.False, new object?[] { 2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { 2.0, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { 2.0f, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (byte)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (char)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (uint)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (ushort)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (ulong)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (byte)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { (long)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, 2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, 2.0 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, 2.0f })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (byte)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (char)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (uint)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (ushort)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (ulong)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (byte)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, (long)2 })]
    public void DoesNotEqualAny_WhenProvidingPartialMatchingObjects_ReturnsTrue(
        object? root,
        object?[] objects)
    {
        root.DoesNotEqualAny(objects).Should().BeTrue();
    }

    [TestCase(null, new object?[] { 1, 2 })]
    [TestCase(null, new object?[] { 2, 1 })]
    [TestCase(1, new object?[] { 2, 3 })]
    [TestCase(1, new object?[] { 2.0, 3 })]
    [TestCase(1, new object?[] { 2.0f, 3 })]
    [TestCase(1, new object?[] { (byte)2, 3 })]
    [TestCase(1, new object?[] { (char)2, 3 })]
    [TestCase(1, new object?[] { (uint)2, 3 })]
    [TestCase(1, new object?[] { (ushort)2, 3 })]
    [TestCase(1, new object?[] { (ulong)2, 3 })]
    [TestCase(1, new object?[] { (short)2, 3 })]
    [TestCase(1, new object?[] { (long)2, 3 })]
    [TestCase(1, new object?[] { 3, 2 })]
    [TestCase(1, new object?[] { 3, 2.0 })]
    [TestCase(1, new object?[] { 3, 2.0f })]
    [TestCase(1, new object?[] { 3, (byte)2 })]
    [TestCase(1, new object?[] { 3, (char)2 })]
    [TestCase(1, new object?[] { 3, (uint)2 })]
    [TestCase(1, new object?[] { 3, (ushort)2 })]
    [TestCase(1, new object?[] { 3, (ulong)2 })]
    [TestCase(1, new object?[] { 3, (short)2 })]
    [TestCase(1, new object?[] { 3, (long)2 })]
    [TestCase(1.0, new object?[] { 2, 3.0 })]
    [TestCase(1.0, new object?[] { 2.0, 3.0 })]
    [TestCase(1.0, new object?[] { 2.0f, 3.0 })]
    [TestCase(1.0, new object?[] { (byte)2, 3.0 })]
    [TestCase(1.0, new object?[] { (char)2, 3.0 })]
    [TestCase(1.0, new object?[] { (uint)2, 3.0 })]
    [TestCase(1.0, new object?[] { (ushort)2, 3.0 })]
    [TestCase(1.0, new object?[] { (ulong)2, 3.0 })]
    [TestCase(1.0, new object?[] { (short)2, 3.0 })]
    [TestCase(1.0, new object?[] { (long)2, 3.0 })]
    [TestCase(1.0, new object?[] { 3.0, 2 })]
    [TestCase(1.0, new object?[] { 3.0, 2.0 })]
    [TestCase(1.0, new object?[] { 3.0, 2.0f })]
    [TestCase(1.0, new object?[] { 3.0, (byte)2 })]
    [TestCase(1.0, new object?[] { 3.0, (char)2 })]
    [TestCase(1.0, new object?[] { 3.0, (uint)2 })]
    [TestCase(1.0, new object?[] { 3.0, (ushort)2 })]
    [TestCase(1.0, new object?[] { 3.0, (ulong)2 })]
    [TestCase(1.0, new object?[] { 3.0, (short)2 })]
    [TestCase(1.0, new object?[] { 3.0, (long)2 })]
    [TestCase(1.0f, new object?[] { 2, 3.0f })]
    [TestCase(1.0f, new object?[] { 2.0, 3.0f })]
    [TestCase(1.0f, new object?[] { 2.0f, 3.0f })]
    [TestCase(1.0f, new object?[] { (byte)2, 3.0f })]
    [TestCase(1.0f, new object?[] { (char)2, 3.0f })]
    [TestCase(1.0f, new object?[] { (uint)2, 3.0f })]
    [TestCase(1.0f, new object?[] { (ushort)2, 3.0f })]
    [TestCase(1.0f, new object?[] { (ulong)2, 3.0f })]
    [TestCase(1.0f, new object?[] { (short)2, 3.0f })]
    [TestCase(1.0f, new object?[] { (long)2, 3.0f })]
    [TestCase(1.0f, new object?[] { 3.0f, 2 })]
    [TestCase(1.0f, new object?[] { 3.0f, 2.0 })]
    [TestCase(1.0f, new object?[] { 3.0f, 2.0f })]
    [TestCase(1.0f, new object?[] { 3.0f, (byte)2 })]
    [TestCase(1.0f, new object?[] { 3.0f, (char)2 })]
    [TestCase(1.0f, new object?[] { 3.0f, (uint)2 })]
    [TestCase(1.0f, new object?[] { 3.0f, (ushort)2 })]
    [TestCase(1.0f, new object?[] { 3.0f, (ulong)2 })]
    [TestCase(1.0f, new object?[] { 3.0f, (short)2 })]
    [TestCase(1.0f, new object?[] { 3.0f, (long)2 })]
    [TestCase((long)1, new object?[] { 2, (long)3 })]
    [TestCase((long)1, new object?[] { 2.0, (long)3 })]
    [TestCase((long)1, new object?[] { 2.0f, (long)3 })]
    [TestCase((long)1, new object?[] { (byte)2, (long)3 })]
    [TestCase((long)1, new object?[] { (char)2, (long)3 })]
    [TestCase((long)1, new object?[] { (uint)2, (long)3 })]
    [TestCase((long)1, new object?[] { (ushort)2, (long)3 })]
    [TestCase((long)1, new object?[] { (ulong)2, (long)3 })]
    [TestCase((long)1, new object?[] { (short)2, (long)3 })]
    [TestCase((long)1, new object?[] { (long)2, (long)3 })]
    [TestCase((long)1, new object?[] { (long)3, 2 })]
    [TestCase((long)1, new object?[] { (long)3, 2.0 })]
    [TestCase((long)1, new object?[] { (long)3, 2.0f })]
    [TestCase((long)1, new object?[] { (long)3, (byte)2 })]
    [TestCase((long)1, new object?[] { (long)3, (char)2 })]
    [TestCase((long)1, new object?[] { (long)3, (uint)2 })]
    [TestCase((long)1, new object?[] { (long)3, (ushort)2 })]
    [TestCase((long)1, new object?[] { (long)3, (ulong)2 })]
    [TestCase((long)1, new object?[] { (long)3, (short)2 })]
    [TestCase((long)1, new object?[] { (long)3, (long)2 })]
    [TestCase((short)1, new object?[] { 2, (short)3 })]
    [TestCase((short)1, new object?[] { 2.0, (short)3 })]
    [TestCase((short)1, new object?[] { 2.0f, (short)3 })]
    [TestCase((short)1, new object?[] { (byte)2, (short)3 })]
    [TestCase((short)1, new object?[] { (char)2, (short)3 })]
    [TestCase((short)1, new object?[] { (uint)2, (short)3 })]
    [TestCase((short)1, new object?[] { (ushort)2, (short)3 })]
    [TestCase((short)1, new object?[] { (ulong)2, (short)3 })]
    [TestCase((short)1, new object?[] { (short)2, (short)3 })]
    [TestCase((short)1, new object?[] { (long)2, (short)3 })]
    [TestCase((short)1, new object?[] { (short)3, 2 })]
    [TestCase((short)1, new object?[] { (short)3, 2.0 })]
    [TestCase((short)1, new object?[] { (short)3, 2.0f })]
    [TestCase((short)1, new object?[] { (short)3, (byte)2 })]
    [TestCase((short)1, new object?[] { (short)3, (char)2 })]
    [TestCase((short)1, new object?[] { (short)3, (uint)2 })]
    [TestCase((short)1, new object?[] { (short)3, (ushort)2 })]
    [TestCase((short)1, new object?[] { (short)3, (ulong)2 })]
    [TestCase((short)1, new object?[] { (short)3, (short)2 })]
    [TestCase((short)1, new object?[] { (short)3, (long)2 })]
    [TestCase((byte)1, new object?[] { 2, (byte)3 })]
    [TestCase((byte)1, new object?[] { 2.0, (byte)3 })]
    [TestCase((byte)1, new object?[] { 2.0f, (byte)3 })]
    [TestCase((byte)1, new object?[] { (byte)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (char)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (uint)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (ushort)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (ulong)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (byte)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (long)2, (byte)3 })]
    [TestCase((byte)1, new object?[] { (byte)3, 2 })]
    [TestCase((byte)1, new object?[] { (byte)3, 2.0 })]
    [TestCase((byte)1, new object?[] { (byte)3, 2.0f })]
    [TestCase((byte)1, new object?[] { (byte)3, (byte)2 })]
    [TestCase((byte)1, new object?[] { (byte)3, (char)2 })]
    [TestCase((byte)1, new object?[] { (byte)3, (uint)2 })]
    [TestCase((byte)1, new object?[] { (byte)3, (ushort)2 })]
    [TestCase((byte)1, new object?[] { (byte)3, (ulong)2 })]
    [TestCase((byte)1, new object?[] { (byte)3, (byte)2 })]
    [TestCase((byte)1, new object?[] { (byte)3, (long)2 })]
    [TestCase((char)1, new object?[] { 2, (char)3 })]
    [TestCase((char)1, new object?[] { 2.0, (char)3 })]
    [TestCase((char)1, new object?[] { 2.0f, (char)3 })]
    [TestCase((char)1, new object?[] { (char)2, (char)3 })]
    [TestCase((char)1, new object?[] { (char)2, (char)3 })]
    [TestCase((char)1, new object?[] { (uint)2, (char)3 })]
    [TestCase((char)1, new object?[] { (ushort)2, (char)3 })]
    [TestCase((char)1, new object?[] { (ulong)2, (char)3 })]
    [TestCase((char)1, new object?[] { (char)2, (char)3 })]
    [TestCase((char)1, new object?[] { (long)2, (char)3 })]
    [TestCase((char)1, new object?[] { (char)3, 2 })]
    [TestCase((char)1, new object?[] { (char)3, 2.0 })]
    [TestCase((char)1, new object?[] { (char)3, 2.0f })]
    [TestCase((char)1, new object?[] { (char)3, (char)2 })]
    [TestCase((char)1, new object?[] { (char)3, (char)2 })]
    [TestCase((char)1, new object?[] { (char)3, (uint)2 })]
    [TestCase((char)1, new object?[] { (char)3, (ushort)2 })]
    [TestCase((char)1, new object?[] { (char)3, (ulong)2 })]
    [TestCase((char)1, new object?[] { (char)3, (char)2 })]
    [TestCase((char)1, new object?[] { (char)3, (long)2 })]
    [TestCase((ulong)1, new object?[] { 2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { 2.0, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { 2.0f, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (char)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (uint)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (ushort)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (ulong)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (long)2, (ulong)3 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, 2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, 2.0 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, 2.0f })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (char)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (uint)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (ushort)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (ulong)2 })]
    [TestCase((ulong)1, new object?[] { (ulong)3, (long)2 })]
    [TestCase((uint)1, new object?[] { 2, (uint)3 })]
    [TestCase((uint)1, new object?[] { 2.0, (uint)3 })]
    [TestCase((uint)1, new object?[] { 2.0f, (uint)3 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (char)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (ushort)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (ulong)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (uint)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (long)2, (uint)3 })]
    [TestCase((uint)1, new object?[] { (uint)3, 2 })]
    [TestCase((uint)1, new object?[] { (uint)3, 2.0 })]
    [TestCase((uint)1, new object?[] { (uint)3, 2.0f })]
    [TestCase((uint)1, new object?[] { (uint)3, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)3, (char)2 })]
    [TestCase((uint)1, new object?[] { (uint)3, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)3, (ushort)2 })]
    [TestCase((uint)1, new object?[] { (uint)3, (ulong)2 })]
    [TestCase((uint)1, new object?[] { (uint)3, (uint)2 })]
    [TestCase((uint)1, new object?[] { (uint)3, (long)2 })]
    [TestCase((ushort)1, new object?[] { 2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { 2.0, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { 2.0f, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (char)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (uint)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (ulong)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (ushort)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (long)2, (ushort)3 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, 2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, 2.0 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, 2.0f })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (char)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (uint)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (ulong)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (ushort)2 })]
    [TestCase((ushort)1, new object?[] { (ushort)3, (long)2 })]
    [TestCase(true, new object?[] { 2, false })]
    [TestCase(true, new object?[] { 2.0, false })]
    [TestCase(true, new object?[] { 2.0f, false })]
    [TestCase(true, new object?[] { (byte)2, false })]
    [TestCase(true, new object?[] { (char)2, false })]
    [TestCase(true, new object?[] { (uint)2, false })]
    [TestCase(true, new object?[] { (ushort)2, false })]
    [TestCase(true, new object?[] { (ulong)2, false })]
    [TestCase(true, new object?[] { (byte)2, false })]
    [TestCase(true, new object?[] { (long)2, false })]
    [TestCase(true, new object?[] { false, 2 })]
    [TestCase(true, new object?[] { false, 2.0 })]
    [TestCase(true, new object?[] { false, 2.0f })]
    [TestCase(true, new object?[] { false, (byte)2 })]
    [TestCase(true, new object?[] { false, (char)2 })]
    [TestCase(true, new object?[] { false, (uint)2 })]
    [TestCase(true, new object?[] { false, (ushort)2 })]
    [TestCase(true, new object?[] { false, (ulong)2 })]
    [TestCase(true, new object?[] { false, (byte)2 })]
    [TestCase(true, new object?[] { false, (long)2 })]
    [TestCase(false, new object?[] { 2, true })]
    [TestCase(false, new object?[] { 2.0, true })]
    [TestCase(false, new object?[] { 2.0f, true })]
    [TestCase(false, new object?[] { (byte)2, true })]
    [TestCase(false, new object?[] { (char)2, true })]
    [TestCase(false, new object?[] { (uint)2, true })]
    [TestCase(false, new object?[] { (ushort)2, true })]
    [TestCase(false, new object?[] { (ulong)2, true })]
    [TestCase(false, new object?[] { (byte)2, true })]
    [TestCase(false, new object?[] { (long)2, true })]
    [TestCase(false, new object?[] { true, 2 })]
    [TestCase(false, new object?[] { true, 2.0 })]
    [TestCase(false, new object?[] { true, 2.0f })]
    [TestCase(false, new object?[] { true, (byte)2 })]
    [TestCase(false, new object?[] { true, (char)2 })]
    [TestCase(false, new object?[] { true, (uint)2 })]
    [TestCase(false, new object?[] { true, (ushort)2 })]
    [TestCase(false, new object?[] { true, (ulong)2 })]
    [TestCase(false, new object?[] { true, (byte)2 })]
    [TestCase(false, new object?[] { true, (long)2 })]
    [TestCase(BooleanEnum.True, new object?[] { 2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { 2.0, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { 2.0f, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (byte)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (char)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (uint)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (ushort)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (ulong)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (byte)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { (long)2, BooleanEnum.False })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, 2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, 2.0 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, 2.0f })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (byte)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (char)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (uint)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (ushort)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (ulong)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (byte)2 })]
    [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.False, (long)2 })]
    [TestCase(BooleanEnum.False, new object?[] { 2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { 2.0, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { 2.0f, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (byte)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (char)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (uint)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (ushort)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (ulong)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (byte)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { (long)2, BooleanEnum.True })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, 2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, 2.0 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, 2.0f })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (byte)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (char)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (uint)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (ushort)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (ulong)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (byte)2 })]
    [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.True, (long)2 })]
    public void DoesNotEqualAll_WhenProvidingNonMatchingObjects_ReturnsTrue(
        object? root,
        object?[] objects)
    {
        root.DoesNotEqualAll(objects).Should().BeTrue();
    }

    [TestCase(null)]
    public void ConsoleWriteLine_WhenWritingFromANullObject_LogsAnEmptyStringToTheConsole(object? root)
    {
        AssertConsoleWriteLine(root, string.Empty);
    }

    [TestCase(null)]
    public void ConsoleWrite_WhenWritingFromANullObject_LogsAnEmptyStringToTheConsole(object? root)
    {
        AssertConsoleWrite(root, string.Empty);
    }

    [TestCase(null, null)]
    [TestCase(null, "-")]
    [TestCase(null, "++")]
    [TestCase(null, "___")]
    public void ConsoleWriteHeading_WhenWritingFromANullObject_LogsAnEmptyStringToTheConsole(
        object? root,
        string? headingUnderline)
    {
        AssertConsoleWriteHeading(root, string.Empty, headingUnderline);
    }

    [TestCase(null, 0)]
    [TestCase(null, 1)]
    [TestCase(null, 2)]
    [TestCase(null, 3)]
    [TestCase(null, 4)]
    public void ConsoleWriteLines_WhenWritingFromANullObject_LogsEmptyLinesToTheConsole(
        object? root,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLines(root, numberOfLineAppends);
    }

    [TestCase(null, null, 0)]
    [TestCase(null, null, 1)]
    [TestCase(null, null, 2)]
    [TestCase(null, null, 3)]
    [TestCase(null, null, 4)]
    [TestCase(null, "-", 0)]
    [TestCase(null, "-", 1)]
    [TestCase(null, "-", 2)]
    [TestCase(null, "-", 3)]
    [TestCase(null, "-", 4)]
    [TestCase(null, "++", 0)]
    [TestCase(null, "++", 1)]
    [TestCase(null, "++", 2)]
    [TestCase(null, "++", 3)]
    [TestCase(null, "++", 4)]
    [TestCase(null, "___", 0)]
    [TestCase(null, "___", 1)]
    [TestCase(null, "___", 2)]
    [TestCase(null, "___", 3)]
    [TestCase(null, "___", 4)]
    public void ConsoleWriteHeading_WhenWritingFromANullObject_LogsEmptyLinesToTheConsole(
        object? root,
        string? headingUnderline,
        int numberOfLineAppends)
    {
        AssertConsoleWriteHeadingWithLineAppends(root, string.Empty, headingUnderline, numberOfLineAppends);
    }

    [TestCase("Test a string", "Test a string")]
    [TestCase(1, "1")]
    [TestCase(true, "True")]
    [TestCase(false, "False")]
    public void ConsoleWriteLine_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(object root, string expectedValue)
    {
        AssertConsoleWriteLine(root, expectedValue);
    }

    [TestCase("Test a string", "Test a string", 0)]
    [TestCase(1, "1", 1)]
    [TestCase(true, "True", 2)]
    [TestCase(false, "False", 3)]
    public void ConsoleWriteLine_WhenWritingFromANonNullObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
        object root,
        string expectedValue,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithLineAppends(root, expectedValue, numberOfLineAppends);
    }

    [TestCase("Test a string", "Test a string", null)]
    [TestCase(1, "1", null)]
    [TestCase(true, "True", null)]
    [TestCase(false, "False", null)]
    [TestCase("Test a string", "Test a string", "-")]
    [TestCase(1, "1", "-")]
    [TestCase(true, "True", "-")]
    [TestCase(false, "False", "-")]
    [TestCase("Test a string", "Test a string", "++")]
    [TestCase(1, "1", "++")]
    [TestCase(true, "True", "++")]
    [TestCase(false, "False", "++")]
    [TestCase("Test a string", "Test a string", "___")]
    [TestCase(1, "1", "___")]
    [TestCase(true, "True", "___")]
    [TestCase(false, "False", "___")]
    public void ConsoleWriteHeading_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(
        object root,
        string expectedValue,
        string? headingUnderline)
    {
        AssertConsoleWriteHeading(root, expectedValue, headingUnderline);
    }

    [TestCase("Test a string", "Test a string", null, 0)]
    [TestCase("Test a string", "Test a string", null, 1)]
    [TestCase("Test a string", "Test a string", null, 2)]
    [TestCase(1, "1", null, 0)]
    [TestCase(1, "1", null, 1)]
    [TestCase(1, "1", null, 2)]
    [TestCase(true, "True", null, 0)]
    [TestCase(true, "True", null, 1)]
    [TestCase(true, "True", null, 2)]
    [TestCase(false, "False", null, 0)]
    [TestCase(false, "False", null, 1)]
    [TestCase(false, "False", null, 2)]
    [TestCase("Test a string", "Test a string", "-", 0)]
    [TestCase("Test a string", "Test a string", "-", 1)]
    [TestCase("Test a string", "Test a string", "-", 2)]
    [TestCase(1, "1", "-", 0)]
    [TestCase(1, "1", "-", 1)]
    [TestCase(1, "1", "-", 2)]
    [TestCase(true, "True", "-", 0)]
    [TestCase(true, "True", "-", 1)]
    [TestCase(true, "True", "-", 2)]
    [TestCase(false, "False", "-", 0)]
    [TestCase(false, "False", "-", 1)]
    [TestCase(false, "False", "-", 2)]
    [TestCase("Test a string", "Test a string", "++", 0)]
    [TestCase("Test a string", "Test a string", "++", 1)]
    [TestCase("Test a string", "Test a string", "++", 2)]
    [TestCase(1, "1", "++", 0)]
    [TestCase(1, "1", "++", 1)]
    [TestCase(1, "1", "++", 2)]
    [TestCase(true, "True", "++", 0)]
    [TestCase(true, "True", "++", 1)]
    [TestCase(true, "True", "++", 2)]
    [TestCase(false, "False", "++", 0)]
    [TestCase(false, "False", "++", 1)]
    [TestCase(false, "False", "++", 2)]
    [TestCase("Test a string", "Test a string", "___", 0)]
    [TestCase("Test a string", "Test a string", "___", 1)]
    [TestCase("Test a string", "Test a string", "___", 2)]
    [TestCase(1, "1", "___", 0)]
    [TestCase(1, "1", "___", 1)]
    [TestCase(1, "1", "___", 2)]
    [TestCase(true, "True", "___", 0)]
    [TestCase(true, "True", "___", 1)]
    [TestCase(true, "True", "___", 2)]
    [TestCase(false, "False", "___", 0)]
    [TestCase(false, "False", "___", 1)]
    [TestCase(false, "False", "___", 2)]
    public void ConsoleWriteHeading_WhenWritingFromANonNullObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
        object root,
        string expectedValue,
        string? headingUnderline,
        int numberOfLineAppends)
    {
        AssertConsoleWriteHeadingWithLineAppends(root, expectedValue, headingUnderline, numberOfLineAppends);
    }

    [TestCase("Test a string", "Test a string")]
    [TestCase(1, "1")]
    [TestCase(true, "True")]
    [TestCase(false, "False")]
    public void ConsoleWrite_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(object root, string expectedValue)
    {
        AssertConsoleWrite(root, expectedValue);
    }

    [TestCase("Test a string", 0)]
    [TestCase(1, 1)]
    [TestCase(1.0, 2)]
    [TestCase(1.0f, 3)]
    [TestCase(true, 4)]
    [TestCase(false, 5)]
    public void ConsoleWriteLines_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(object root, int numberOfWriteLines)
    {
        AssertConsoleWriteLines(root, numberOfWriteLines);
    }

    [TestCase("Name X")]
    [TestCase("X Name")]
    [TestCase("Tenjin")]
    public void ConsoleWriteLine_WhenWritingFromACustomObject_LogsTheExpectedValueToTheConsole(string name)
    {
        var customObject = new ConsoleObject(name);
        var expectedValue = ConsoleObject.GetOutputText(name);

        AssertConsoleWriteLine(customObject, expectedValue);
    }

    [TestCase("Name X", 0)]
    [TestCase("X Name", 1)]
    [TestCase("Tenjin", 2)]
    public void ConsoleWriteLine_WhenWritingFromACustomObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
        string name,
        int numberOfLineAppends)
    {
        var customObject = new ConsoleObject(name);
        var expectedValue = ConsoleObject.GetOutputText(name);

        AssertConsoleWriteLineWithLineAppends(customObject, expectedValue, numberOfLineAppends);
    }

    [TestCase("Name X", null)]
    [TestCase("X Name", null)]
    [TestCase("Tenjin", null)]
    [TestCase("Name X", "-")]
    [TestCase("X Name", "-")]
    [TestCase("Tenjin", "-")]
    [TestCase("Name X", "++")]
    [TestCase("X Name", "++")]
    [TestCase("Tenjin", "++")]
    [TestCase("Name X", "___")]
    [TestCase("X Name", "___")]
    [TestCase("Tenjin", "___")]
    public void ConsoleWriteHeading_WhenWritingFromACustomObject_LogsTheExpectedValueToTheConsole(string name, string? headingUnderline)
    {
        var customObject = new ConsoleObject(name);
        var expectedValue = ConsoleObject.GetOutputText(name);

        AssertConsoleWriteHeading(customObject, expectedValue, headingUnderline);
    }

    [TestCase("Name X", 0, null)]
    [TestCase("X Name", 1, null)]
    [TestCase("Tenjin", 2, null)]
    [TestCase("Name X", 0, "-")]
    [TestCase("X Name", 1, "-")]
    [TestCase("Tenjin", 2, "-")]
    [TestCase("Name X", 0, "==")]
    [TestCase("X Name", 1, "==")]
    [TestCase("Tenjin", 2, "==")]
    [TestCase("Name X", 0, "___")]
    [TestCase("X Name", 1, "___")]
    [TestCase("Tenjin", 2, "___")]
    public void ConsoleWriteHeading_WhenWritingFromACustomObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
        string name,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var customObject = new ConsoleObject(name);
        var expectedValue = ConsoleObject.GetOutputText(name);

        AssertConsoleWriteHeadingWithLineAppends(customObject, expectedValue, headingUnderline, numberOfLineAppends);
    }

    [TestCase("Name X")]
    [TestCase("X Name")]
    [TestCase("Tenjin")]
    public void ConsoleWrite_WhenWritingFromACustomObject_LogsTheExpectedValueToTheConsole(string name)
    {
        var customObject = new ConsoleObject(name);
        var expectedValue = ConsoleObject.GetOutputText(name);

        AssertConsoleWrite(customObject, expectedValue);
    }

    [Test]
    public void ConsoleWriteLine_WhenWritingNullableFormattableObjects_LogsAnEmptyString()
    {
        AssertConsoleWriteLineWithFormat(null, "bogus-format-string", null, string.Empty);
    }

    [TestCase(null)]
    [TestCase("-")]
    [TestCase("==")]
    [TestCase("___")]
    public void ConsoleWriteHeading_WhenWritingNullableFormattableObjects_LogsAnEmptyString(string? headerUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(null, null, null, string.Empty, headerUnderline);
    }

    [Test]
    public void ConsoleWrite_WhenWritingNullableFormattableObjects_LogsAnEmptyString()
    {
        AssertConsoleWriteWithFormat(null, "bogus-format-string", null, string.Empty);
    }

    [TestCase("2000-01-01")]
    [TestCase("2005-01-05")]
    [TestCase("2005-02-10")]
    [TestCase("2010-10-10")]
    [TestCase("2020-12-31")]
    public void ConsoleWriteLine_WhenWritingDateTimeWithFormatString_LogsTheExpectedValueToTheConsole(string value)
    {
        var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
        var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

        AssertConsoleWriteLineWithFormat(dateTime, DateTimeWriteLineOutputFormat, null, expectedValue);
    }

    [TestCase("2000-01-01", 0)]
    [TestCase("2005-01-05", 1)]
    [TestCase("2005-02-10", 2)]
    [TestCase("2010-10-10", 3)]
    [TestCase("2020-12-31", 4)]
    public void ConsoleWriteLine_WhenWritingDateTimeWithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        string value,
        int numberOfLineAppends)
    {
        var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
        var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            dateTime, numberOfLineAppends, DateTimeWriteLineOutputFormat, null, expectedValue);
    }

    [TestCase("2000-01-01", null)]
    [TestCase("2005-01-05", null)]
    [TestCase("2005-02-10", null)]
    [TestCase("2010-10-10", null)]
    [TestCase("2020-12-31", null)]
    [TestCase("2000-01-01", "-")]
    [TestCase("2005-01-05", "-")]
    [TestCase("2005-02-10", "-")]
    [TestCase("2010-10-10", "-")]
    [TestCase("2020-12-31", "-")]
    [TestCase("2000-01-01", "==")]
    [TestCase("2005-01-05", "==")]
    [TestCase("2005-02-10", "==")]
    [TestCase("2010-10-10", "==")]
    [TestCase("2020-12-31", "==")]
    [TestCase("2000-01-01", "___")]
    [TestCase("2005-01-05", "___")]
    [TestCase("2005-02-10", "___")]
    [TestCase("2010-10-10", "___")]
    [TestCase("2020-12-31", "___")]
    public void ConsoleWriteHeading_WhenWritingDateTimeWithFormatString_LogsTheExpectedValueToTheConsole(string value, string? headingUnderline)
    {
        var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
        var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

        AssertConsoleWriteHeadingWithFormat(dateTime, DateTimeWriteLineOutputFormat, null, expectedValue, headingUnderline);
    }

    [TestCase("2000-01-01", 0, null)]
    [TestCase("2005-01-05", 1, null)]
    [TestCase("2005-02-10", 2, null)]
    [TestCase("2010-10-10", 3, null)]
    [TestCase("2020-12-31", 4, null)]
    [TestCase("2000-01-01", 0, "+")]
    [TestCase("2005-01-05", 1, "+")]
    [TestCase("2005-02-10", 2, "+")]
    [TestCase("2010-10-10", 3, "+")]
    [TestCase("2020-12-31", 4, "+")]
    [TestCase("2000-01-01", 0, "--")]
    [TestCase("2005-01-05", 1, "--")]
    [TestCase("2005-02-10", 2, "--")]
    [TestCase("2010-10-10", 3, "--")]
    [TestCase("2020-12-31", 4, "--")]
    [TestCase("2000-01-01", 0, "___")]
    [TestCase("2005-01-05", 1, "___")]
    [TestCase("2005-02-10", 2, "___")]
    [TestCase("2010-10-10", 3, "___")]
    [TestCase("2020-12-31", 4, "___")]
    public void ConsoleWriteHeading_WhenWritingDateTimeWithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        string value,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
        var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            dateTime, numberOfLineAppends, DateTimeWriteLineOutputFormat, null, expectedValue, headingUnderline);
    }

    [TestCase("2000-01-01")]
    [TestCase("2005-01-05")]
    [TestCase("2005-02-10")]
    [TestCase("2010-10-10")]
    [TestCase("2020-12-31")]
    public void ConsoleWrite_WhenWritingDateTimeWithFormatString_LogsTheExpectedValueToTheConsole(string value)
    {
        var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
        var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

        AssertConsoleWriteWithFormat(dateTime, DateTimeWriteLineOutputFormat, null, expectedValue);
    }

    [TestCase((ushort)255, "X", "FF")]
    [TestCase((ushort)255, "x", "ff")]
    public void ConsoleWriteLine_WhenWritingUInt16WithFormatString_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteLineWithFormat(value, format, null, expectedResult);
    }

    [TestCase((ushort)100, "C", "en-ZA", "R100,00")]
    [TestCase((ushort)100, "F", "en-US", "100.000")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %")]
    public void ConsoleWriteLine_WhenWritingUInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase((uint)255, "X", "FF")]
    [TestCase((uint)255, "x", "ff")]
    public void ConsoleWriteLine_WhenWritingUInt32WithFormatString_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteLineWithFormat(value, format, null, expectedResult);
    }

    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %")]
    public void ConsoleWriteLine_WhenWritingUInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase((ulong)255, "X", "FF")]
    [TestCase((ulong)255, "x", "ff")]
    public void ConsoleWriteLine_WhenWritingUInt64WithFormatString_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteLineWithFormat(value, format, null, expectedResult);
    }

    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %")]
    public void ConsoleWriteLine_WhenWritingUInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF")]
    [TestCase(255, "x", "ff")]
    public void ConsoleWriteLine_WhenWritingInt16WithFormatString_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteLineWithFormat(value, format, null, expectedResult);
    }

    [TestCase(100, "C", "en-ZA", "R100,00")]
    [TestCase(100, "F", "en-US", "100.000")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %")]
    public void ConsoleWriteLine_WhenWritingInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF")]
    [TestCase(255, "x", "ff")]
    public void ConsoleWriteLine_WhenWritingInt32WithFormatString_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteLineWithFormat(value, format, null, expectedResult);
    }

    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00")]
    [TestCase(1000000, "F", "en-US", "1000000.000")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %")]
    public void ConsoleWriteLine_WhenWritingInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF")]
    [TestCase(255, "x", "ff")]
    public void ConsoleWriteLine_WhenWritingInt64WithFormatString_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteLineWithFormat(value, format, null, expectedResult);
    }

    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00")]
    [TestCase(100000000, "F", "en-US", "100000000.000")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %")]
    public void ConsoleWriteLine_WhenWritingInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %")]
    public void ConsoleWriteLine_WhenWritingFloatWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        float value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %")]
    public void ConsoleWriteLine_WhenWritingDoubleWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        double value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase((ushort)255, "X", "FF", 0)]
    [TestCase((ushort)255, "x", "ff", 1)]
    public void ConsoleWriteLine_WhenWritingUInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string expectedResult,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
    }

    [TestCase((ushort)100, "C", "en-ZA", "R100,00", 0)]
    [TestCase((ushort)100, "F", "en-US", "100.000", 1)]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", 2)]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingUInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase((uint)255, "X", "FF", 0)]
    [TestCase((uint)255, "x", "ff", 1)]
    public void ConsoleWriteLine_WhenWritingUInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string expectedResult,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
    }

    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", 0)]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", 1)]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", 2)]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingUInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase((ulong)255, "X", "FF", 0)]
    [TestCase((ulong)255, "x", "ff", 1)]
    public void ConsoleWriteLine_WhenWritingUInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string expectedResult,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
    }

    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", 0)]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", 1)]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", 2)]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingUInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF", 0)]
    [TestCase(255, "x", "ff", 1)]
    public void ConsoleWriteLine_WhenWritingInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string expectedResult,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
    }

    [TestCase(100, "C", "en-ZA", "R100,00", 0)]
    [TestCase(100, "F", "en-US", "100.000", 1)]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", 2)]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF", 0)]
    [TestCase(255, "x", "ff", 1)]
    public void ConsoleWriteLine_WhenWritingInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string expectedResult,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
    }

    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", 0)]
    [TestCase(1000000, "F", "en-US", "1000000.000", 1)]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", 2)]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF", 0)]
    [TestCase(255, "x", "ff", 1)]
    public void ConsoleWriteLine_WhenWritingInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string expectedResult,
        int numberOfLineAppends)
    {
        AssertConsoleWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
    }

    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", 0)]
    [TestCase(100000000, "F", "en-US", "100000000.000", 1)]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", 2)]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", 0)]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", 1)]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", 2)]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", 3)]
    public void ConsoleWriteLine_WhenWritingFloatWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        float value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", 0)]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", 1)]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", 2)]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", 3)]
    public void ConsoleWriteLine_WhenWritingDoubleWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        double value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteLineWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult);
    }

    [TestCase((ushort)255, "X", "FF", null)]
    [TestCase((ushort)255, "x", "ff", null)]
    [TestCase((ushort)255, "X", "FF", "-")]
    [TestCase((ushort)255, "x", "ff", "-")]
    [TestCase((ushort)255, "X", "FF", "++")]
    [TestCase((ushort)255, "x", "ff", "++")]
    [TestCase((ushort)255, "X", "FF", "___")]
    [TestCase((ushort)255, "x", "ff", "___")]
    public void ConsoleWriteHeading_WhenWritingUInt16WithFormatString_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string expectedResult,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
    }

    [TestCase((ushort)100, "C", "en-ZA", "R100,00", null)]
    [TestCase((ushort)100, "F", "en-US", "100.000", null)]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", null)]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", null)]
    [TestCase((ushort)100, "C", "en-ZA", "R100,00", "-")]
    [TestCase((ushort)100, "F", "en-US", "100.000", "-")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", "-")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", "-")]
    [TestCase((ushort)100, "C", "en-ZA", "R100,00", "++")]
    [TestCase((ushort)100, "F", "en-US", "100.000", "++")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", "++")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", "++")]
    [TestCase((ushort)100, "C", "en-ZA", "R100,00", "___")]
    [TestCase((ushort)100, "F", "en-US", "100.000", "___")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", "___")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingUInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase((uint)255, "X", "FF", null)]
    [TestCase((uint)255, "x", "ff", null)]
    [TestCase((uint)255, "X", "FF", "-")]
    [TestCase((uint)255, "x", "ff", "-")]
    [TestCase((uint)255, "X", "FF", "++")]
    [TestCase((uint)255, "x", "ff", "++")]
    [TestCase((uint)255, "X", "FF", "___")]
    [TestCase((uint)255, "x", "ff", "___")]
    public void ConsoleWriteHeading_WhenWritingUInt32WithFormatString_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string expectedResult,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
    }

    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", null)]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", null)]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", null)]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", null)]
    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", "-")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", "-")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", "-")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", "-")]
    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", "++")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", "++")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", "++")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", "++")]
    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", "___")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", "___")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", "___")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingUInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase((ulong)255, "X", "FF", null)]
    [TestCase((ulong)255, "x", "ff", null)]
    [TestCase((ulong)255, "X", "FF", "-")]
    [TestCase((ulong)255, "x", "ff", "-")]
    [TestCase((ulong)255, "X", "FF", "++")]
    [TestCase((ulong)255, "x", "ff", "++")]
    [TestCase((ulong)255, "X", "FF", "___")]
    [TestCase((ulong)255, "x", "ff", "___")]
    public void ConsoleWriteHeading_WhenWritingUInt64WithFormatString_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string expectedResult,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
    }

    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", null)]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", null)]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", null)]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", null)]
    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", "-")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", "-")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", "-")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", "-")]
    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", "++")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", "++")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", "++")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", "++")]
    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", "___")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", "___")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", "___")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingUInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(255, "X", "FF", null)]
    [TestCase(255, "x", "ff", null)]
    [TestCase(255, "X", "FF", "-")]
    [TestCase(255, "x", "ff", "-")]
    [TestCase(255, "X", "FF", "++")]
    [TestCase(255, "x", "ff", "++")]
    [TestCase(255, "X", "FF", "___")]
    [TestCase(255, "x", "ff", "___")]
    public void ConsoleWriteHeading_WhenWritingInt16WithFormatString_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string expectedResult,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
    }

    [TestCase(100, "C", "en-ZA", "R100,00", null)]
    [TestCase(100, "F", "en-US", "100.000", null)]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", null)]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", null)]
    [TestCase(100, "C", "en-ZA", "R100,00", "-")]
    [TestCase(100, "F", "en-US", "100.000", "-")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", "-")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", "-")]
    [TestCase(100, "C", "en-ZA", "R100,00", "++")]
    [TestCase(100, "F", "en-US", "100.000", "++")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", "++")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", "++")]
    [TestCase(100, "C", "en-ZA", "R100,00", "___")]
    [TestCase(100, "F", "en-US", "100.000", "___")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", "___")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(255, "X", "FF", null)]
    [TestCase(255, "x", "ff", null)]
    [TestCase(255, "X", "FF", "-")]
    [TestCase(255, "x", "ff", "-")]
    [TestCase(255, "X", "FF", "++")]
    [TestCase(255, "x", "ff", "++")]
    [TestCase(255, "X", "FF", "___")]
    [TestCase(255, "x", "ff", "___")]
    public void ConsoleWriteHeading_WhenWritingInt32WithFormatString_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string expectedResult,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
    }

    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", null)]
    [TestCase(1000000, "F", "en-US", "1000000.000", null)]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", null)]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", null)]
    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", "-")]
    [TestCase(1000000, "F", "en-US", "1000000.000", "-")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", "-")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", "-")]
    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", "++")]
    [TestCase(1000000, "F", "en-US", "1000000.000", "++")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", "++")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", "++")]
    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", "___")]
    [TestCase(1000000, "F", "en-US", "1000000.000", "___")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", "___")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(255, "X", "FF", null)]
    [TestCase(255, "x", "ff", null)]
    [TestCase(255, "X", "FF", "-")]
    [TestCase(255, "x", "ff", "-")]
    [TestCase(255, "X", "FF", "++")]
    [TestCase(255, "x", "ff", "++")]
    [TestCase(255, "X", "FF", "___")]
    [TestCase(255, "x", "ff", "___")]
    public void ConsoleWriteHeading_WhenWritingInt64WithFormatString_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string expectedResult,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
    }

    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", null)]
    [TestCase(100000000, "F", "en-US", "100000000.000", null)]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", null)]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", null)]
    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", "-")]
    [TestCase(100000000, "F", "en-US", "100000000.000", "-")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", "-")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", "-")]
    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", "++")]
    [TestCase(100000000, "F", "en-US", "100000000.000", "++")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", "++")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", "++")]
    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", "___")]
    [TestCase(100000000, "F", "en-US", "100000000.000", "___")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", "___")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", null)]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", null)]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", null)]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", null)]
    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", "-")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", "-")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", "-")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", "-")]
    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", "++")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", "++")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", "++")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", "++")]
    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", "___")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", "___")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", "___")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", "___")]
    public void ConsoleWriteHeading_WhenWritingFloatWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        float value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", null)]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", null)]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", null)]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", null)]
    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", "-")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", "-")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", "-")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", "-")]
    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", "++")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", "++")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", "++")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", "++")]
    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", "___")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", "___")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", "___")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", "___")]
    public void ConsoleWriteHeading_WhenWritingDoubleWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        double value,
        string? format,
        string culture,
        string expectedResult,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase((ushort)255, "X", "FF", 0, null)]
    [TestCase((ushort)255, "x", "ff", 1, null)]
    [TestCase((ushort)255, "X", "FF", 0, "-")]
    [TestCase((ushort)255, "x", "ff", 1, "-")]
    [TestCase((ushort)255, "X", "FF", 0, "++")]
    [TestCase((ushort)255, "x", "ff", 1, "++")]
    [TestCase((ushort)255, "X", "FF", 0, "___")]
    [TestCase((ushort)255, "x", "ff", 1, "___")]
    public void ConsoleWriteHeading_WhenWritingUInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, null, expectedResult, headingUnderline);
    }

    [TestCase((ushort)100, "C", "en-ZA", "R100,00", 0, null)]
    [TestCase((ushort)100, "F", "en-US", "100.000", 1, null)]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", 2, null)]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", 3, null)]
    [TestCase((ushort)100, "C", "en-ZA", "R100,00", 0, "-")]
    [TestCase((ushort)100, "F", "en-US", "100.000", 1, "-")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", 2, "-")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", 3, "-")]
    [TestCase((ushort)100, "C", "en-ZA", "R100,00", 0, "++")]
    [TestCase((ushort)100, "F", "en-US", "100.000", 1, "++")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", 2, "++")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", 3, "++")]
    [TestCase((ushort)100, "C", "en-ZA", "R100,00", 0, "___")]
    [TestCase((ushort)100, "F", "en-US", "100.000", 1, "___")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", 2, "___")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingUInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase((uint)255, "X", "FF", 0, null)]
    [TestCase((uint)255, "x", "ff", 1, null)]
    [TestCase((uint)255, "X", "FF", 0, "-")]
    [TestCase((uint)255, "x", "ff", 1, "-")]
    [TestCase((uint)255, "X", "FF", 0, "++")]
    [TestCase((uint)255, "x", "ff", 1, "++")]
    [TestCase((uint)255, "X", "FF", 0, "___")]
    [TestCase((uint)255, "x", "ff", 1, "___")]
    public void ConsoleWriteHeading_WhenWritingUInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, null, expectedResult, headingUnderline);
    }

    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", 0, null)]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", 1, null)]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", 2, null)]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", 3, null)]
    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", 0, "-")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", 1, "-")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", 2, "-")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", 3, "-")]
    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", 0, "++")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", 1, "++")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", 2, "++")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", 3, "++")]
    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", 0, "___")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000", 1, "___")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", 2, "___")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingUInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase((ulong)255, "X", "FF", 0, null)]
    [TestCase((ulong)255, "x", "ff", 1, null)]
    [TestCase((ulong)255, "X", "FF", 0, "-")]
    [TestCase((ulong)255, "x", "ff", 1, "-")]
    [TestCase((ulong)255, "X", "FF", 0, "++")]
    [TestCase((ulong)255, "x", "ff", 1, "++")]
    [TestCase((ulong)255, "X", "FF", 0, "___")]
    [TestCase((ulong)255, "x", "ff", 1, "___")]
    public void ConsoleWriteHeading_WhenWritingUInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, null, expectedResult, headingUnderline);
    }

    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", 0, null)]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", 1, null)]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", 2, null)]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, null)]
    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", 0, "-")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", 1, "-")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", 2, "-")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, "-")]
    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", 0, "++")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", 1, "++")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", 2, "++")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, "++")]
    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", 0, "___")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000", 1, "___")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", 2, "___")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingUInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(255, "X", "FF", 0, null)]
    [TestCase(255, "x", "ff", 1, null)]
    [TestCase(255, "X", "FF", 0, "-")]
    [TestCase(255, "x", "ff", 1, "-")]
    [TestCase(255, "X", "FF", 0, "++")]
    [TestCase(255, "x", "ff", 1, "++")]
    [TestCase(255, "X", "FF", 0, "___")]
    [TestCase(255, "x", "ff", 1, "___")]
    public void ConsoleWriteHeading_WhenWritingInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, null, expectedResult, headingUnderline);
    }

    [TestCase(100, "C", "en-ZA", "R100,00", 0, null)]
    [TestCase(100, "F", "en-US", "100.000", 1, null)]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", 2, null)]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", 3, null)]
    [TestCase(100, "C", "en-ZA", "R100,00", 0, "-")]
    [TestCase(100, "F", "en-US", "100.000", 1, "-")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", 2, "-")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", 3, "-")]
    [TestCase(100, "C", "en-ZA", "R100,00", 0, "++")]
    [TestCase(100, "F", "en-US", "100.000", 1, "++")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", 2, "++")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", 3, "++")]
    [TestCase(100, "C", "en-ZA", "R100,00", 0, "___")]
    [TestCase(100, "F", "en-US", "100.000", 1, "___")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002", 2, "___")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(255, "X", "FF", 0, null)]
    [TestCase(255, "x", "ff", 1, null)]
    [TestCase(255, "X", "FF", 0, "-")]
    [TestCase(255, "x", "ff", 1, "-")]
    [TestCase(255, "X", "FF", 0, "++")]
    [TestCase(255, "x", "ff", 1, "++")]
    [TestCase(255, "X", "FF", 0, "___")]
    [TestCase(255, "x", "ff", 1, "___")]
    public void ConsoleWriteHeading_WhenWritingInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, null, expectedResult, headingUnderline);
    }

    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", 0, null)]
    [TestCase(1000000, "F", "en-US", "1000000.000", 1, null)]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", 2, null)]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", 3, null)]
    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", 0, "-")]
    [TestCase(1000000, "F", "en-US", "1000000.000", 1, "-")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", 2, "-")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", 3, "-")]
    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", 0, "++")]
    [TestCase(1000000, "F", "en-US", "1000000.000", 1, "++")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", 2, "++")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", 3, "++")]
    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", 0, "___")]
    [TestCase(1000000, "F", "en-US", "1000000.000", 1, "___")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006", 2, "___")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(255, "X", "FF", 0, null)]
    [TestCase(255, "x", "ff", 1, null)]
    [TestCase(255, "X", "FF", 0, "-")]
    [TestCase(255, "x", "ff", 1, "-")]
    [TestCase(255, "X", "FF", 0, "++")]
    [TestCase(255, "x", "ff", 1, "++")]
    [TestCase(255, "X", "FF", 0, "___")]
    [TestCase(255, "x", "ff", 1, "___")]
    public void ConsoleWriteHeading_WhenWritingInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, null, expectedResult, headingUnderline);
    }

    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", 0, null)]
    [TestCase(100000000, "F", "en-US", "100000000.000", 1, null)]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", 2, null)]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, null)]
    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", 0, "-")]
    [TestCase(100000000, "F", "en-US", "100000000.000", 1, "-")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", 2, "-")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, "-")]
    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", 0, "++")]
    [TestCase(100000000, "F", "en-US", "100000000.000", 1, "++")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", 2, "++")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, "++")]
    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", 0, "___")]
    [TestCase(100000000, "F", "en-US", "100000000.000", 1, "___")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008", 2, "___")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", 0, null)]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", 1, null)]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", 2, null)]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", 3, null)]
    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", 0, "-")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", 1, "-")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", 2, "-")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", 3, "-")]
    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", 0, "++")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", 1, "++")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", 2, "++")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", 3, "++")]
    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", 0, "___")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000", 1, "___")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", 2, "___")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingFloatWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        float value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", 0, null)]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", 1, null)]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", 2, null)]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", 3, null)]
    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", 0, "-")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", 1, "-")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", 2, "-")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", 3, "-")]
    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", 0, "++")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", 1, "++")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", 2, "++")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", 3, "++")]
    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", 0, "___")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334", 1, "___")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", 2, "___")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", 3, "___")]
    public void ConsoleWriteHeading_WhenWritingDoubleWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
        double value,
        string? format,
        string culture,
        string expectedResult,
        int numberOfLineAppends,
        string? headingUnderline)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteHeadingWithFormatWithLineAppends(
            value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
    }

    [TestCase((ushort)255, "X", "FF")]
    [TestCase((ushort)255, "x", "ff")]
    public void ConsoleWrite_WhenWritingUInt16WithFormatString_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteWithFormat(value, format, null, expectedResult);
    }

    [TestCase((ushort)100, "C", "en-ZA", "R100,00")]
    [TestCase((ushort)100, "F", "en-US", "100.000")]
    [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002")]
    [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %")]
    public void ConsoleWrite_WhenWritingUInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        ushort value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase((uint)255, "X", "FF")]
    [TestCase((uint)255, "x", "ff")]
    public void ConsoleWrite_WhenWritingUInt32WithFormatString_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteWithFormat(value, format, null, expectedResult);
    }

    [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00")]
    [TestCase((uint)1000000, "F", "en-US", "1000000.000")]
    [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006")]
    [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %")]
    public void ConsoleWrite_WhenWritingUInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        uint value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase((ulong)255, "X", "FF")]
    [TestCase((ulong)255, "x", "ff")]
    public void ConsoleWrite_WhenWritingUInt64WithFormatString_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteWithFormat(value, format, null, expectedResult);
    }

    [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00")]
    [TestCase((ulong)100000000, "F", "en-US", "100000000.000")]
    [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008")]
    [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %")]
    public void ConsoleWrite_WhenWritingUInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        ulong value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF")]
    [TestCase(255, "x", "ff")]
    public void ConsoleWrite_WhenWritingInt16WithFormatString_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteWithFormat(value, format, null, expectedResult);
    }

    [TestCase(100, "C", "en-ZA", "R100,00")]
    [TestCase(100, "F", "en-US", "100.000")]
    [TestCase(100, "E", "nl-NL", "1,000000E+002")]
    [TestCase(100, "P", "fr-FR", "10 000,000 %")]
    public void ConsoleWrite_WhenWritingInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        short value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF")]
    [TestCase(255, "x", "ff")]
    public void ConsoleWrite_WhenWritingInt32WithFormatString_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteWithFormat(value, format, null, expectedResult);
    }

    [TestCase(1000000, "C", "en-ZA", "R1 000 000,00")]
    [TestCase(1000000, "F", "en-US", "1000000.000")]
    [TestCase(1000000, "E", "nl-NL", "1,000000E+006")]
    [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %")]
    public void ConsoleWrite_WhenWritingInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        int value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(255, "X", "FF")]
    [TestCase(255, "x", "ff")]
    public void ConsoleWrite_WhenWritingInt64WithFormatString_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string expectedResult)
    {
        AssertConsoleWriteWithFormat(value, format, null, expectedResult);
    }

    [TestCase(100000000, "C", "en-ZA", "R100 000 000,00")]
    [TestCase(100000000, "F", "en-US", "100000000.000")]
    [TestCase(100000000, "E", "nl-NL", "1,000000E+008")]
    [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %")]
    public void ConsoleWrite_WhenWritingInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        long value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00")]
    [TestCase(100000000.334f, "F", "en-US", "100000000.000")]
    [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008")]
    [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %")]
    public void ConsoleWrite_WhenWritingFloatWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        float value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33")]
    [TestCase(100000000.334, "F", "en-US", "100000000.334")]
    [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008")]
    [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %")]
    public void ConsoleWrite_WhenWritingDoubleWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
        double value,
        string? format,
        string culture,
        string expectedResult)
    {
        var formatProvider = CultureInfo.GetCultureInfo(culture, true);

        AssertConsoleWriteWithFormat(value, format, formatProvider, expectedResult);
    }

    [Test]
    public async Task ToFunctionTask_WhenProvidingAFunctionTask_ExecutesAsAFunction()
    {
        var checkDigit = 0;
        var function = this.ToFunctionTask(() =>
        {
            checkDigit++;

            return Task.CompletedTask;
        });

        function.Should().BeOfType<Func<Task>>();
        checkDigit.Should().Be(0);

        await function();

        checkDigit.Should().Be(1);
    }

    [Test]
    public void ToFunctionTask_WhenRunningMultipleFunctions_ExecutesAsParallel()
    {
        var checkDigit = 0;
        var root = new object();
        var threadIds = new Dictionary<int, int>();
        var tasks = new List<Func<Task>>(NumberOfTaskThreads);

        for (var i = 0; i < NumberOfTaskThreads; i++)
        {
            var function = this.ToFunctionTask(() =>
            {
                lock (root)
                {
                    checkDigit++;

                    ThreadingUtilities.IncreaseThreadIdDictionary(threadIds);
                    Task.Delay(250).Wait();
                }

                return Task.CompletedTask;
            });

            tasks.Add(function);
        }

        checkDigit.Should().Be(0);

        tasks.RunParallel();

        threadIds.Should().HaveCountGreaterThan(1);
        checkDigit.Should().Be(NumberOfTaskThreads);
    }

    private static void AssertConsoleWriteWithFormat(
        IFormattable? root,
        string? format,
        IFormatProvider? formatProvider,
        string expectedValue)
    {
        using var monitor = new ConsoleTestMonitor();

        root.ConsoleWrite(format, formatProvider);

        AssertValues(expectedValue, monitor.GetOutputText());
    }

    private static void AssertConsoleWriteLines(
        object? root,
        int numberOfWriteLines)
    {
        using var monitor = new ConsoleTestMonitor();
        var testValue = new StringBuilder();

        for (var i = 0; i < numberOfWriteLines; i++)
        {
            testValue.Append(Environment.NewLine);
        }

        root.ConsoleWriteLines(numberOfWriteLines);

        AssertValues(testValue.ToString(), monitor.GetOutputText());
    }

    private static void AssertConsoleWrite(
        object? root,
        string expectedValue)
    {
        using var monitor = new ConsoleTestMonitor();

        root.ConsoleWrite();

        AssertValues(expectedValue, monitor.GetOutputText());
    }

    private static void AssertConsoleWriteLine(
        object? root,
        string expectedValue)
    {
        GlobalAssertConsoleWriteLine(expectedValue, null, () => root.ConsoleWriteLine());
    }

    private static void AssertConsoleWriteLineWithLineAppends(
        object? root,
        string expectedValue,
        int numberOfLineAppends)
    {
        GlobalAssertConsoleWriteLine(expectedValue, numberOfLineAppends, () =>
            root.ConsoleWriteLine(numberOfLineAppends));
    }

    private static void AssertConsoleWriteLineWithFormat(
        IFormattable? root,
        string? format,
        IFormatProvider? formatProvider,
        string expectedValue)
    {
        GlobalAssertConsoleWriteLine(expectedValue, null, () =>
            root.ConsoleWriteLine(format, formatProvider));
    }

    private static void AssertConsoleWriteLineWithFormatWithLineAppends(
        IFormattable? root,
        int numberOfLineAppends,
        string? format,
        IFormatProvider? formatProvider,
        string expectedValue)
    {
        GlobalAssertConsoleWriteLine(expectedValue, numberOfLineAppends, () =>
            root.ConsoleWriteLine(format, formatProvider, numberOfLineAppends));
    }

    private static void AssertConsoleWriteHeading(
        object? root,
        string expectedValue,
        string? headingUnderline = null)
    {
        GlobalAssertConsoleWriteHeading(expectedValue, null, headingUnderline, () =>
            root.ConsoleWriteHeading(headingUnderline));
    }

    private static void AssertConsoleWriteHeadingWithLineAppends(
        object? root,
        string expectedValue,
        string? headingUnderline,
        int numberOfLineAppends)
    {
        GlobalAssertConsoleWriteHeading(expectedValue, numberOfLineAppends, headingUnderline, () =>
            root.ConsoleWriteHeading(headingUnderline, numberOfLineAppends));
    }

    private static void AssertConsoleWriteHeadingWithFormat(
        IFormattable? root,
        string? format,
        IFormatProvider? formatProvider,
        string expectedValue,
        string? headingUnderline = null)
    {
        GlobalAssertConsoleWriteHeading(expectedValue, null, headingUnderline, () =>
            root.ConsoleWriteHeading(format, formatProvider, headingUnderline));
    }

    private static void AssertConsoleWriteHeadingWithFormatWithLineAppends(
        IFormattable? root,
        int numberOfLineAppends,
        string? format,
        IFormatProvider? formatProvider,
        string expectedValue,
        string? headingUnderline = null)
    {
        GlobalAssertConsoleWriteHeading(expectedValue, numberOfLineAppends, headingUnderline, () =>
            root.ConsoleWriteHeading(format, formatProvider, headingUnderline, numberOfLineAppends));
    }

    private static void GlobalAssertConsoleWriteLine(
        string expectedValue,
        int? numberOfLineAppends,
        Action writeToRoot)
    {
        using var monitor = new ConsoleTestMonitor();
        var testValue = new StringBuilder($"{expectedValue}{Environment.NewLine}");

        if (numberOfLineAppends.HasValue)
        {
            for (var i = 0; i < numberOfLineAppends; i++)
            {
                testValue.Append(Environment.NewLine);
            }
        }

        writeToRoot();

        AssertValues(testValue.ToString(), monitor.GetOutputText());
    }

    private static void GlobalAssertConsoleWriteHeading(
        string expectedValue,
        int? numberOfLineAppends,
        string? headingUnderline,
        Action writeToRoot)
    {
        using var monitor = new ConsoleTestMonitor();
        var testValue = new StringBuilder($"{expectedValue}{Environment.NewLine}");
        var underline = headingUnderline.IsNullOrEmpty()
            ? ObjectExtensions.HeadingUnderline
            : headingUnderline;

        if (expectedValue.IsNotNullOrEmpty())
        {
            for (var i = 0; i < expectedValue.Length; i++)
            {
                testValue.Append(underline);
            }

            testValue.Append(Environment.NewLine);
        }

        if (numberOfLineAppends.HasValue)
        {
            for (var i = 0; i < numberOfLineAppends; i++)
            {
                testValue.Append(Environment.NewLine);
            }
        }

        writeToRoot();

        AssertValues(testValue.ToString(), monitor.GetOutputText());
    }

    private static void AssertValues(string expected, string provided)
    {
        /*
         * It would seem some Windows installations give different white space characters.
         * For example, Windows 11 professional gives fr-FR white spaces as (UTF-8):
         *
         * 1. \xc2\xa0      - NO-BREAK SPACE
         * 2. \xe2\x80\xaf  - NARROW NO-BREAK SPACE
         *
         * Therefore, to compensate for such differences, the code below replaces it with a normal space.
         */
        provided = provided
            .Replace(" ", " ")
            .Replace(" ", " ");

        expected.Should().Be(provided);
    }
}