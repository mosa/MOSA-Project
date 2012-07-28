/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.HelloWorld.x86.Tests
{
	public class SimpleTest : KernelTest
	{
		public SimpleTest()
			: base("Simple")
		{
			testMethods.Add(SimpleTest1);
			testMethods.Add(SimpleTest2);
			testMethods.Add(SimpleTest3);
			testMethods.Add(SimpleTest4);
			testMethods.Add(SimpleTest5);
			testMethods.Add(SimpleTest6);
			testMethods.Add(SimpleTest7);
			testMethods.Add(SimpleTest8);
			testMethods.Add(SimpleTest9);
			testMethods.Add(SimpleTest10);
			testMethods.Add(SimpleTest99);
		}

		public static bool SimpleTest1()
		{
			int a = 10;
			int b = 20;

			int c = a + b;

			return c == 30;
		}

		public static bool SimpleTest2()
		{
			uint a = 10;
			uint b = 20;

			uint c = a + b;

			return c == 30;
		}

		public static bool SimpleTest3()
		{
			byte a = 10;
			uint b = 20;

			uint c = a + b;

			return c == 30;
		}

		public static bool SimpleTest4()
		{
			ulong a = 10;
			ulong b = 20;

			ulong c = a + b;

			return c == 30;
		}

		public static bool SimpleTest5()
		{
			ulong a = ulong.MaxValue;
			ulong b = 20;

			ulong c = a + b;

			return c == unchecked(ulong.MaxValue + 20);
		}

		public static bool SimpleTest6()
		{
			char a = (char)10;
			char b = (char)20;

			char c = (char)(a + b);

			return c == 30;
		}

		public static bool SimpleTest7()
		{
			ulong a = 10;
			ulong b = 0;

			ulong c = a * b;

			return c == 0;
		}

		public static bool SimpleTest8()
		{
			int a = 10;
			int b = 0;

			int c = a * b;

			return c == 0;
		}

		public static bool SimpleTest9()
		{
			ulong a = 10;
			ulong b = 1;

			ulong c = a * b;

			return c == 10;
		}

		public static bool SimpleTest10()
		{
			int a = 1;
			int b = 10;

			int c = a * b;

			return c == 10;
		}

		public static bool SimpleTest11()
		{
			int a = 0;
			int b = 10;

			int c = a * b;

			return c == 10;
		}


		public static int SimpleTest12()
		{
			int a = 32;
			int b = 10;
			int c = 10;
			int d = 1;
			int e = 0;

			int z = (a * b) + ((c * d) + (c * d) * e) + e;

			return z;
		}

		public static int SimpleTest12(int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			return c;
		}

		public static int SimpleTest13(ref int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			return c;
		}

		public static int SimpleTest14(ref int q)
		{
			int a = 10;
			int b = 20;

			int c = a + b + q;

			q = a + b;

			return c;
		}

		public static bool SimpleTest99()
		{
			int[] a = new int[5];
			for (int i = 0; i < 5; i++)
				a[i] = i * 2;

			int total = 0;

			foreach (int v in a)
				total = total + v;

			return (0 + 2 + 4 + 6 + 8) == total;
		}

	}
}
