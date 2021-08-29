// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.Demo.TestWorld.x86.Tests
{
	public class DelegateTest : KernelTest
	{
		public DelegateTest()
			: base("Delegate")
		{
			testMethods.Add(DelegateTest1);
			testMethods.Add(DelegateTest2);
			testMethods.Add(DelegateTest3);
			testMethods.Add(DelegateTest4);
			testMethods.Add(DelegateTest5);
			testMethods.Add(DelegateTest6);
			testMethods.Add(DelegateTest7);
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

		public static bool DelegateTest3()
		{
			int ret = DelegateTests.InlineDelegate1();

			return ret == 124;
		}

		public static bool DelegateTest4()
		{
			int ret = DelegateTests.InlineDelegate2();

			return ret == 124;
		}

		public static bool DelegateTest5()
		{
			int ret = DelegateTests.InlineDelegate3();

			return ret == 124;
		}

		public static bool DelegateTest6()
		{
			int ret = DelegateTests.InlineDelegate4();

			return ret == 0;
		}

		public static bool DelegateTest7()
		{
			int ret = DelegateTests.InlineDelegate5();

			return ret == 124;
		}
	}
}
