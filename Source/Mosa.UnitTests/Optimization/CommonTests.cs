// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class CommonTests
{
	[MosaUnitTest]
	public static bool OptimizationTest1()
	{
		var a = 10;
		var b = 20;

		var c = a + b;

		return c == 30;
	}

	[MosaUnitTest]
	public static bool OptimizationTest2()
	{
		uint a = 10;
		uint b = 20;

		var c = a + b;

		return c == 30;
	}

	[MosaUnitTest]
	public static bool OptimizationTest3()
	{
		byte a = 10;
		uint b = 20;

		var c = a + b;

		return c == 30;
	}

	[MosaUnitTest]
	public static bool OptimizationTest4()
	{
		ulong a = 10;
		ulong b = 20;

		var c = a + b;

		return c == 30;
	}

	[MosaUnitTest]
	public static bool OptimizationTest5()
	{
		var a = ulong.MaxValue;
		ulong b = 20;

		var c = a + b;

		return c == unchecked(ulong.MaxValue + 20);
	}

	[MosaUnitTest]
	public static bool OptimizationTest6()
	{
		var a = (char)10;
		var b = (char)20;

		var c = (char)(a + b);

		return c == 30;
	}

	[MosaUnitTest]
	public static bool OptimizationTest7()
	{
		ulong a = 10;
		ulong b = 0;

		var c = a * b;

		return c == 0;
	}

	[MosaUnitTest]
	public static bool OptimizationTest8()
	{
		var a = 10;
		var b = 0;

		var c = a * b;

		return c == 0;
	}

	[MosaUnitTest]
	public static bool OptimizationTest9()
	{
		ulong a = 10;
		ulong b = 1;

		var c = a * b;

		return c == 10;
	}

	[MosaUnitTest]
	public static bool OptimizationTest10()
	{
		var a = 1;
		var b = 10;

		var c = a * b;

		return c == 10;
	}

	[MosaUnitTest]
	public static bool OptimizationTest11()
	{
		var a = 0;
		var b = 10;

		var c = a * b;

		return c == 0;
	}

	[MosaUnitTest]
	public static int OptimizationTest12()
	{
		var a = 32;
		var b = 10;
		var c = 10;
		var d = 1;
		var e = 0;

		var z = a * b + c * d + c * d * e + e;

		return z;
	}

	[MosaUnitTest(Series = "I4")]
	public static int OptimizationTest13(int q)
	{
		var a = 10;
		var b = 20;

		var c = a + b + q;

		return c;
	}
}
