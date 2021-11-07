// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests.Primitive;

namespace Mosa.Demo.TestWorld.x86.Tests
{
	public class ComparisonTest : KernelTest
	{
		public ComparisonTest()
			: base("Comparison")
		{
			testMethods.Add(CompareTest1);
			testMethods.Add(CompareTest2);
			testMethods.Add(CompareTest3);
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
