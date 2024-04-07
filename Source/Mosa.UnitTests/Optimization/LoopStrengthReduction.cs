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
			a++;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction4a()
	{
		var a = 8;
		var n = 10;

		for (var i = n; i > 0; i--)
		{
			a++;

			if (i > 11)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction4b()
	{
		var a = 8;
		var n = 10;

		for (var i = n; i > 3; i--)
		{
			a++;

			if (i < 1)
				return 0;
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
			a += i;

			if (i > 20)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6a()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i < n; i++)
		{
			a += i;

			if (i > 9)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6b()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i < n; i++)
		{
			a += i;

			if (i > 10)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6c()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i < n; i++)
		{
			a += i;

			if (i > 11)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6d()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i != n; i++)
		{
			a += i;

			if (i > 9)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6e()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i != n; i++)
		{
			a += i;

			if (i > 10)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int Reduction6f()
	{
		var a = 8;
		var n = 10;

		for (var i = 0; i != n; i++)
		{
			a += i;

			if (i > 11)
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
