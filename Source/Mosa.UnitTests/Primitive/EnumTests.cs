// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive;

enum EnumU1Type : byte
{
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumU1
{
	[MosaUnitTest]
	public static byte PositiveConversion()
	{
		return (byte)EnumU1Type.Positive50;
	}

	[MosaUnitTest]
	public static byte PositivePlusOne1()
	{
		return (byte)(EnumU1Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static byte PositivePlusOne2()
	{
		return (byte)(EnumU1Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static byte PositiveMinusOne1()
	{
		return (byte)(EnumU1Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static byte PositiveMinusOne2()
	{
		return (byte)(EnumU1Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static byte PositiveShl()
	{
		return (byte)EnumU1Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static byte PositiveShr()
	{
		return (byte)EnumU1Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static byte PositiveMul2()
	{
		return (byte)EnumU1Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static byte PositiveDiv2()
	{
		return (byte)EnumU1Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static byte PositiveRem2()
	{
		return (byte)EnumU1Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static byte PositiveAssignPlusOne()
	{
		var e = EnumU1Type.Positive50;
		e += 1;
		return (byte)e;
	}

	[MosaUnitTest]
	public static byte PositiveAssignMinusOne()
	{
		var e = EnumU1Type.Positive50;
		e -= 1;
		return (byte)e;
	}

	[MosaUnitTest]
	public static byte PositivePreincrement()
	{
		var e = EnumU1Type.Positive50;
		++e;
		return (byte)e;
	}

	[MosaUnitTest]
	public static byte PositivePredecrement()
	{
		var e = EnumU1Type.Positive50;
		--e;
		return (byte)e;
	}

	[MosaUnitTest]
	public static byte PositivePostincrement()
	{
		var e = EnumU1Type.Positive50;
		e++;
		return (byte)e;
	}

	[MosaUnitTest]
	public static byte PositivePostdecrement()
	{
		var e = EnumU1Type.Positive50;
		e--;
		return (byte)e;
	}

	[MosaUnitTest]
	public static byte PositiveAnd()
	{
		return (byte)EnumU1Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static byte PositiveOr()
	{
		return (byte)EnumU1Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static byte PositiveXOr()
	{
		return (byte)EnumU1Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumU1Type e, byte v)
	{
		return (byte)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumU1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumU1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumU1Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumU1Type e, byte v)
	{
		return (byte)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumU1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumU1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumU1Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumU1Type e, byte v)
	{
		return (byte)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumU1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumU1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumU1Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumU1Type e, byte v)
	{
		return (byte)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumU1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumU1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumU1Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumU1Type e, byte v)
	{
		return (byte)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU1Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumU1Type e, byte v)
	{
		return (byte)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumU1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumU1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumU1Type.Positive50, 49);
	}

}

enum EnumU2Type : ushort
{
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumU2
{
	[MosaUnitTest]
	public static ushort PositiveConversion()
	{
		return (ushort)EnumU2Type.Positive50;
	}

	[MosaUnitTest]
	public static ushort PositivePlusOne1()
	{
		return (ushort)(EnumU2Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static ushort PositivePlusOne2()
	{
		return (ushort)(EnumU2Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static ushort PositiveMinusOne1()
	{
		return (ushort)(EnumU2Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static ushort PositiveMinusOne2()
	{
		return (ushort)(EnumU2Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static ushort PositiveShl()
	{
		return (ushort)EnumU2Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static ushort PositiveShr()
	{
		return (ushort)EnumU2Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static ushort PositiveMul2()
	{
		return (ushort)EnumU2Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static ushort PositiveDiv2()
	{
		return (ushort)EnumU2Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static ushort PositiveRem2()
	{
		return (ushort)EnumU2Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static ushort PositiveAssignPlusOne()
	{
		var e = EnumU2Type.Positive50;
		e += 1;
		return (ushort)e;
	}

	[MosaUnitTest]
	public static ushort PositiveAssignMinusOne()
	{
		var e = EnumU2Type.Positive50;
		e -= 1;
		return (ushort)e;
	}

	[MosaUnitTest]
	public static ushort PositivePreincrement()
	{
		var e = EnumU2Type.Positive50;
		++e;
		return (ushort)e;
	}

	[MosaUnitTest]
	public static ushort PositivePredecrement()
	{
		var e = EnumU2Type.Positive50;
		--e;
		return (ushort)e;
	}

	[MosaUnitTest]
	public static ushort PositivePostincrement()
	{
		var e = EnumU2Type.Positive50;
		e++;
		return (ushort)e;
	}

	[MosaUnitTest]
	public static ushort PositivePostdecrement()
	{
		var e = EnumU2Type.Positive50;
		e--;
		return (ushort)e;
	}

	[MosaUnitTest]
	public static ushort PositiveAnd()
	{
		return (ushort)EnumU2Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static ushort PositiveOr()
	{
		return (ushort)EnumU2Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static ushort PositiveXOr()
	{
		return (ushort)EnumU2Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumU2Type e, ushort v)
	{
		return (ushort)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumU2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumU2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumU2Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumU2Type e, ushort v)
	{
		return (ushort)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumU2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumU2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumU2Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumU2Type e, ushort v)
	{
		return (ushort)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumU2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumU2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumU2Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumU2Type e, ushort v)
	{
		return (ushort)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumU2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumU2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumU2Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumU2Type e, ushort v)
	{
		return (ushort)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU2Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumU2Type e, ushort v)
	{
		return (ushort)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumU2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumU2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumU2Type.Positive50, 49);
	}

}

enum EnumU4Type : uint
{
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumU4
{
	[MosaUnitTest]
	public static uint PositiveConversion()
	{
		return (uint)EnumU4Type.Positive50;
	}

	[MosaUnitTest]
	public static uint PositivePlusOne1()
	{
		return (uint)(EnumU4Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static uint PositivePlusOne2()
	{
		return (uint)(EnumU4Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static uint PositiveMinusOne1()
	{
		return (uint)(EnumU4Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static uint PositiveMinusOne2()
	{
		return (uint)(EnumU4Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static uint PositiveShl()
	{
		return (uint)EnumU4Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static uint PositiveShr()
	{
		return (uint)EnumU4Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static uint PositiveMul2()
	{
		return (uint)EnumU4Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static uint PositiveDiv2()
	{
		return (uint)EnumU4Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static uint PositiveRem2()
	{
		return (uint)EnumU4Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static uint PositiveAssignPlusOne()
	{
		var e = EnumU4Type.Positive50;
		e += 1;
		return (uint)e;
	}

	[MosaUnitTest]
	public static uint PositiveAssignMinusOne()
	{
		var e = EnumU4Type.Positive50;
		e -= 1;
		return (uint)e;
	}

	[MosaUnitTest]
	public static uint PositivePreincrement()
	{
		var e = EnumU4Type.Positive50;
		++e;
		return (uint)e;
	}

	[MosaUnitTest]
	public static uint PositivePredecrement()
	{
		var e = EnumU4Type.Positive50;
		--e;
		return (uint)e;
	}

	[MosaUnitTest]
	public static uint PositivePostincrement()
	{
		var e = EnumU4Type.Positive50;
		e++;
		return (uint)e;
	}

	[MosaUnitTest]
	public static uint PositivePostdecrement()
	{
		var e = EnumU4Type.Positive50;
		e--;
		return (uint)e;
	}

	[MosaUnitTest]
	public static uint PositiveAnd()
	{
		return (uint)EnumU4Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static uint PositiveOr()
	{
		return (uint)EnumU4Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static uint PositiveXOr()
	{
		return (uint)EnumU4Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumU4Type e, uint v)
	{
		return (uint)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumU4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumU4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumU4Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumU4Type e, uint v)
	{
		return (uint)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumU4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumU4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumU4Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumU4Type e, uint v)
	{
		return (uint)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumU4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumU4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumU4Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumU4Type e, uint v)
	{
		return (uint)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumU4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumU4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumU4Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumU4Type e, uint v)
	{
		return (uint)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU4Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumU4Type e, uint v)
	{
		return (uint)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumU4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumU4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumU4Type.Positive50, 49);
	}

}

enum EnumU8Type : ulong
{
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumU8
{
	[MosaUnitTest]
	public static ulong PositiveConversion()
	{
		return (ulong)EnumU8Type.Positive50;
	}

	[MosaUnitTest]
	public static ulong PositivePlusOne1()
	{
		return (ulong)(EnumU8Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static ulong PositivePlusOne2()
	{
		return (ulong)(EnumU8Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static ulong PositiveMinusOne1()
	{
		return (ulong)(EnumU8Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static ulong PositiveMinusOne2()
	{
		return (ulong)(EnumU8Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static ulong PositiveShl()
	{
		return (ulong)EnumU8Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static ulong PositiveShr()
	{
		return (ulong)EnumU8Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static ulong PositiveMul2()
	{
		return (ulong)EnumU8Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static ulong PositiveDiv2()
	{
		return (ulong)EnumU8Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static ulong PositiveRem2()
	{
		return (ulong)EnumU8Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static ulong PositiveAssignPlusOne()
	{
		var e = EnumU8Type.Positive50;
		e += 1;
		return (ulong)e;
	}

	[MosaUnitTest]
	public static ulong PositiveAssignMinusOne()
	{
		var e = EnumU8Type.Positive50;
		e -= 1;
		return (ulong)e;
	}

	[MosaUnitTest]
	public static ulong PositivePreincrement()
	{
		var e = EnumU8Type.Positive50;
		++e;
		return (ulong)e;
	}

	[MosaUnitTest]
	public static ulong PositivePredecrement()
	{
		var e = EnumU8Type.Positive50;
		--e;
		return (ulong)e;
	}

	[MosaUnitTest]
	public static ulong PositivePostincrement()
	{
		var e = EnumU8Type.Positive50;
		e++;
		return (ulong)e;
	}

	[MosaUnitTest]
	public static ulong PositivePostdecrement()
	{
		var e = EnumU8Type.Positive50;
		e--;
		return (ulong)e;
	}

	[MosaUnitTest]
	public static ulong PositiveAnd()
	{
		return (ulong)EnumU8Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static ulong PositiveOr()
	{
		return (ulong)EnumU8Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static ulong PositiveXOr()
	{
		return (ulong)EnumU8Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumU8Type e, ulong v)
	{
		return (ulong)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumU8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumU8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumU8Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumU8Type e, ulong v)
	{
		return (ulong)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumU8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumU8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumU8Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumU8Type e, ulong v)
	{
		return (ulong)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumU8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumU8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumU8Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumU8Type e, ulong v)
	{
		return (ulong)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumU8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumU8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumU8Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumU8Type e, ulong v)
	{
		return (ulong)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumU8Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumU8Type e, ulong v)
	{
		return (ulong)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumU8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumU8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumU8Type.Positive50, 49);
	}

}

enum EnumI1Type : sbyte
{
	Negative51 = -51,
	Negative50,
	Negative49,
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumI1
{
	[MosaUnitTest]
	public static sbyte PositiveConversion()
	{
		return (sbyte)EnumI1Type.Positive50;
	}

	[MosaUnitTest]
	public static sbyte PositivePlusOne1()
	{
		return (sbyte)(EnumI1Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static sbyte PositivePlusOne2()
	{
		return (sbyte)(EnumI1Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static sbyte PositiveMinusOne1()
	{
		return (sbyte)(EnumI1Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static sbyte PositiveMinusOne2()
	{
		return (sbyte)(EnumI1Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static sbyte PositiveShl()
	{
		return (sbyte)EnumI1Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static sbyte PositiveShr()
	{
		return (sbyte)EnumI1Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static sbyte PositiveMul2()
	{
		return (sbyte)EnumI1Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static sbyte PositiveDiv2()
	{
		return (sbyte)EnumI1Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static sbyte PositiveRem2()
	{
		return (sbyte)EnumI1Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static sbyte PositiveAssignPlusOne()
	{
		var e = EnumI1Type.Positive50;
		e += 1;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte PositiveAssignMinusOne()
	{
		var e = EnumI1Type.Positive50;
		e -= 1;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte PositivePreincrement()
	{
		var e = EnumI1Type.Positive50;
		++e;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte PositivePredecrement()
	{
		var e = EnumI1Type.Positive50;
		--e;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte PositivePostincrement()
	{
		var e = EnumI1Type.Positive50;
		e++;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte PositivePostdecrement()
	{
		var e = EnumI1Type.Positive50;
		e--;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte PositiveAnd()
	{
		return (sbyte)EnumI1Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static sbyte PositiveOr()
	{
		return (sbyte)EnumI1Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static sbyte PositiveXOr()
	{
		return (sbyte)EnumI1Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumI1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumI1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumI1Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumI1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumI1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumI1Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumI1Type e, sbyte v)
	{
		return (sbyte)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumI1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumI1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumI1Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumI1Type e, sbyte v)
	{
		return (sbyte)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumI1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumI1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumI1Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI1Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumI1Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumI1Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumI1Type.Positive50, 49);
	}

	[MosaUnitTest]
	public static sbyte NegativeConversion()
	{
		return (sbyte)EnumI1Type.Negative50;
	}

	[MosaUnitTest]
	public static sbyte NegativePlusOne1()
	{
		return (sbyte)(EnumI1Type.Negative50 + 1);
	}

	[MosaUnitTest]
	public static sbyte NegativePlusOne2()
	{
		return (sbyte)(EnumI1Type.Negative50 + 2);
	}

	[MosaUnitTest]
	public static sbyte NegativeMinusOne1()
	{
		return (sbyte)(EnumI1Type.Negative50 - 1);
	}

	[MosaUnitTest]
	public static sbyte NegativeMinusOne2()
	{
		return (sbyte)(EnumI1Type.Negative50 - 2);
	}

	[MosaUnitTest]
	public static sbyte NegativeShl()
	{
		return (sbyte)EnumI1Type.Negative50 << 1;
	}

	[MosaUnitTest]
	public static sbyte NegativeShr()
	{
		return (sbyte)EnumI1Type.Negative50 >> 1;
	}

	[MosaUnitTest]
	public static sbyte NegativeMul2()
	{
		return (sbyte)EnumI1Type.Negative50 * 2;
	}

	[MosaUnitTest]
	public static sbyte NegativeDiv2()
	{
		return (sbyte)EnumI1Type.Negative50 / 2;
	}

	[MosaUnitTest]
	public static sbyte NegativeRem2()
	{
		return (sbyte)EnumI1Type.Negative50 % 2;
	}

	[MosaUnitTest]
	public static sbyte NegativeAssignPlusOne()
	{
		var e = EnumI1Type.Negative50;
		e += 1;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte NegativeAssignMinusOne()
	{
		var e = EnumI1Type.Negative50;
		e -= 1;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte NegativePreincrement()
	{
		var e = EnumI1Type.Negative50;
		++e;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte NegativePredecrement()
	{
		var e = EnumI1Type.Negative50;
		--e;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte NegativePostincrement()
	{
		var e = EnumI1Type.Negative50;
		e++;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte NegativePostdecrement()
	{
		var e = EnumI1Type.Negative50;
		e--;
		return (sbyte)e;
	}

	[MosaUnitTest]
	public static sbyte NegativeAnd()
	{
		return (sbyte)EnumI1Type.Negative50 & 0xF;
	}

	[MosaUnitTest]
	public static sbyte NegativeOr()
	{
		return (sbyte)EnumI1Type.Negative50 | 1;
	}

	[MosaUnitTest]
	public static sbyte NegativeXOr()
	{
		return (sbyte)EnumI1Type.Negative50 ^ 1;
	}

	private static bool InternalNegativeEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e == v;
	}

	[MosaUnitTest]
	public static bool NegativeEqual1()
	{
		return InternalNegativeEqual(EnumI1Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeEqual2()
	{
		return InternalNegativeEqual(EnumI1Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeEqual3()
	{
		return InternalNegativeEqual(EnumI1Type.Negative50, -51);
	}

	private static bool InternalNegativeNotEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e != v;
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual1()
	{
		return InternalNegativeNotEqual(EnumI1Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual2()
	{
		return InternalNegativeNotEqual(EnumI1Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual3()
	{
		return InternalNegativeNotEqual(EnumI1Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThan(EnumI1Type e, sbyte v)
	{
		return (sbyte)e > v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan1()
	{
		return InternalNegativeGreaterThan(EnumI1Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan2()
	{
		return InternalNegativeGreaterThan(EnumI1Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan3()
	{
		return InternalNegativeGreaterThan(EnumI1Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThan(EnumI1Type e, sbyte v)
	{
		return (sbyte)e < v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThan1()
	{
		return InternalNegativeLessThan(EnumI1Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan2()
	{
		return InternalNegativeLessThan(EnumI1Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan3()
	{
		return InternalNegativeLessThan(EnumI1Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThanOrEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e >= v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual1()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI1Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual2()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI1Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual3()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI1Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThanOrEqual(EnumI1Type e, sbyte v)
	{
		return (sbyte)e <= v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual1()
	{
		return InternalNegativeLessThanOrEqual(EnumI1Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual2()
	{
		return InternalNegativeLessThanOrEqual(EnumI1Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual3()
	{
		return InternalNegativeLessThanOrEqual(EnumI1Type.Negative50, -51);
	}

}

enum EnumI2Type : short
{
	Negative51 = -51,
	Negative50,
	Negative49,
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumI2
{
	[MosaUnitTest]
	public static short PositiveConversion()
	{
		return (short)EnumI2Type.Positive50;
	}

	[MosaUnitTest]
	public static short PositivePlusOne1()
	{
		return (short)(EnumI2Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static short PositivePlusOne2()
	{
		return (short)(EnumI2Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static short PositiveMinusOne1()
	{
		return (short)(EnumI2Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static short PositiveMinusOne2()
	{
		return (short)(EnumI2Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static short PositiveShl()
	{
		return (short)EnumI2Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static short PositiveShr()
	{
		return (short)EnumI2Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static short PositiveMul2()
	{
		return (short)EnumI2Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static short PositiveDiv2()
	{
		return (short)EnumI2Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static short PositiveRem2()
	{
		return (short)EnumI2Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static short PositiveAssignPlusOne()
	{
		var e = EnumI2Type.Positive50;
		e += 1;
		return (short)e;
	}

	[MosaUnitTest]
	public static short PositiveAssignMinusOne()
	{
		var e = EnumI2Type.Positive50;
		e -= 1;
		return (short)e;
	}

	[MosaUnitTest]
	public static short PositivePreincrement()
	{
		var e = EnumI2Type.Positive50;
		++e;
		return (short)e;
	}

	[MosaUnitTest]
	public static short PositivePredecrement()
	{
		var e = EnumI2Type.Positive50;
		--e;
		return (short)e;
	}

	[MosaUnitTest]
	public static short PositivePostincrement()
	{
		var e = EnumI2Type.Positive50;
		e++;
		return (short)e;
	}

	[MosaUnitTest]
	public static short PositivePostdecrement()
	{
		var e = EnumI2Type.Positive50;
		e--;
		return (short)e;
	}

	[MosaUnitTest]
	public static short PositiveAnd()
	{
		return (short)EnumI2Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static short PositiveOr()
	{
		return (short)EnumI2Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static short PositiveXOr()
	{
		return (short)EnumI2Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumI2Type e, short v)
	{
		return (short)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumI2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumI2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumI2Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumI2Type e, short v)
	{
		return (short)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumI2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumI2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumI2Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumI2Type e, short v)
	{
		return (short)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumI2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumI2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumI2Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumI2Type e, short v)
	{
		return (short)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumI2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumI2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumI2Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumI2Type e, short v)
	{
		return (short)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI2Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumI2Type e, short v)
	{
		return (short)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumI2Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumI2Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumI2Type.Positive50, 49);
	}

	[MosaUnitTest]
	public static short NegativeConversion()
	{
		return (short)EnumI2Type.Negative50;
	}

	[MosaUnitTest]
	public static short NegativePlusOne1()
	{
		return (short)(EnumI2Type.Negative50 + 1);
	}

	[MosaUnitTest]
	public static short NegativePlusOne2()
	{
		return (short)(EnumI2Type.Negative50 + 2);
	}

	[MosaUnitTest]
	public static short NegativeMinusOne1()
	{
		return (short)(EnumI2Type.Negative50 - 1);
	}

	[MosaUnitTest]
	public static short NegativeMinusOne2()
	{
		return (short)(EnumI2Type.Negative50 - 2);
	}

	[MosaUnitTest]
	public static short NegativeShl()
	{
		return (short)EnumI2Type.Negative50 << 1;
	}

	[MosaUnitTest]
	public static short NegativeShr()
	{
		return (short)EnumI2Type.Negative50 >> 1;
	}

	[MosaUnitTest]
	public static short NegativeMul2()
	{
		return (short)EnumI2Type.Negative50 * 2;
	}

	[MosaUnitTest]
	public static short NegativeDiv2()
	{
		return (short)EnumI2Type.Negative50 / 2;
	}

	[MosaUnitTest]
	public static short NegativeRem2()
	{
		return (short)EnumI2Type.Negative50 % 2;
	}

	[MosaUnitTest]
	public static short NegativeAssignPlusOne()
	{
		var e = EnumI2Type.Negative50;
		e += 1;
		return (short)e;
	}

	[MosaUnitTest]
	public static short NegativeAssignMinusOne()
	{
		var e = EnumI2Type.Negative50;
		e -= 1;
		return (short)e;
	}

	[MosaUnitTest]
	public static short NegativePreincrement()
	{
		var e = EnumI2Type.Negative50;
		++e;
		return (short)e;
	}

	[MosaUnitTest]
	public static short NegativePredecrement()
	{
		var e = EnumI2Type.Negative50;
		--e;
		return (short)e;
	}

	[MosaUnitTest]
	public static short NegativePostincrement()
	{
		var e = EnumI2Type.Negative50;
		e++;
		return (short)e;
	}

	[MosaUnitTest]
	public static short NegativePostdecrement()
	{
		var e = EnumI2Type.Negative50;
		e--;
		return (short)e;
	}

	[MosaUnitTest]
	public static short NegativeAnd()
	{
		return (short)EnumI2Type.Negative50 & 0xF;
	}

	[MosaUnitTest]
	public static short NegativeOr()
	{
		return (short)EnumI2Type.Negative50 | 1;
	}

	[MosaUnitTest]
	public static short NegativeXOr()
	{
		return (short)EnumI2Type.Negative50 ^ 1;
	}

	private static bool InternalNegativeEqual(EnumI2Type e, short v)
	{
		return (short)e == v;
	}

	[MosaUnitTest]
	public static bool NegativeEqual1()
	{
		return InternalNegativeEqual(EnumI2Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeEqual2()
	{
		return InternalNegativeEqual(EnumI2Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeEqual3()
	{
		return InternalNegativeEqual(EnumI2Type.Negative50, -51);
	}

	private static bool InternalNegativeNotEqual(EnumI2Type e, short v)
	{
		return (short)e != v;
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual1()
	{
		return InternalNegativeNotEqual(EnumI2Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual2()
	{
		return InternalNegativeNotEqual(EnumI2Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual3()
	{
		return InternalNegativeNotEqual(EnumI2Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThan(EnumI2Type e, short v)
	{
		return (short)e > v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan1()
	{
		return InternalNegativeGreaterThan(EnumI2Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan2()
	{
		return InternalNegativeGreaterThan(EnumI2Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan3()
	{
		return InternalNegativeGreaterThan(EnumI2Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThan(EnumI2Type e, short v)
	{
		return (short)e < v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThan1()
	{
		return InternalNegativeLessThan(EnumI2Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan2()
	{
		return InternalNegativeLessThan(EnumI2Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan3()
	{
		return InternalNegativeLessThan(EnumI2Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThanOrEqual(EnumI2Type e, short v)
	{
		return (short)e >= v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual1()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI2Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual2()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI2Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual3()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI2Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThanOrEqual(EnumI2Type e, short v)
	{
		return (short)e <= v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual1()
	{
		return InternalNegativeLessThanOrEqual(EnumI2Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual2()
	{
		return InternalNegativeLessThanOrEqual(EnumI2Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual3()
	{
		return InternalNegativeLessThanOrEqual(EnumI2Type.Negative50, -51);
	}

}

enum EnumI4Type : int
{
	Negative51 = -51,
	Negative50,
	Negative49,
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumI4
{
	[MosaUnitTest]
	public static int PositiveConversion()
	{
		return (int)EnumI4Type.Positive50;
	}

	[MosaUnitTest]
	public static int PositivePlusOne1()
	{
		return (int)(EnumI4Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static int PositivePlusOne2()
	{
		return (int)(EnumI4Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static int PositiveMinusOne1()
	{
		return (int)(EnumI4Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static int PositiveMinusOne2()
	{
		return (int)(EnumI4Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static int PositiveShl()
	{
		return (int)EnumI4Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static int PositiveShr()
	{
		return (int)EnumI4Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static int PositiveMul2()
	{
		return (int)EnumI4Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static int PositiveDiv2()
	{
		return (int)EnumI4Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static int PositiveRem2()
	{
		return (int)EnumI4Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static int PositiveAssignPlusOne()
	{
		var e = EnumI4Type.Positive50;
		e += 1;
		return (int)e;
	}

	[MosaUnitTest]
	public static int PositiveAssignMinusOne()
	{
		var e = EnumI4Type.Positive50;
		e -= 1;
		return (int)e;
	}

	[MosaUnitTest]
	public static int PositivePreincrement()
	{
		var e = EnumI4Type.Positive50;
		++e;
		return (int)e;
	}

	[MosaUnitTest]
	public static int PositivePredecrement()
	{
		var e = EnumI4Type.Positive50;
		--e;
		return (int)e;
	}

	[MosaUnitTest]
	public static int PositivePostincrement()
	{
		var e = EnumI4Type.Positive50;
		e++;
		return (int)e;
	}

	[MosaUnitTest]
	public static int PositivePostdecrement()
	{
		var e = EnumI4Type.Positive50;
		e--;
		return (int)e;
	}

	[MosaUnitTest]
	public static int PositiveAnd()
	{
		return (int)EnumI4Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static int PositiveOr()
	{
		return (int)EnumI4Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static int PositiveXOr()
	{
		return (int)EnumI4Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumI4Type e, int v)
	{
		return (int)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumI4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumI4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumI4Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumI4Type e, int v)
	{
		return (int)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumI4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumI4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumI4Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumI4Type e, int v)
	{
		return (int)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumI4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumI4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumI4Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumI4Type e, int v)
	{
		return (int)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumI4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumI4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumI4Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumI4Type e, int v)
	{
		return (int)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI4Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumI4Type e, int v)
	{
		return (int)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumI4Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumI4Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumI4Type.Positive50, 49);
	}

	[MosaUnitTest]
	public static int NegativeConversion()
	{
		return (int)EnumI4Type.Negative50;
	}

	[MosaUnitTest]
	public static int NegativePlusOne1()
	{
		return (int)(EnumI4Type.Negative50 + 1);
	}

	[MosaUnitTest]
	public static int NegativePlusOne2()
	{
		return (int)(EnumI4Type.Negative50 + 2);
	}

	[MosaUnitTest]
	public static int NegativeMinusOne1()
	{
		return (int)(EnumI4Type.Negative50 - 1);
	}

	[MosaUnitTest]
	public static int NegativeMinusOne2()
	{
		return (int)(EnumI4Type.Negative50 - 2);
	}

	[MosaUnitTest]
	public static int NegativeShl()
	{
		return (int)EnumI4Type.Negative50 << 1;
	}

	[MosaUnitTest]
	public static int NegativeShr()
	{
		return (int)EnumI4Type.Negative50 >> 1;
	}

	[MosaUnitTest]
	public static int NegativeMul2()
	{
		return (int)EnumI4Type.Negative50 * 2;
	}

	[MosaUnitTest]
	public static int NegativeDiv2()
	{
		return (int)EnumI4Type.Negative50 / 2;
	}

	[MosaUnitTest]
	public static int NegativeRem2()
	{
		return (int)EnumI4Type.Negative50 % 2;
	}

	[MosaUnitTest]
	public static int NegativeAssignPlusOne()
	{
		var e = EnumI4Type.Negative50;
		e += 1;
		return (int)e;
	}

	[MosaUnitTest]
	public static int NegativeAssignMinusOne()
	{
		var e = EnumI4Type.Negative50;
		e -= 1;
		return (int)e;
	}

	[MosaUnitTest]
	public static int NegativePreincrement()
	{
		var e = EnumI4Type.Negative50;
		++e;
		return (int)e;
	}

	[MosaUnitTest]
	public static int NegativePredecrement()
	{
		var e = EnumI4Type.Negative50;
		--e;
		return (int)e;
	}

	[MosaUnitTest]
	public static int NegativePostincrement()
	{
		var e = EnumI4Type.Negative50;
		e++;
		return (int)e;
	}

	[MosaUnitTest]
	public static int NegativePostdecrement()
	{
		var e = EnumI4Type.Negative50;
		e--;
		return (int)e;
	}

	[MosaUnitTest]
	public static int NegativeAnd()
	{
		return (int)EnumI4Type.Negative50 & 0xF;
	}

	[MosaUnitTest]
	public static int NegativeOr()
	{
		return (int)EnumI4Type.Negative50 | 1;
	}

	[MosaUnitTest]
	public static int NegativeXOr()
	{
		return (int)EnumI4Type.Negative50 ^ 1;
	}

	private static bool InternalNegativeEqual(EnumI4Type e, int v)
	{
		return (int)e == v;
	}

	[MosaUnitTest]
	public static bool NegativeEqual1()
	{
		return InternalNegativeEqual(EnumI4Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeEqual2()
	{
		return InternalNegativeEqual(EnumI4Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeEqual3()
	{
		return InternalNegativeEqual(EnumI4Type.Negative50, -51);
	}

	private static bool InternalNegativeNotEqual(EnumI4Type e, int v)
	{
		return (int)e != v;
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual1()
	{
		return InternalNegativeNotEqual(EnumI4Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual2()
	{
		return InternalNegativeNotEqual(EnumI4Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual3()
	{
		return InternalNegativeNotEqual(EnumI4Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThan(EnumI4Type e, int v)
	{
		return (int)e > v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan1()
	{
		return InternalNegativeGreaterThan(EnumI4Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan2()
	{
		return InternalNegativeGreaterThan(EnumI4Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan3()
	{
		return InternalNegativeGreaterThan(EnumI4Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThan(EnumI4Type e, int v)
	{
		return (int)e < v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThan1()
	{
		return InternalNegativeLessThan(EnumI4Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan2()
	{
		return InternalNegativeLessThan(EnumI4Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan3()
	{
		return InternalNegativeLessThan(EnumI4Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThanOrEqual(EnumI4Type e, int v)
	{
		return (int)e >= v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual1()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI4Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual2()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI4Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual3()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI4Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThanOrEqual(EnumI4Type e, int v)
	{
		return (int)e <= v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual1()
	{
		return InternalNegativeLessThanOrEqual(EnumI4Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual2()
	{
		return InternalNegativeLessThanOrEqual(EnumI4Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual3()
	{
		return InternalNegativeLessThanOrEqual(EnumI4Type.Negative50, -51);
	}

}

enum EnumI8Type : long
{
	Negative51 = -51,
	Negative50,
	Negative49,
	Positive49 = 49,
	Positive50,
	Positive51
}

public static class EnumI8
{
	[MosaUnitTest]
	public static long PositiveConversion()
	{
		return (long)EnumI8Type.Positive50;
	}

	[MosaUnitTest]
	public static long PositivePlusOne1()
	{
		return (long)(EnumI8Type.Positive50 + 1);
	}

	[MosaUnitTest]
	public static long PositivePlusOne2()
	{
		return (long)(EnumI8Type.Positive50 + 2);
	}

	[MosaUnitTest]
	public static long PositiveMinusOne1()
	{
		return (long)(EnumI8Type.Positive50 - 1);
	}

	[MosaUnitTest]
	public static long PositiveMinusOne2()
	{
		return (long)(EnumI8Type.Positive50 - 2);
	}

	[MosaUnitTest]
	public static long PositiveShl()
	{
		return (long)EnumI8Type.Positive50 << 1;
	}

	[MosaUnitTest]
	public static long PositiveShr()
	{
		return (long)EnumI8Type.Positive50 >> 1;
	}

	[MosaUnitTest]
	public static long PositiveMul2()
	{
		return (long)EnumI8Type.Positive50 * 2;
	}

	[MosaUnitTest]
	public static long PositiveDiv2()
	{
		return (long)EnumI8Type.Positive50 / 2;
	}

	[MosaUnitTest]
	public static long PositiveRem2()
	{
		return (long)EnumI8Type.Positive50 % 2;
	}

	[MosaUnitTest]
	public static long PositiveAssignPlusOne()
	{
		var e = EnumI8Type.Positive50;
		e += 1;
		return (long)e;
	}

	[MosaUnitTest]
	public static long PositiveAssignMinusOne()
	{
		var e = EnumI8Type.Positive50;
		e -= 1;
		return (long)e;
	}

	[MosaUnitTest]
	public static long PositivePreincrement()
	{
		var e = EnumI8Type.Positive50;
		++e;
		return (long)e;
	}

	[MosaUnitTest]
	public static long PositivePredecrement()
	{
		var e = EnumI8Type.Positive50;
		--e;
		return (long)e;
	}

	[MosaUnitTest]
	public static long PositivePostincrement()
	{
		var e = EnumI8Type.Positive50;
		e++;
		return (long)e;
	}

	[MosaUnitTest]
	public static long PositivePostdecrement()
	{
		var e = EnumI8Type.Positive50;
		e--;
		return (long)e;
	}

	[MosaUnitTest]
	public static long PositiveAnd()
	{
		return (long)EnumI8Type.Positive50 & 0xF;
	}

	[MosaUnitTest]
	public static long PositiveOr()
	{
		return (long)EnumI8Type.Positive50 | 1;
	}

	[MosaUnitTest]
	public static long PositiveXOr()
	{
		return (long)EnumI8Type.Positive50 ^ 1;
	}

	private static bool InternalPositiveEqual(EnumI8Type e, long v)
	{
		return (long)e == v;
	}

	[MosaUnitTest]
	public static bool PositiveEqual1()
	{
		return InternalPositiveEqual(EnumI8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveEqual2()
	{
		return InternalPositiveEqual(EnumI8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveEqual3()
	{
		return InternalPositiveEqual(EnumI8Type.Positive50, 49);
	}

	private static bool InternalPositiveNotEqual(EnumI8Type e, long v)
	{
		return (long)e != v;
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual1()
	{
		return InternalPositiveNotEqual(EnumI8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual2()
	{
		return InternalPositiveNotEqual(EnumI8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveNotEqual3()
	{
		return InternalPositiveNotEqual(EnumI8Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThan(EnumI8Type e, long v)
	{
		return (long)e > v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan1()
	{
		return InternalPositiveGreaterThan(EnumI8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan2()
	{
		return InternalPositiveGreaterThan(EnumI8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThan3()
	{
		return InternalPositiveGreaterThan(EnumI8Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThan(EnumI8Type e, long v)
	{
		return (long)e < v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThan1()
	{
		return InternalPositiveLessThan(EnumI8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan2()
	{
		return InternalPositiveLessThan(EnumI8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThan3()
	{
		return InternalPositiveLessThan(EnumI8Type.Positive50, 49);
	}

	private static bool InternalPositiveGreaterThanOrEqual(EnumI8Type e, long v)
	{
		return (long)e >= v;
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual1()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual2()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveGreaterThanOrEqual3()
	{
		return InternalPositiveGreaterThanOrEqual(EnumI8Type.Positive50, 49);
	}

	private static bool InternalPositiveLessThanOrEqual(EnumI8Type e, long v)
	{
		return (long)e <= v;
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual1()
	{
		return InternalPositiveLessThanOrEqual(EnumI8Type.Positive50, 50);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual2()
	{
		return InternalPositiveLessThanOrEqual(EnumI8Type.Positive50, 51);
	}

	[MosaUnitTest]
	public static bool PositiveLessThanOrEqual3()
	{
		return InternalPositiveLessThanOrEqual(EnumI8Type.Positive50, 49);
	}

	[MosaUnitTest]
	public static long NegativeConversion()
	{
		return (long)EnumI8Type.Negative50;
	}

	[MosaUnitTest]
	public static long NegativePlusOne1()
	{
		return (long)(EnumI8Type.Negative50 + 1);
	}

	[MosaUnitTest]
	public static long NegativePlusOne2()
	{
		return (long)(EnumI8Type.Negative50 + 2);
	}

	[MosaUnitTest]
	public static long NegativeMinusOne1()
	{
		return (long)(EnumI8Type.Negative50 - 1);
	}

	[MosaUnitTest]
	public static long NegativeMinusOne2()
	{
		return (long)(EnumI8Type.Negative50 - 2);
	}

	[MosaUnitTest]
	public static long NegativeShl()
	{
		return (long)EnumI8Type.Negative50 << 1;
	}

	[MosaUnitTest]
	public static long NegativeShr()
	{
		return (long)EnumI8Type.Negative50 >> 1;
	}

	[MosaUnitTest]
	public static long NegativeMul2()
	{
		return (long)EnumI8Type.Negative50 * 2;
	}

	[MosaUnitTest]
	public static long NegativeDiv2()
	{
		return (long)EnumI8Type.Negative50 / 2;
	}

	[MosaUnitTest]
	public static long NegativeRem2()
	{
		return (long)EnumI8Type.Negative50 % 2;
	}

	[MosaUnitTest]
	public static long NegativeAssignPlusOne()
	{
		var e = EnumI8Type.Negative50;
		e += 1;
		return (long)e;
	}

	[MosaUnitTest]
	public static long NegativeAssignMinusOne()
	{
		var e = EnumI8Type.Negative50;
		e -= 1;
		return (long)e;
	}

	[MosaUnitTest]
	public static long NegativePreincrement()
	{
		var e = EnumI8Type.Negative50;
		++e;
		return (long)e;
	}

	[MosaUnitTest]
	public static long NegativePredecrement()
	{
		var e = EnumI8Type.Negative50;
		--e;
		return (long)e;
	}

	[MosaUnitTest]
	public static long NegativePostincrement()
	{
		var e = EnumI8Type.Negative50;
		e++;
		return (long)e;
	}

	[MosaUnitTest]
	public static long NegativePostdecrement()
	{
		var e = EnumI8Type.Negative50;
		e--;
		return (long)e;
	}

	[MosaUnitTest]
	public static long NegativeAnd()
	{
		return (long)EnumI8Type.Negative50 & 0xF;
	}

	[MosaUnitTest]
	public static long NegativeOr()
	{
		return (long)EnumI8Type.Negative50 | 1;
	}

	[MosaUnitTest]
	public static long NegativeXOr()
	{
		return (long)EnumI8Type.Negative50 ^ 1;
	}

	private static bool InternalNegativeEqual(EnumI8Type e, long v)
	{
		return (long)e == v;
	}

	[MosaUnitTest]
	public static bool NegativeEqual1()
	{
		return InternalNegativeEqual(EnumI8Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeEqual2()
	{
		return InternalNegativeEqual(EnumI8Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeEqual3()
	{
		return InternalNegativeEqual(EnumI8Type.Negative50, -51);
	}

	private static bool InternalNegativeNotEqual(EnumI8Type e, long v)
	{
		return (long)e != v;
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual1()
	{
		return InternalNegativeNotEqual(EnumI8Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual2()
	{
		return InternalNegativeNotEqual(EnumI8Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeNotEqual3()
	{
		return InternalNegativeNotEqual(EnumI8Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThan(EnumI8Type e, long v)
	{
		return (long)e > v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan1()
	{
		return InternalNegativeGreaterThan(EnumI8Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan2()
	{
		return InternalNegativeGreaterThan(EnumI8Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThan3()
	{
		return InternalNegativeGreaterThan(EnumI8Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThan(EnumI8Type e, long v)
	{
		return (long)e < v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThan1()
	{
		return InternalNegativeLessThan(EnumI8Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan2()
	{
		return InternalNegativeLessThan(EnumI8Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThan3()
	{
		return InternalNegativeLessThan(EnumI8Type.Negative50, -51);
	}

	private static bool InternalNegativeGreaterThanOrEqual(EnumI8Type e, long v)
	{
		return (long)e >= v;
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual1()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI8Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual2()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI8Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeGreaterThanOrEqual3()
	{
		return InternalNegativeGreaterThanOrEqual(EnumI8Type.Negative50, -51);
	}

	private static bool InternalNegativeLessThanOrEqual(EnumI8Type e, long v)
	{
		return (long)e <= v;
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual1()
	{
		return InternalNegativeLessThanOrEqual(EnumI8Type.Negative50, -50);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual2()
	{
		return InternalNegativeLessThanOrEqual(EnumI8Type.Negative50, -49);
	}

	[MosaUnitTest]
	public static bool NegativeLessThanOrEqual3()
	{
		return InternalNegativeLessThanOrEqual(EnumI8Type.Negative50, -51);
	}

}
