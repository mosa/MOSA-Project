// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Other;

public static class AnalysisTests

{
	private struct Struct
	{ public int X { get; set; } }

	[MosaUnitTest]
	public static int Slow()
	{
		Struct a = new Struct();

		for (int i = 0; i < 10000; i++)
		{
			a.X += 1;
		}

		return a.X;
	}

	[MosaUnitTest]
	public static int Fast()
	{
		Struct a = new Struct();

		for (int i = 0; i < 10000; i++)
		{
			a.X = a.X + 1;
		}

		return a.X;
	}
}
