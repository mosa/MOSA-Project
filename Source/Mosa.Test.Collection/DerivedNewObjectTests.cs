// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public class DerivedNewObjectTests : NewObjectTests
	{
		private readonly int int32;

		public DerivedNewObjectTests()
		{
			int32 = 0;
		}

		public DerivedNewObjectTests(int value)
		{
			int32 = value;
		}

		public DerivedNewObjectTests(int v1, int v2)
		{
			int32 = v1 + v2;
		}

		public DerivedNewObjectTests(int v1, int v2, int v3)
		{
			int32 = (v1 * v2) + v3;
		}

		public static bool WithoutArgs()
		{
			DerivedNewObjectTests d = new DerivedNewObjectTests();
			return d != null;
		}

		public static bool WithOneArg()
		{
			DerivedNewObjectTests d = new DerivedNewObjectTests(42);
			return (d.int32 == 42);
		}

		public static bool WithTwoArgs()
		{
			DerivedNewObjectTests d = new DerivedNewObjectTests(42, 3);
			return (d.int32 == 45);
		}

		public static bool WithThreeArgs()
		{
			DerivedNewObjectTests d = new DerivedNewObjectTests(21, 2, 7);
			return (d.int32 == 49);
		}
	}
}
