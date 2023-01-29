// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.Optimization;

public static class LoopUnrollingTests
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SimpleUnroll()
	{
		int v = 0;

		for (int i = 0; i < 2; i++)
		{
			v = v + 1;
		}

		return v;
	}
}
