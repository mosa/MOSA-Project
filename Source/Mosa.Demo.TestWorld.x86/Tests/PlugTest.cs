﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

	public class PlugTest : KernelTest
	{
		public PlugTest()
			: base("Plug")
		{
			testMethods.Add(PlugTest1);
			testMethods.Add(PlugTest2);
			testMethods.Add(PlugTest3);
			testMethods.Add(PlugTest4);
			testMethods.Add(PlugTest5);
			testMethods.Add(PlugTest6);
		}

		public static bool PlugTest1()
		{
			return PlugTestCase.AddOne(10) == 11;
		}

		public static bool PlugTest2()
		{
			return PlugTestCase.Double(10) == 20;
		}

		public static bool PlugTest3()
		{
			return new PlugTestCase().AddZ2Z(11) == 22;
		}

		public static bool PlugTest4()
		{
			return System.Runtime.Intrinsics.X86.Popcnt.PopCount(3) == 2;
		}

		public static bool PlugTest5()
		{
			return System.Runtime.Intrinsics.X86.Lzcnt.LeadingZeroCount(0x0) == 32;
		}

		public static bool PlugTest6()
		{
			return System.Runtime.Intrinsics.X86.Bmi1.TrailingZeroCount(0x2) == 1;
		}
	}
}
