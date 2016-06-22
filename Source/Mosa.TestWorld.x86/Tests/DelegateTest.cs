// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Collection;

namespace Mosa.TestWorld.x86.Tests
{
	public class DelegateTest : KernelTest
	{
		public DelegateTest()
			: base("Delegate")
		{
			testMethods.AddLast(DelegateTest1);
			testMethods.AddLast(DelegateTest2);
		}

		public static bool DelegateTest1()
		{
			int ret = DelegateTests.CallInstanceDelegate();

			return ret == 456;
		}

		public static bool DelegateTest2()
		{
			int ret = DelegateTests.CallInstanceDelegateStatic();

			return ret == 4560;
		}
	}
}
