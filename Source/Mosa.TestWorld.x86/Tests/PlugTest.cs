// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Internal.Plug;

namespace Mosa.TestWorld.x86.Tests
{
	internal class PlugTestCase
	{
		// Incomplete implementations that plugs will implement
		public static int Double(int a)
		{
			return 0;
		}

		public static int AddOne(int a)
		{
			return 0;
		}

		public int AddZ2Z(int z)
		{
			return 0;
		}
	}

	[Type("Mosa.TestWorld.x86.Tests.PlugTestCase")]
	internal static class PlugTestImplementation
	{
		public static int AddOne(int a)
		{
			return a + 1;
		}

		[Method("Mosa.TestWorld.x86.Tests.PlugTestCase.Double")]
		public static int Double(int a)
		{
			return a + a;
		}

		public static int AddZ2Z(ref PlugTestCase plugTestCase, int z)
		{
			return z + z;
		}
	}

	public class PlugTestTest : KernelTest
	{
		public PlugTestTest()
			: base("Plug")
		{
			testMethods.AddLast(PlugTestTest1);
			testMethods.AddLast(PlugTestTest2);
			testMethods.AddLast(PlugTestTest3);
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
