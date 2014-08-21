/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.TestWorld.x86.Tests
{
	public class OtherTest : KernelTest
	{
		public OtherTest()
			: base("Other")
		{
			testMethods.Add(OtherTest1);
			testMethods.Add(OtherTest2);
			testMethods.Add(OtherTest3);
			testMethods.Add(OtherTest4);
			testMethods.Add(OtherTest5);
			testMethods.Add(OtherTest6);
			testMethods.Add(OtherTest7);
			testMethods.Add(OtherTest8);
			testMethods.Add(OtherTest9);
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

		public static bool OtherTest3()
		{
			return 3.Equals(3);
		}

		public static bool OtherTest4()
		{
			return (32uL / 16uL) == 2uL;
		}

		public static bool OtherTest5()
		{
			return (222222222222uL / 2uL) == 111111111111uL;
		}

		public static bool OtherTest6()
		{
			return (666666666663uL % 5uL) == 3uL;
		}

		public static bool OtherTest7()
		{
			return (32L / 16L) == 2L;
		}

		public static bool OtherTest8()
		{
			return (222222222222L / 2L) == 111111111111L;
		}

		public static bool OtherTest9()
		{
			return (666666666663L % 5L) == 3L;
		}
	}

	public struct TestStruct
	{
		public byte One;
	}
}