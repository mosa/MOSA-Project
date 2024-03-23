// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.Optimization;

public static class LoopUnrollingTests
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SimpleUnroll()
	{
		var v = 0;

		for (var i = 0; i < 2; i++)
		{
			v++;
		}

		return v;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SimpleUnroll2(int a)
	{
		var v = 0;

		for (var i = 0; i < 2; i++)
		{
			v += a;
		}

		return v;
	}
}
