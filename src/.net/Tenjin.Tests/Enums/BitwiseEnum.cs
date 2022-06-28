using System;

namespace Tenjin.Tests.Enums;

[Flags]
public enum BitwiseEnum
{
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _4 = 4,
    _8 = 8,
    _16 = 16,
    _32 = 32,
    _64 = 64,
    _128 = 128,
    _256 = 256
}