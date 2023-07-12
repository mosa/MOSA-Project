// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.FlowControl;

public static class WhileTests
{
	[MosaUnitTest(0, 20)]
	[MosaUnitTest(-20, 0)]
	[MosaUnitTest(-100, 100)]
	public static int WhileIncI4(int start, int limit)
	{
		var count = 0;

		while (start < limit)
		{
			++count;
			++start;
		}

		return count;
	}

	[MosaUnitTest(20, 0)]
	[MosaUnitTest(0, -20)]
	[MosaUnitTest(100, -100)]
	public static int WhileDecI4(int start, int limit)
	{
		var count = 0;

		while (start > limit)
		{
			++count;
			--start;
		}

		return count;
	}

	[MosaUnitTest]
	public static bool WhileFalse()
	{
		var called = false;

		while (false)
		{
#pragma warning disable CS0162 // Unreachable code detected
			called = true;
#pragma warning restore CS0162 // Unreachable code detected
		}

		return called;
	}

	[MosaUnitTest]
	public static bool WhileContinueBreak()
	{
		const int limit = 20;
		var count = 0;

		while (true)
		{
			++count;

			if (count == limit)
			{
				break;
			}
			else
			{
				continue;
			}
		}

		return count == 20;
	}

	[MosaUnitTest]
	public static bool WhileContinueBreak2()
	{
		var start = 0;
		const int limit = 20;
		var count = 0;

		while (true)
		{
			++count;
			++start;

			if (start == limit)
			{
				break;
			}
			else
			{
				continue;
			}
		}

		return start == limit && count == 20;
	}

	[MosaUnitTest]
	public static int WhileContinueBreak2B()
	{
		var start = 0;
		var limit = 20;
		var count = 0;

		while (true)
		{
			++count;
			++start;

			if (start == limit)
			{
				break;
			}
			else
			{
				continue;
			}
		}

		return count;
	}

	[MosaUnitTest((byte)254, (byte)1)]
	[MosaUnitTest(byte.MaxValue, byte.MinValue)]
	public static int WhileOverflowIncI1(byte start, byte limit)
	{
		var count = 0;

		while (start != limit)
		{
			++start;
			++count;
		}

		return count;
	}

	[MosaUnitTest((byte)1, (byte)254)]
	[MosaUnitTest(byte.MinValue, byte.MaxValue)]
	public static int WhileOverflowDecI1(byte start, byte limit)
	{
		var count = 0;

		while (start != limit)
		{
			--start;
			++count;
		}

		return count;
	}

	[MosaUnitTest(2, 3, 0, 20)]
	[MosaUnitTest(0, 1, 100, 200)]
	[MosaUnitTest(1, 0, -100, 100)]
	[MosaUnitTest(int.MaxValue, int.MinValue, -2, 3)]
	public static int WhileNestedEqualsI4(int a, int b, int start, int limit)
	{
		var count = 0;
		var start2 = start;
		var status = a;

		while (status == a)
		{
			start2 = 1;

			while (start2 < 5)
			{
				++start2;
			}

			++start;

			if (start == limit)
			{
				status = b;
			}
		}

		return count;
	}
}
