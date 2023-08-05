// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Basic;

public static class ConvFloatTests
{
	[MosaUnitTest(Series = "R4")]
	public static double ConvR4R8(float a)
	{
		return a;
	}

	[MosaUnitTest(Series = "R8")]
	public static float ConvR8R4(double a)
	{
		return (float)a;
	}

	[MosaUnitTest(Series = "R8")]
	public static long ConvR8Bits(double a)
	{
		return System.BitConverter.DoubleToInt64Bits(a);
	}

	[MosaUnitTest(Series = "R4")]
	public static long ConvR4I8(float a)
	{
		return (long)a;
	}

	[MosaUnitTest(Series = "R4")]
	public static ulong ConvR4U8(float a)
	{
		return (ulong)a;
	}

	[MosaUnitTest(Series = "R8")]
	public static long ConvR8I8(double a)
	{
		return (long)a;
	}

	[MosaUnitTest(Series = "R8")]
	public static ulong ConvR8U8(double a)
	{
		return (ulong)a;
	}

	public static int ConvR4I4(float a)
	{
		return (int)a;
	}

	//[MosaUnitTest(Series = "R4")]	// BUG
	public static uint ConvR4U4(float a)
	{
		return (uint)a;
	}

	[MosaUnitTest(Series = "R8")]
	public static int ConvR8I4(double a)
	{
		return (int)a;
	}

	//[MosaUnitTest(Series = "R8")]	// BUG
	public static uint ConvR8U4(double a)
	{
		return (uint)a;
	}
}
