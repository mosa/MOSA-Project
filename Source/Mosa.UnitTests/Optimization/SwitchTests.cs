// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.Optimization
{
	public static class SwitchTests
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
	}
}
