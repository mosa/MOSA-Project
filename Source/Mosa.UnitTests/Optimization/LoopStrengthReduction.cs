// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class LoopStrengthReduction
{
	private static int Reduction1(int[] n)
	{
		var a = 0;

		for (var i = 0; i < 100; i++)
		{
			a *= n[i];
		}

		return a;
	}

	private static int Reduction2(int[] n)
	{
		var a = 0;

		for (var i = 0; i < 100; i++)
		{
			a *= n[i];
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction3()
	{
		var a = 8;
		var x = 0;
		var y = 8;
		var z = 1;
		var n = 100;

		for (var i = n; i > 0; i--)
		{
			x = y + z;
			a += x * x;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction4()
	{
		var a = 8;
		var n = 10;

		for (var i = n; i > 0; i--)
		{
			a *= i;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction5()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i < n; i++)
		{
			a *= i;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i < n; i++)
		{
			a *= i;

			if (i > 20)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction7()
	{
		var a = 8;
		var n = 10;

		for (var i = n; i > 0; i--)
		{
			a *= i;

			if (i < 0)
				return 0;
		}

		return a;
	}
}
