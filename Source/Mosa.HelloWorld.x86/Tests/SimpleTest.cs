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
			: base("SimpleTest") 
		{
			testMethods.Add(SimpleTest1);
		}

		public static bool SimpleTest1()
		{
			int a = 10;
			int b = 20;

			int c = a + b;

			return c == 30;
		}

	}
}
