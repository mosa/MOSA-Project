// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Other
{
	public static partial class InterestingTests
	{
		public struct Struct
		{ public int X { get; set; } }

		public static int Slow()
		{
			Struct a = new Struct();

			for (int i = 0; i < 10000000; i++)
			{
				a.X += 1;
			}

			return a.X;
		}

		public static int Fast()
		{
			Struct a = new Struct();

			for (int i = 0; i < 10000000; i++)
			{
				a.X = a.X + 1;
			}

			return a.X;
		}
	}
}
