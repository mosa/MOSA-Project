// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.FlowControl;

public static class SwitchTests
{
	[MosaUnitTest(Series = "I1")]
	public static sbyte SwitchI1(sbyte a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			-1 => -1,
			2 => 2,
			-2 => -2,
			23 => 23,
			sbyte.MinValue => sbyte.MinValue,
			sbyte.MaxValue => sbyte.MaxValue,
			_ => 42
		};
	}
	[MosaUnitTest(Series = "I2")]
	public static short SwitchI2(short a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			-1 => -1,
			2 => 2,
			-2 => -2,
			23 => 23,
			short.MinValue => short.MinValue,
			short.MaxValue => short.MaxValue,
			_ => 42
		};
	}
	[MosaUnitTest(Series = "I4")]
	public static int SwitchI4(int a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			-1 => -1,
			2 => 2,
			-2 => -2,
			23 => 23,
			int.MinValue => int.MinValue,
			int.MaxValue => int.MaxValue,
			_ => 42
		};
	}
	[MosaUnitTest(Series = "I8")]
	public static long SwitchI8(long a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			-1 => -1,
			2 => 2,
			-2 => -2,
			23 => 23,
			long.MinValue => long.MinValue,
			long.MaxValue => long.MaxValue,
			_ => 42
		};
	}

	[MosaUnitTest(Series = "U1")]
	public static byte SwitchU1(byte a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			2 => 2,
			23 => 23,
			byte.MaxValue => byte.MaxValue,
			_ => 42
		};
	}

	[MosaUnitTest(Series = "U2")]
	public static ushort SwitchU2(ushort a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			2 => 2,
			23 => 23,
			ushort.MaxValue => ushort.MaxValue,
			_ => 42
		};
	}

	[MosaUnitTest(Series = "U4")]
	public static uint SwitchU4(uint a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			2 => 2,
			23 => 23,
			uint.MaxValue => uint.MaxValue,
			_ => 42
		};
	}

	[MosaUnitTest(Series = "U8")]
	public static ulong SwitchU8(ulong a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			2 => 2,
			23 => 23,
			ulong.MaxValue => ulong.MaxValue,
			_ => 42
		};
	}
}
