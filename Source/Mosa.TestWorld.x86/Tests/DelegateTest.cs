/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Test.Collection;

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