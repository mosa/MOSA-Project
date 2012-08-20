/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.HelloWorld.x86.Tests
{

	public class OtherTest : KernelTest
	{
		public OtherTest()
			: base("Other")
		{
			testMethods.Add(OtherTest1);
			testMethods.Add(OtherTest2);
		}

		private static uint StaticValue = 0x200000;

		public static bool OtherTest1()
		{
			uint x = StaticValue;

			return x == 0x200000;
		}

		public static bool OtherTest2()
		{
			return StaticValue == 0x200000;
		}

	}
}
