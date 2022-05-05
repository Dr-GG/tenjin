using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Tenjin.Extensions;
using Tenjin.Tests.Enums;
using Tenjin.Tests.Models.Console;
using Tenjin.Tests.Services;
using Tenjin.Tests.Utilities;

namespace Tenjin.Tests.ExtensionsTests
{
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
            Assert.IsTrue(left.DoesNotEqual(right));
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
        [TestCase(true, new object?[] {true, true})]
        [TestCase(false, new object?[] { false, false })]
        [TestCase(BooleanEnum.True, new object?[] { BooleanEnum.True, BooleanEnum.True })]
        [TestCase(BooleanEnum.False, new object?[] { BooleanEnum.False, BooleanEnum.False })]
        public void EqualsAll_WhenProvidingMatchingObjects_ReturnsTrue(
            object? root, 
            object?[] objects)
        {
            Assert.IsTrue(root.EqualsAll(objects));
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
            Assert.IsFalse(root.EqualsAll(objects));
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
        [TestCase(1, new object?[] { 1, (byte)2})]
        [TestCase(1, new object?[] { 1, (char)2})]
        [TestCase(1, new object?[] { 1, (uint)2})]
        [TestCase(1, new object?[] { 1, (ushort)2})]
        [TestCase(1, new object?[] { 1, (ulong)2})]
        [TestCase(1, new object?[] { 1, (short)2})]
        [TestCase(1, new object?[] { 1, (long)2})]
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
            Assert.IsTrue(root.EqualsAny(objects));
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
            Assert.IsTrue(root.DoesNotEqualAny(objects));
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
            Assert.IsTrue(root.DoesNotEqualAll(objects));
        }

        [TestCase(null)]
        public void WriteLine_WhenWritingFromANullObject_LogsAnEmptyStringToTheConsole(object? root)
        {
            AssertWriteLine(root, string.Empty);
        }

        [TestCase(null)]
        public void Write_WhenWritingFromANullObject_LogsAnEmptyStringToTheConsole(object? root)
        {
            AssertWrite(root, string.Empty);
        }

        [TestCase(null, null)]
        [TestCase(null, "-")]
        [TestCase(null, "++")]
        [TestCase(null, "___")]
        public void WriteHeading_WhenWritingFromANullObject_LogsAnEmptyStringToTheConsole(
            object? root, 
            string? headingUnderline)
        {
            AssertWriteHeading(root, string.Empty, headingUnderline);
        }

