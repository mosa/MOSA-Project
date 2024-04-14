// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Basic;

public static class LoopRange
{
	[MosaUnitTest]
	public static int LoopIncreasing1()
	{
		var start = 0;
		var end = 10;
		var exit = 8;
		var a = 0;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing2()
	{
		var start = 0;
		var end = 10;
		var exit = 9;
		var a = 0;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing3()
	{
		var start = 0;
		var end = 10;
		var exit = 10;
		var a = 0;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing4()
	{
		var start = 0;
		var end = 10;
		var exit = 11;
		var a = 0;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing1b()
	{
		var start = 0;
		var end = 10;
		var exit = 8;
		var a = 0;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing2b()
	{
		var start = 0;
		var end = 10;
		var exit = 9;
		var a = 0;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing3b()
	{
		var start = 0;
		var end = 10;
		var exit = 10;
		var a = 0;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopIncreasing4b()
	{
		var start = 0;
		var end = 10;
		var exit = 11;
		var a = 0;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing1u()
	{
		var start = 0u;
		var end = 10u;
		var exit = 8u;
		var a = 0u;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing2u()
	{
		var start = 0u;
		var end = 10u;
		var exit = 9u;
		var a = 0u;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing3u()
	{
		var start = 0u;
		var end = 10u;
		var exit = 10u;
		var a = 0u;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing4u()
	{
		var start = 0u;
		var end = 10u;
		var exit = 11u;
		var a = 0u;

		for (var i = start; i < end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing1bu()
	{
		var start = 0u;
		var end = 10u;
		var exit = 8u;
		var a = 0u;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing2bu()
	{
		var start = 0u;
		var end = 10u;
		var exit = 9u;
		var a = 0u;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing3bu()
	{
		var start = 0u;
		var end = 10u;
		var exit = 10u;
		var a = 0u;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopIncreasing4bu()
	{
		var start = 0u;
		var end = 10u;
		var exit = 11u;
		var a = 0u;

		for (var i = start; i <= end; i++)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing1()
	{
		var start = 10;
		var end = 0;
		var exit = 10;
		var a = 0;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing2()
	{
		var start = 10;
		var end = 0;
		var exit = 9;
		var a = 0;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing3()
	{
		var start = 10;
		var end = 0;
		var exit = 0;
		var a = 0;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing4()
	{
		var start = 10;
		var end = 0;
		var exit = -1;
		var a = 0;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing1b()
	{
		var start = 10;
		var end = 0;
		var exit = 10;
		var a = 0;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing2b()
	{
		var start = 10;
		var end = 0;
		var exit = 9;
		var a = 0;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing3b()
	{
		var start = 10;
		var end = 0;
		var exit = 0;
		var a = 0;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing4b()
	{
		var start = 10;
		var end = 0;
		var exit = -1;
		var a = 0;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing1u()
	{
		var start = 10u;
		var end = 0u;
		var exit = 8u;
		var a = 0u;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing2u()
	{
		var start = 10u;
		var end = 0u;
		var exit = 9u;
		var a = 0u;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing3u()
	{
		var start = 10u;
		var end = 0u;
		var exit = 0u;
		var a = 0u;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing4u()
	{
		var start = 10u;
		var end = 0u;
		var exit = 11u;
		var a = 11u;

		for (var i = start; i > end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing1bu()
	{
		var start = 10u;
		var end = 0u;
		var exit = 10u;
		var a = 0u;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing2bu()
	{
		var start = 10u;
		var end = 0u;
		var exit = 9u;
		var a = 0u;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing3bu()
	{
		var start = 10u;
		var end = 0u;
		var exit = 1u;
		var a = 0u;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing4bu()
	{
		var start = 10u;
		var end = 1u;
		var exit = 11u;
		var a = 0u;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i < exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing3bxx()
	{
		var start = 10;
		var end = 0;
		var exit = 10;
		var a = 0;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static int LoopDecreasing4bxx()
	{
		var start = 10;
		var end = 0;
		var exit = 11;
		var a = 0;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing3buxx()
	{
		var start = 10u;
		var end = 0u;
		var exit = 10u;
		var a = 0u;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}

	[MosaUnitTest]
	public static uint LoopDecreasing4buxx()
	{
		var start = 10u;
		var end = 0u;
		var exit = 11u;
		var a = 0u;

		for (var i = start; i >= end; i--)
		{
			a += i;

			if (i > exit)
				return 0;
		}

		return a;
	}
}
