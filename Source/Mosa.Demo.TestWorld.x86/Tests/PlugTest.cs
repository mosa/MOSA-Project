// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Demo.TestWorld.x86.Tests
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

	//[Type("Mosa.Demo.TestWorld.x86.Tests.PlugTestCase")]
	internal static class PlugTestImplementation
	{
		[Plug("Mosa.Demo.TestWorld.x86.Tests.PlugTestCase::AddOne")]
		public static int AddOne(int a)
		{
			return a + 1;
		}

		[Plug("Mosa.Demo.TestWorld.x86.Tests.PlugTestCase::Double")]
		public static int Double(int a)
		{
			return a + a;
		}

		[Plug("Mosa.Demo.TestWorld.x86.Tests.PlugTestCase::AddZ2Z")]
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
