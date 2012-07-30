/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.HelloWorld.x86.Tests
{
	public class OptimizationTest : KernelTest
	{
		public OptimizationTest()
			: base("Optimization")
		{
			testMethods.Add(OptimizationTest1);
			testMethods.Add(OptimizationTest2);
			testMethods.Add(OptimizationTest3);
			testMethods.Add(OptimizationTest4);
			testMethods.Add(OptimizationTest5);
			testMethods.Add(OptimizationTest6);
			testMethods.Add(OptimizationTest7);
			testMethods.Add(OptimizationTest8);
			testMethods.Add(OptimizationTest9);
			testMethods.Add(OptimizationTest10);
			testMethods.Add(OptimizationTest11);
		}

		public static bool OptimizationTest1()
		{
			int a = 10;
			int b = 20;

			int c = a + b;

			return c == 30;
		}

		public static bool OptimizationTest2()
		{
			uint a = 10;
			uint b = 20;

			uint c = a + b;

			return c == 30;
		}

		public static bool OptimizationTest3()
		{
			byte a = 10;
			uint b = 20;

			uint c = a + b;

			return c == 30;
		}

		public static bool OptimizationTest4()
		{
			ulong a = 10;
			ulong b = 20;

			ulong c = a + b;

			return c == 30;
		}

		public static bool OptimizationTest5()
		{
			ulong a = ulong.MaxValue;
			ulong b = 20;

			ulong c = a + b;

			return c == unchecked(ulong.MaxValue + 20);
		}

		public static bool OptimizationTest6()
		{
			char a = (char)10;
			char b = (char)20;

			char c = (char)(a + b);

			return c == 30;
		}

		public static bool OptimizationTest7()
		{
			ulong a = 10;
			ulong b = 0;

			ulong c = a * b;

			return c == 0;
		}

		public static bool OptimizationTest8()
		{
			int a = 10;
			int b = 0;

			int c = a * b;

			return c == 0;
		}

		public static bool OptimizationTest9()
		{
			ulong a = 10;
			ulong b = 1;

			ulong c = a * b;

			return c == 10;
		}

		public static bool OptimizationTest10()
		{
			int a = 1;
			int b = 10;

			int c = a * b;

			return c == 10;
		}

		public static bool OptimizationTest11()
		{
			int a = 0;
			int b = 10;

			int c = a * b;

			return c == 0;
		}

		public static int OptimizationTest12()
		{
			int a = 32;
			int b = 10;
			int c = 10;
			int d = 1;
			int e = 0;

			int z = (a * b) + ((c * d) + (c * d) * e) + e;

			return z;
		}

		public static int OptimizationTest12(int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			return c;
		}

		public static int OptimizationTest13(ref int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			return c;
		}

		public static int OptimizationTest14(ref int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			q = a + b;

			return c;
		}

	}
}