        [TestCase(null, 0)]
        [TestCase(null, 1)]
        [TestCase(null, 2)]
        [TestCase(null, 3)]
        [TestCase(null, 4)]
        public void WriteLines_WhenWritingFromANullObject_LogsEmptyLinesToTheConsole(
            object? root,
            int numberOfLineAppends)
        {
            AssertWriteLines(root, numberOfLineAppends);
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
        public void WriteHeading_WhenWritingFromANullObject_LogsEmptyLinesToTheConsole(
            object? root,
            string? headingUnderline,
            int numberOfLineAppends)
        {
            AssertWriteHeadingWithLineAppends(root, string.Empty, headingUnderline, numberOfLineAppends);
        }

        [TestCase("Test a string", "Test a string")]
        [TestCase(1, "1")]
        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void WriteLine_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(object root, string expectedValue)
        {
            AssertWriteLine(root, expectedValue);
        }

        [TestCase("Test a string", "Test a string", 0)]
        [TestCase(1, "1", 1)]
        [TestCase(true, "True", 2)]
        [TestCase(false, "False", 3)]
        public void WriteLine_WhenWritingFromANonNullObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
            object root, 
            string expectedValue,
            int numberOfLineAppends)
        {
            AssertWriteLineWithLineAppends(root, expectedValue, numberOfLineAppends);
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
        public void WriteHeading_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(
            object root, 
            string expectedValue,
            string? headingUnderline)
        {
            AssertWriteHeading(root, expectedValue, headingUnderline);
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
        public void WriteHeading_WhenWritingFromANonNullObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
            object root,
            string expectedValue,
            string? headingUnderline,
            int numberOfLineAppends)
        {
            AssertWriteHeadingWithLineAppends(root, expectedValue, headingUnderline, numberOfLineAppends);
        }

        [TestCase("Test a string", "Test a string")]
        [TestCase(1, "1")]
        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void Write_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(object root, string expectedValue)
        {
            AssertWrite(root, expectedValue);
        }

        [TestCase("Test a string", 0)]
        [TestCase(1, 1)]
        [TestCase(1.0, 2)]
        [TestCase(1.0f, 3)]
        [TestCase(true, 4)]
        [TestCase(false, 5)]
        public void WriteLines_WhenWritingFromANonNullObject_LogsTheExpectedValueToTheConsole(object root, int numberOfWriteLines)
        {
            AssertWriteLines(root, numberOfWriteLines);
        }

        [TestCase("Name X")]
        [TestCase("X Name")]
        [TestCase("Tenjin")]
        public void WriteLine_WhenWritingFromACustomObject_LogsTheExpectedValueToTheConsole(string name)
        {
            var customObject = new ConsoleObject(name);
            var expectedValue = ConsoleObject.GetOutputText(name);

            AssertWriteLine(customObject, expectedValue);
        }

        [TestCase("Name X", 0)]
        [TestCase("X Name", 1)]
        [TestCase("Tenjin", 2)]
        public void WriteLine_WhenWritingFromACustomObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
            string name,
            int numberOfLineAppends)
        {
            var customObject = new ConsoleObject(name);
            var expectedValue = ConsoleObject.GetOutputText(name);

            AssertWriteLineWithLineAppends(customObject, expectedValue, numberOfLineAppends);
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
        public void WriteHeading_WhenWritingFromACustomObject_LogsTheExpectedValueToTheConsole(string name, string? headingUnderline)
        {
            var customObject = new ConsoleObject(name);
            var expectedValue = ConsoleObject.GetOutputText(name);

            AssertWriteHeading(customObject, expectedValue);
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
        public void WriteHeading_WhenWritingFromACustomObjectWithLineAppends_LogsTheExpectedValueToTheConsole(
            string name,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var customObject = new ConsoleObject(name);
            var expectedValue = ConsoleObject.GetOutputText(name);

            AssertWriteHeadingWithLineAppends(customObject, expectedValue, headingUnderline, numberOfLineAppends);
        }

        [TestCase("Name X")]
        [TestCase("X Name")]
        [TestCase("Tenjin")]
        public void Write_WhenWritingFromACustomObject_LogsTheExpectedValueToTheConsole(string name)
        {
            var customObject = new ConsoleObject(name);
            var expectedValue = ConsoleObject.GetOutputText(name);

            AssertWrite(customObject, expectedValue);
        }

        [Test]
        public void WriteLine_WhenWritingNullableFormattableObjects_LogsAnEmptyString()
        {
            AssertWriteLineWithFormat(null, "bogus-format-string", null, string.Empty);
        }

        [TestCase(null)]
        [TestCase("-")]
        [TestCase("==")]
        [TestCase("___")]
        public void WriteHeading_WhenWritingNullableFormattableObjects_LogsAnEmptyString(string? headerUnderline)
        {
            AssertWriteHeadingWithFormat(null, null, null, string.Empty, headerUnderline);
        }

        [Test]
        public void Write_WhenWritingNullableFormattableObjects_LogsAnEmptyString()
        {
            AssertWriteWithFormat(null, "bogus-format-string", null, string.Empty);
        }

        [TestCase("2000-01-01")]
        [TestCase("2005-01-05")]
        [TestCase("2005-02-10")]
        [TestCase("2010-10-10")]
        [TestCase("2020-12-31")]
        public void WriteLine_WhenWritingDateTimeWithFormatString_LogsTheExpectedValueToTheConsole(string value)
        {
            var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
            var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

            AssertWriteLineWithFormat(dateTime, DateTimeWriteLineOutputFormat, null, expectedValue);
        }

        [TestCase("2000-01-01", 0)]
        [TestCase("2005-01-05", 1)]
        [TestCase("2005-02-10", 2)]
        [TestCase("2010-10-10", 3)]
        [TestCase("2020-12-31", 4)]
        public void WriteLine_WhenWritingDateTimeWithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            string value,
            int numberOfLineAppends)
        {
            var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
            var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

            AssertWriteLineWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingDateTimeWithFormatString_LogsTheExpectedValueToTheConsole(string value, string? headingUnderline)
        {
            var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
            var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

            AssertWriteHeadingWithFormat(dateTime, DateTimeWriteLineOutputFormat, null, expectedValue, headingUnderline);
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
        public void WriteHeading_WhenWritingDateTimeWithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            string value,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
            var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

            AssertWriteHeadingWithFormatWithLineAppends(
                dateTime, numberOfLineAppends, DateTimeWriteLineOutputFormat, null, expectedValue, headingUnderline);
        }

        [TestCase("2000-01-01")]
        [TestCase("2005-01-05")]
        [TestCase("2005-02-10")]
        [TestCase("2010-10-10")]
        [TestCase("2020-12-31")]
        public void Write_WhenWritingDateTimeWithFormatString_LogsTheExpectedValueToTheConsole(string value)
        {
            var dateTime = DateTime.ParseExact(value, DateTimeWriteLineInputFormat, null);
            var expectedValue = dateTime.ToString(DateTimeWriteLineOutputFormat);

            AssertWriteWithFormat(dateTime, DateTimeWriteLineOutputFormat, null, expectedValue);
        }

        [TestCase((ushort)255, "X", "FF")]
        [TestCase((ushort)255, "x", "ff")]
        public void WriteLine_WhenWritingUInt16WithFormatString_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string expectedResult)
        {
            AssertWriteLineWithFormat(value, format, null, expectedResult);
        }

        [TestCase((ushort)100, "C", "en-ZA", "R100,00")]
        [TestCase((ushort)100, "F", "en-US", "100.000")]
        [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002")]
        [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %")]
        public void WriteLine_WhenWritingUInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase((uint)255, "X", "FF")]
        [TestCase((uint)255, "x", "ff")]
        public void WriteLine_WhenWritingUInt32WithFormatString_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string expectedResult)
        {
            AssertWriteLineWithFormat(value, format, null, expectedResult);
        }

        [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00")]
        [TestCase((uint)1000000, "F", "en-US", "1000000.000")]
        [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006")]
        [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %")]
        public void WriteLine_WhenWritingUInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase((ulong)255, "X", "FF")]
        [TestCase((ulong)255, "x", "ff")]
        public void WriteLine_WhenWritingUInt64WithFormatString_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string expectedResult)
        {
            AssertWriteLineWithFormat(value, format, null, expectedResult);
        }

        [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00")]
        [TestCase((ulong)100000000, "F", "en-US", "100000000.000")]
        [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008")]
        [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %")]
        public void WriteLine_WhenWritingUInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF")]
        [TestCase(255, "x", "ff")]
        public void WriteLine_WhenWritingInt16WithFormatString_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string expectedResult)
        {
            AssertWriteLineWithFormat(value, format, null, expectedResult);
        }

        [TestCase(100, "C", "en-ZA", "R100,00")]
        [TestCase(100, "F", "en-US", "100.000")]
        [TestCase(100, "E", "nl-NL", "1,000000E+002")]
        [TestCase(100, "P", "fr-FR", "10 000,000 %")]
        public void WriteLine_WhenWritingInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            short value, 
            string? format, 
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF")]
        [TestCase(255, "x", "ff")]
        public void WriteLine_WhenWritingInt32WithFormatString_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string expectedResult)
        {
            AssertWriteLineWithFormat(value, format, null, expectedResult);
        }

        [TestCase(1000000, "C", "en-ZA", "R1 000 000,00")]
        [TestCase(1000000, "F", "en-US", "1000000.000")]
        [TestCase(1000000, "E", "nl-NL", "1,000000E+006")]
        [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %")]
        public void WriteLine_WhenWritingInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF")]
        [TestCase(255, "x", "ff")]
        public void WriteLine_WhenWritingInt64WithFormatString_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string expectedResult)
        {
            AssertWriteLineWithFormat(value, format, null, expectedResult);
        }

        [TestCase(100000000, "C", "en-ZA", "R100 000 000,00")]
        [TestCase(100000000, "F", "en-US", "100000000.000")]
        [TestCase(100000000, "E", "nl-NL", "1,000000E+008")]
        [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %")]
        public void WriteLine_WhenWritingInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00")]
        [TestCase(100000000.334f, "F", "en-US", "100000000.000")]
        [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008")]
        [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %")]
        public void WriteLine_WhenWritingFloatWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            float value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33")]
        [TestCase(100000000.334, "F", "en-US", "100000000.334")]
        [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008")]
        [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %")]
        public void WriteLine_WhenWritingDoubleWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            double value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase((ushort)255, "X", "FF", 0)]
        [TestCase((ushort)255, "x", "ff", 1)]
        public void WriteLine_WhenWritingUInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string expectedResult,
            int numberOfLineAppends)
        {
            AssertWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
        }

        [TestCase((ushort)100, "C", "en-ZA", "R100,00", 0)]
        [TestCase((ushort)100, "F", "en-US", "100.000", 1)]
        [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002", 2)]
        [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %", 3)]
        public void WriteLine_WhenWritingUInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase((uint)255, "X", "FF", 0)]
        [TestCase((uint)255, "x", "ff", 1)]
        public void WriteLine_WhenWritingUInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string expectedResult,
            int numberOfLineAppends)
        {
            AssertWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
        }

        [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00", 0)]
        [TestCase((uint)1000000, "F", "en-US", "1000000.000", 1)]
        [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006", 2)]
        [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %", 3)]
        public void WriteLine_WhenWritingUInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase((ulong)255, "X", "FF", 0)]
        [TestCase((ulong)255, "x", "ff", 1)]
        public void WriteLine_WhenWritingUInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string expectedResult,
            int numberOfLineAppends)
        {
            AssertWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
        }

        [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00", 0)]
        [TestCase((ulong)100000000, "F", "en-US", "100000000.000", 1)]
        [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008", 2)]
        [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %", 3)]
        public void WriteLine_WhenWritingUInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF", 0)]
        [TestCase(255, "x", "ff", 1)]
        public void WriteLine_WhenWritingInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string expectedResult,
            int numberOfLineAppends)
        {
            AssertWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
        }

        [TestCase(100, "C", "en-ZA", "R100,00", 0)]
        [TestCase(100, "F", "en-US", "100.000", 1)]
        [TestCase(100, "E", "nl-NL", "1,000000E+002", 2)]
        [TestCase(100, "P", "fr-FR", "10 000,000 %", 3)]
        public void WriteLine_WhenWritingInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF", 0)]
        [TestCase(255, "x", "ff", 1)]
        public void WriteLine_WhenWritingInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string expectedResult,
            int numberOfLineAppends)
        {
            AssertWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
        }

        [TestCase(1000000, "C", "en-ZA", "R1 000 000,00", 0)]
        [TestCase(1000000, "F", "en-US", "1000000.000", 1)]
        [TestCase(1000000, "E", "nl-NL", "1,000000E+006", 2)]
        [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %", 3)]
        public void WriteLine_WhenWritingInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF", 0)]
        [TestCase(255, "x", "ff", 1)]
        public void WriteLine_WhenWritingInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string expectedResult,
            int numberOfLineAppends)
        {
            AssertWriteLineWithFormatWithLineAppends(value, numberOfLineAppends, format, null, expectedResult);
        }

        [TestCase(100000000, "C", "en-ZA", "R100 000 000,00", 0)]
        [TestCase(100000000, "F", "en-US", "100000000.000", 1)]
        [TestCase(100000000, "E", "nl-NL", "1,000000E+008", 2)]
        [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %", 3)]
        public void WriteLine_WhenWritingInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00", 0)]
        [TestCase(100000000.334f, "F", "en-US", "100000000.000", 1)]
        [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008", 2)]
        [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %", 3)]
        public void WriteLine_WhenWritingFloatWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            float value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult);
        }

        [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33", 0)]
        [TestCase(100000000.334, "F", "en-US", "100000000.334", 1)]
        [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008", 2)]
        [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %", 3)]
        public void WriteLine_WhenWritingDoubleWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            double value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteLineWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingUInt16WithFormatString_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string expectedResult,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
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
        public void WriteHeading_WhenWritingUInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase((uint)255, "X", "FF", null)]
        [TestCase((uint)255, "x", "ff", null)]
        [TestCase((uint)255, "X", "FF", "-")]
        [TestCase((uint)255, "x", "ff", "-")]
        [TestCase((uint)255, "X", "FF", "++")]
        [TestCase((uint)255, "x", "ff", "++")]
        [TestCase((uint)255, "X", "FF", "___")]
        [TestCase((uint)255, "x", "ff", "___")]
        public void WriteHeading_WhenWritingUInt32WithFormatString_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string expectedResult,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormat(value, format, null, expectedResult);
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
        public void WriteHeading_WhenWritingUInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase((ulong)255, "X", "FF", null)]
        [TestCase((ulong)255, "x", "ff", null)]
        [TestCase((ulong)255, "X", "FF", "-")]
        [TestCase((ulong)255, "x", "ff", "-")]
        [TestCase((ulong)255, "X", "FF", "++")]
        [TestCase((ulong)255, "x", "ff", "++")]
        [TestCase((ulong)255, "X", "FF", "___")]
        [TestCase((ulong)255, "x", "ff", "___")]
        public void WriteHeading_WhenWritingUInt64WithFormatString_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string expectedResult,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormat(value, format, null, expectedResult);
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
        public void WriteHeading_WhenWritingUInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase(255, "X", "FF", null)]
        [TestCase(255, "x", "ff", null)]
        [TestCase(255, "X", "FF", "-")]
        [TestCase(255, "x", "ff", "-")]
        [TestCase(255, "X", "FF", "++")]
        [TestCase(255, "x", "ff", "++")]
        [TestCase(255, "X", "FF", "___")]
        [TestCase(255, "x", "ff", "___")]
        public void WriteHeading_WhenWritingInt16WithFormatString_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string expectedResult,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
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
        public void WriteHeading_WhenWritingInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase(255, "X", "FF", null)]
        [TestCase(255, "x", "ff", null)]
        [TestCase(255, "X", "FF", "-")]
        [TestCase(255, "x", "ff", "-")]
        [TestCase(255, "X", "FF", "++")]
        [TestCase(255, "x", "ff", "++")]
        [TestCase(255, "X", "FF", "___")]
        [TestCase(255, "x", "ff", "___")]
        public void WriteHeading_WhenWritingInt32WithFormatString_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string expectedResult,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
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
        public void WriteHeading_WhenWritingInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase(255, "X", "FF", null)]
        [TestCase(255, "x", "ff", null)]
        [TestCase(255, "X", "FF", "-")]
        [TestCase(255, "x", "ff", "-")]
        [TestCase(255, "X", "FF", "++")]
        [TestCase(255, "x", "ff", "++")]
        [TestCase(255, "X", "FF", "___")]
        [TestCase(255, "x", "ff", "___")]
        public void WriteHeading_WhenWritingInt64WithFormatString_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string expectedResult,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormat(value, format, null, expectedResult, headingUnderline);
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
        public void WriteHeading_WhenWritingInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
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
        public void WriteHeading_WhenWritingFloatWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            float value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
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
        public void WriteHeading_WhenWritingDoubleWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            double value,
            string? format,
            string culture,
            string expectedResult,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormat(value, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase((ushort)255, "X", "FF", 0, null)]
        [TestCase((ushort)255, "x", "ff", 1, null)]
        [TestCase((ushort)255, "X", "FF", 0, "-")]
        [TestCase((ushort)255, "x", "ff", 1, "-")]
        [TestCase((ushort)255, "X", "FF", 0, "++")]
        [TestCase((ushort)255, "x", "ff", 1, "++")]
        [TestCase((ushort)255, "X", "FF", 0, "___")]
        [TestCase((ushort)255, "x", "ff", 1, "___")]
        public void WriteHeading_WhenWritingUInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingUInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingUInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingUInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingUInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingUInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingInt16WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingInt16WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingInt32WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingInt32WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingInt64WithFormatStringWithLineAppends_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingInt64WithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingFloatWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            float value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
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
        public void WriteHeading_WhenWritingDoubleWithFormatStringAndFormatProviderWithLineAppends_LogsTheExpectedValueToTheConsole(
            double value,
            string? format,
            string culture,
            string expectedResult,
            int numberOfLineAppends,
            string? headingUnderline)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteHeadingWithFormatWithLineAppends(
                value, numberOfLineAppends, format, formatProvider, expectedResult, headingUnderline);
        }

        [TestCase((ushort)255, "X", "FF")]
        [TestCase((ushort)255, "x", "ff")]
        public void Write_WhenWritingUInt16WithFormatString_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string expectedResult)
        {
            AssertWriteWithFormat(value, format, null, expectedResult);
        }

        [TestCase((ushort)100, "C", "en-ZA", "R100,00")]
        [TestCase((ushort)100, "F", "en-US", "100.000")]
        [TestCase((ushort)100, "E", "nl-NL", "1,000000E+002")]
        [TestCase((ushort)100, "P", "fr-FR", "10 000,000 %")]
        public void Write_WhenWritingUInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            ushort value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase((uint)255, "X", "FF")]
        [TestCase((uint)255, "x", "ff")]
        public void Write_WhenWritingUInt32WithFormatString_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string expectedResult)
        {
            AssertWriteWithFormat(value, format, null, expectedResult);
        }

        [TestCase((uint)1000000, "C", "en-ZA", "R1 000 000,00")]
        [TestCase((uint)1000000, "F", "en-US", "1000000.000")]
        [TestCase((uint)1000000, "E", "nl-NL", "1,000000E+006")]
        [TestCase((uint)1000000, "P", "fr-FR", "100 000 000,000 %")]
        public void Write_WhenWritingUInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            uint value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase((ulong)255, "X", "FF")]
        [TestCase((ulong)255, "x", "ff")]
        public void Write_WhenWritingUInt64WithFormatString_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string expectedResult)
        {
            AssertWriteWithFormat(value, format, null, expectedResult);
        }

        [TestCase((ulong)100000000, "C", "en-ZA", "R100 000 000,00")]
        [TestCase((ulong)100000000, "F", "en-US", "100000000.000")]
        [TestCase((ulong)100000000, "E", "nl-NL", "1,000000E+008")]
        [TestCase((ulong)100000000, "P", "fr-FR", "10 000 000 000,000 %")]
        public void Write_WhenWritingUInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            ulong value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF")]
        [TestCase(255, "x", "ff")]
        public void Write_WhenWritingInt16WithFormatString_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string expectedResult)
        {
            AssertWriteWithFormat(value, format, null, expectedResult);
        }

