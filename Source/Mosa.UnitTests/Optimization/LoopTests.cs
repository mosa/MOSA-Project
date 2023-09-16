// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Optimization;

public static class LoopTests
{
	private static uint[] array = { 1, 2, 3, 4 };

	[MosaUnitTest]
	public static uint Loop0()
	{
		var x = 0u;

		for (uint i = 0; i < 4; i++)
		{
			x += 2;
		}

		return x;
	}

	[MosaUnitTest]
	public static uint Loop1()
	{
		var x = 0u;

		for (uint i = 0; i < 4; i++)
		{
			x += i * 4 + 4;
		}

		return x;
	}

	[MosaUnitTest]
	public static uint Loop2()
	{
		var x = 0u;

		for (uint i = 0; i < 4; i++)
		{
			x += array[i];
		}

		return x;
	}
}
