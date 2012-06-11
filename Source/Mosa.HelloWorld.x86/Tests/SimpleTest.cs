/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Test.Collection;

namespace Mosa.HelloWorld.x86.Tests
{
	public class SimpleTest : KernelTest
	{
		public SimpleTest()
			: base("Simple")
		{
			testMethods.Add(SimpleTest1);
			testMethods.Add(SimpleTest2);
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
