// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Basic;

public static class CallOrderTests

{
	//[MosaUnitTest]
	public static void CallEmpty()
	{
		CallEmpty_Target();
	}

	private static void CallEmpty_Target()
	{
	}

	[MosaUnitTest(Series = "I4")]
	public static bool CallOrderI4(int a)
	{
		return a == 1;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static bool CallOrderI4I4(int a, int b)
	{
		return a == 1 && b == 2;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int CallOrderI4I4_2(int a, int b)
	{
		return a + b * 10;
	}

	[MosaUnitTest(Series = "U4U4")]
	public static bool CallOrderU4U4(uint a, uint b)
	{
		return a == 1 && b == 2;
	}

	[MosaUnitTest(Series = "U4U4")]
	public static uint CallOrderU4U4_2(uint a, uint b)
	{
		return a + b * 10;
	}

	[MosaUnitTest(Series = "I4MiniI4MiniI4Mini")]
	public static bool CallOrderI4I4I4(int a, int b, int c)
	{
		return a == 1 && b == 2 && c == 3;
	}

	[MosaUnitTest(Series = "I4MiniI4MiniI4MiniI4Mini")]
	public static bool CallOrderI4I4I4I4(int a, int b, int c, int d)
	{
		return a == 1 && b == 2 && c == 3 && d == 4;
	}

	[MosaUnitTest(Series = "I8MiniI8MiniI8MiniI8Mini")]
	public static bool CallOrderI8I8I8I8(long a, long b, long c, long d)
	{
		return a == 1 && b == 2 && c == 3 && d == 4;
	}

	[MosaUnitTest(Series = "U8")]
	public static bool CallOrderU8(ulong a)
	{
		return a == 1;
	}

	[MosaUnitTest(Series = "U8U8")]
	public static bool CallOrderU8U8(ulong a, ulong b)
	{
		return a == 1 && b == 2;
	}

	[MosaUnitTest(Series = "U8MiniU8MiniU8MiniU8Mini")]
	public static bool CallOrderU8U8U8U8(ulong a, ulong b, ulong c, ulong d)
	{
		return a == 1 && b == 2 && c == 3 && d == 4;
	}

	[MosaUnitTest(Series = "U4MiniU8MiniU8MiniU8Mini")]
	public static bool CallOrderU4U8U8U8(uint a, ulong b, ulong c, ulong d)
	{
		return a == 1 && b == 2 && c == 3 && d == 4;
	}

	[MosaUnitTest(Series = "I4MiniI4MiniI4MiniI4Mini")]
	public static int CallOrderI4I4I4I4_2(int a, int b, int c, int d)
	{
		return a + b * 10 + c * 100 + d * 1000;
	}
}
