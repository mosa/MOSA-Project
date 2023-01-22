// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.UnitTests;

namespace Mosa.UnitTests.Optimization
{
	public static partial class SwitchTests
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SimpleSwitch(int a)
		{
			switch (a)
			{
				case 0: return 0;
				case 1: return 1;
				case 2: return 2;
				case 3: return 3;
				default: return a;
			}
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
}
