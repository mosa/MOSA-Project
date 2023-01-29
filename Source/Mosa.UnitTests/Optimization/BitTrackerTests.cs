// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class BitTrackerTests
{
	[MosaUnitTest(Series = "I4")]
	public static bool BitTrackerTest1(int x)
	{
		var a = x | 0x3;    //  3 = 0b000011
		return a == 0;      //  false - can't be zero
	}

	[MosaUnitTest(Series = "I4")]
	public static bool BitTrackerTest2(int x)
	{
		var a = x | 0x3;    //  3 = 0b000011
		return a != 0;      //  true - can't be zero
	}

	[MosaUnitTest(Series = "I4")]
	public static int BitTrackerTest3(int x)
	{
		var a = x | 0x5;    //  3 = 0b000101
		var b = a & 0x4;    //  4 = b0000100

		return b;           // == 3
	}

	[MosaUnitTest(Series = "U4")]
	public static bool BitTrackerTest4(uint x)
	{
		var a = x & 31;
		var b = a + 1;

		return 32 < b;      // == 0
	}

	[MosaUnitTest(Series = "U4")]
	public static bool BitTrackerTest5(uint x)
	{
		var a = x & 31;
		var b = a + 1;

		return 33 <= b;      // == 0
	}
}