        [TestCase(100, "C", "en-ZA", "R100,00")]
        [TestCase(100, "F", "en-US", "100.000")]
        [TestCase(100, "E", "nl-NL", "1,000000E+002")]
        [TestCase(100, "P", "fr-FR", "10 000,000 %")]
        public void Write_WhenWritingInt16WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            short value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF")]
        [TestCase(255, "x", "ff")]
        public void Write_WhenWritingInt32WithFormatString_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string expectedResult)
        {
            AssertWriteWithFormat(value, format, null, expectedResult);
        }

        [TestCase(1000000, "C", "en-ZA", "R1 000 000,00")]
        [TestCase(1000000, "F", "en-US", "1000000.000")]
        [TestCase(1000000, "E", "nl-NL", "1,000000E+006")]
        [TestCase(1000000, "P", "fr-FR", "100 000 000,000 %")]
        public void Write_WhenWritingInt32WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            int value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(255, "X", "FF")]
        [TestCase(255, "x", "ff")]
        public void Write_WhenWritingInt64WithFormatString_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string expectedResult)
        {
            AssertWriteWithFormat(value, format, null, expectedResult);
        }

        [TestCase(100000000, "C", "en-ZA", "R100 000 000,00")]
        [TestCase(100000000, "F", "en-US", "100000000.000")]
        [TestCase(100000000, "E", "nl-NL", "1,000000E+008")]
        [TestCase(100000000, "P", "fr-FR", "10 000 000 000,000 %")]
        public void Write_WhenWritingInt64WithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            long value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(100000000.334f, "C", "en-ZA", "R100 000 000,00")]
        [TestCase(100000000.334f, "F", "en-US", "100000000.000")]
        [TestCase(100000000.334f, "E", "nl-NL", "1,000000E+008")]
        [TestCase(100000000.334f, "P", "fr-FR", "10 000 000 000,000 %")]
        public void Write_WhenWritingFloatWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            float value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
        }

        [TestCase(100000000.334, "C", "en-ZA", "R100 000 000,33")]
        [TestCase(100000000.334, "F", "en-US", "100000000.334")]
        [TestCase(100000000.334, "E", "nl-NL", "1,000000E+008")]
        [TestCase(100000000.334, "P", "fr-FR", "10 000 000 033,400 %")]
        public void Write_WhenWritingDoubleWithFormatStringAndFormatProvider_LogsTheExpectedValueToTheConsole(
            double value,
            string? format,
            string culture,
            string expectedResult)
        {
            var formatProvider = CultureInfo.GetCultureInfo(culture, true);

            AssertWriteWithFormat(value, format, formatProvider, expectedResult);
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

            Assert.IsInstanceOf<Func<Task>>(function);
            Assert.AreEqual(0, checkDigit);

            await function();

            Assert.AreEqual(1, checkDigit);
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
                        Thread.Sleep(250);
                    }

                    return Task.CompletedTask;
                });

                tasks.Add(function);
            }

            Assert.AreEqual(0, checkDigit);

            tasks.RunParallel();

            Assert.Greater(threadIds.Count, 1);
            Assert.AreEqual(NumberOfTaskThreads, checkDigit);
        }

        private static void AssertWriteWithFormat(
            IFormattable? root,
            string? format,
            IFormatProvider? formatProvider,
            string expectedValue)
        {
            using var monitor = new ConsoleTestMonitor();

            root.Write(format, formatProvider);

            AssertValues(expectedValue, monitor.GetOutputText());
        }

        private static void AssertWriteLines(
            object? root,
            int numberOfWriteLines)
        {
            using var monitor = new ConsoleTestMonitor();
            var testValue = new StringBuilder();

            for (var i = 0; i < numberOfWriteLines; i++)
            {
                testValue.Append(Environment.NewLine);
            }

            root.WriteLines(numberOfWriteLines);

            AssertValues(testValue.ToString(), monitor.GetOutputText());
        }

        private static void AssertWrite(
            object? root,
            string expectedValue)
        {
            using var monitor = new ConsoleTestMonitor();

            root.Write();

            AssertValues(expectedValue, monitor.GetOutputText());
        }

        private static void AssertWriteLine(
            object? root,
            string expectedValue)
        {
            GlobalAssertWriteLine(expectedValue, null, () => root.WriteLine());
        }

        private static void AssertWriteLineWithLineAppends(
            object? root,
            string expectedValue,
            int numberOfLineAppends)
        {
            GlobalAssertWriteLine(expectedValue, numberOfLineAppends, () => 
                root.WriteLine(numberOfLineAppends));
        }

        private static void AssertWriteLineWithFormat(
            IFormattable? root,
            string? format,
            IFormatProvider? formatProvider,
            string expectedValue)
        {
            GlobalAssertWriteLine(expectedValue, null, () =>
                root.WriteLine(format, formatProvider));
        }

        private static void AssertWriteLineWithFormatWithLineAppends(
            IFormattable? root,
            int numberOfLineAppends,
            string? format,
            IFormatProvider? formatProvider,
            string expectedValue)
        {
            GlobalAssertWriteLine(expectedValue, numberOfLineAppends, () =>
                root.WriteLine(format, formatProvider, numberOfLineAppends));
        }

        private static void AssertWriteHeading(
            object? root,
            string expectedValue,
            string? headingUnderline = null)
        {
            GlobalAssertWriteHeading(expectedValue, null, headingUnderline, () =>
                root.WriteHeading(headingUnderline));
        }

        private static void AssertWriteHeadingWithLineAppends(
            object? root,
            string expectedValue,
            string? headingUnderline,
            int numberOfLineAppends)
        {
            GlobalAssertWriteHeading(expectedValue, numberOfLineAppends, headingUnderline, () =>
                root.WriteHeading(headingUnderline, numberOfLineAppends));
        }

        private static void AssertWriteHeadingWithFormat(
            IFormattable? root,
            string? format,
            IFormatProvider? formatProvider,
            string expectedValue,
            string? headingUnderline = null)
        {
            GlobalAssertWriteHeading(expectedValue, null, headingUnderline, () =>
                root.WriteHeading(format, formatProvider, headingUnderline));
        }

        private static void AssertWriteHeadingWithFormatWithLineAppends(
            IFormattable? root,
            int numberOfLineAppends,
            string? format,
            IFormatProvider? formatProvider,
            string expectedValue,
            string? headingUnderline = null)
        {
            GlobalAssertWriteHeading(expectedValue, numberOfLineAppends, headingUnderline, () =>
                root.WriteHeading(format, formatProvider, headingUnderline, numberOfLineAppends));
        }

        private static void GlobalAssertWriteLine(
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

        private static void GlobalAssertWriteHeading(
            string expectedValue,
            int? numberOfLineAppends,
            string? headingUnderline,
            Action writeToRoot)
        {
            using var monitor = new ConsoleTestMonitor();
            var testValue = new StringBuilder($"{expectedValue}{Environment.NewLine}");
            var underline = headingUnderline.IsNotNullOrEmpty()
                ? ObjectExtensions.DefaultHeadingUnderline
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
             * 1. \xc2\xa0 - NO-BREAK SPACE
             * 2. \xe2\x80\xaf	NARROW NO-BREAK SPACE
             *
             * Therefore, to compensate for such differences, the code below replaces it with a normal space.
             */
            provided = provided
                .Replace(" ", " ")
                .Replace(" ", " ");

            Assert.AreEqual(expected, provided);
        }
    }
}
