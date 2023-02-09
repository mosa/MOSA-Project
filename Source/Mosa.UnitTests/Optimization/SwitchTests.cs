// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.Optimization;

public static class SwitchTests
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SimpleSwitch(int a)
	{
		return a switch
		{
			0 => 0,
			1 => 1,
			2 => 2,
			3 => 3,
			_ => a
		};
	}

	[MosaUnitTest]
	public static int SwitchTests1()
	{
		return SimpleSwitch(1);
	}

	[MosaUnitTest]
	public static int SwitchTests2()
	{
		return SimpleSwitch(3);
	}

	[MosaUnitTest]
	public static int SwitchTests3()
	{
		return SimpleSwitch(4);
	}

	public static int SwitchTests4()
	{
		return SimpleSwitch(5);
	}
}
