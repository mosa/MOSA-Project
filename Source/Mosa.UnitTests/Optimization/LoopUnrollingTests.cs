// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class LoopUnrollingTests
{
	[MosaUnitTest]
	public static int SimpleUnroll()
	{
		var v = 0;

		for (var i = 0; i < 2; i++)
		{
			v++;
		}

		return v;
	}

	[MosaUnitTest(Series = "I4")]
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
