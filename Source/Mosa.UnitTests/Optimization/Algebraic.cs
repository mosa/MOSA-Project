// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class Algebraic
{
	[MosaUnitTest(Series = "I4I4")]
	public static int AAPlusBBPlus2AB(int a, int b)
	{
		return a * a + b * b + 2 * a * b;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int AAPlusBBMinus2AB(int a, int b)
	{
		return a * a + b * b - 2 * a * b;
	}

	[MosaUnitTest(Series = "I4")]
	public static int PerfectSquareFormula1(int x)
	{
		return x * x + 10 * x + 25;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint PerfectSquareFormula2(uint x)
	{
		return x * x + 10 * x + 25;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint PerfectSquareFormula3(uint x)
	{
		return x * x - 4 * x - 16;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint PerfectSquareFormula4(uint x)
	{
		return x * x - 5 * x - 25;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int AAMinusBB(int a, int b)
	{
		return a * a - b * b;
	}

	[MosaUnitTest(Series = "U4U4")]
	public static uint AAMinusBB2(uint a, uint b)
	{
		return a * a - b * b;
	}

	[MosaUnitTest(Series = "U4U4")]
	public static uint AAAPlus3AABPlus3ABBPlusBBB(uint a, uint b)
	{
		return a * a * a + 3 * a * a * b + 3 * a * b * b + b * b * b;
	}
}
