/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Platform.x86;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.ClassLib;

namespace Mosa.HelloWorld.Tests
{

	public class Generic2Test : KernelTest
	{
		public static void Test()
		{
			Screen.Goto(23, 65);
			Screen.Write("List Test: ");

			PrintResult(GenericTest1());
		}

		public static bool GenericTest1()
		{
			LinkedList<int> list = new LinkedList<int>();

			//int a = Math.Min(10, 10);

			return true;
		}
	}

}
