// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.Optimization;

public static class LoopStrengthReduction
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int Reduction1(int[] n)
	{
		var a = 0;

		for (var i = 0; i < 100; i++)
		{
			a *= n[i];
		}

		return a;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int Reduction2(int[] n)
	{
		var a = 0;

		for (var i = 0; i < 100; i++)
		{
			a *= n[i];
		}

		return a;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
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

	[MethodImpl(MethodImplOptions.NoInlining)]
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
}
