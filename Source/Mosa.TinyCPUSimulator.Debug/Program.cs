/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Mosa.TinyCPUSimulator.TestSystem.xUnit;

namespace Mosa.TinyCPUSimulator.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Test2();
		}

		private static void Test3()
		{
			var fixture = new UInt64Fixture();

			fixture.DivU8U8(18446744073709551615, 4294967294);
		}

		private static void Test2()
		{
			var fixture = new EnumFixture();

			fixture.ItemAMustEqual5();
		}

		private static void Test1()
		{
			var test = new TestCPUx86();

			test.RunTest();
		}
	}
}