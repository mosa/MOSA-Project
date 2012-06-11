/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal.Plug;

namespace Mosa.HelloWorld.x86.Tests
{
	class PlugTestCase
	{
		// Incomplete implementations that plugs will implement
		public static int Double(int a) { return 0; }
		public static int AddOne(int a) { return 0; }
		public int AddZ2Z(int z) { return 0; }
	}

	[PlugType("Mosa.HelloWorld.x86.Tests.PlugTestCase")]
	static class PlugTestImplementation
	{
		public static int AddOne(int a) { return a + 1; }

		[PlugMethod("Mosa.HelloWorld.x86.Tests.PlugTestCase.Double")]
		public static int Double(int a) { return a + a; }

		public static int AddZ2Z(ref PlugTestCase plugTestCase, int z) { return z + z; }
	}

	public class PlugTestTest : KernelTest
	{
		public PlugTestTest()
			: base("Plug")
		{
			testMethods.Add(PlugTestTest1);
			testMethods.Add(PlugTestTest2);
			testMethods.Add(PlugTestTest3);
		}

		public static bool PlugTestTest1()
		{
			return PlugTestCase.AddOne(10) == 11;
		}

		public static bool PlugTestTest2()
		{
			return PlugTestCase.Double(10) == 20;
		}

		public static bool PlugTestTest3()
		{
			PlugTestCase test = new PlugTestCase();
			return test.AddZ2Z(11) == 22;
		}

	}
}
