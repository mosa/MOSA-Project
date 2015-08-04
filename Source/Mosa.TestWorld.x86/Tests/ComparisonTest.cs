// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Test.Collection;

namespace Mosa.TestWorld.x86.Tests
{
	public class ComparisonTest : KernelTest
	{
		public ComparisonTest()
			: base("Comparison")
		{
			testMethods.AddLast(CompareTest1);
			testMethods.AddLast(CompareTest2);
			testMethods.AddLast(CompareTest3);
		}

		public static bool CompareTest1()
		{
			return ComparisonTests.CompareEqualI1I8(-1, -1);
		}

		public static bool CompareTest2()
		{
			return ComparisonTests.CompareEqualI2I8(-2, -2);
		}

		public static bool CompareTest3()
		{
			return ComparisonTests.CompareEqualI2I8(-1, -1);
		}
	}
}
