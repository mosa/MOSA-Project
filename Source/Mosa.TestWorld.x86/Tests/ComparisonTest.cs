// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Collection;

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
			return ComparisonTests.CompareEqualI1(-1, -1);
		}

		public static bool CompareTest2()
		{
			return ComparisonTests.CompareEqualI2(-2, -2);
		}

		public static bool CompareTest3()
		{
			return ComparisonTests.CompareEqualI2(-1, -1);
		}
	}
}
