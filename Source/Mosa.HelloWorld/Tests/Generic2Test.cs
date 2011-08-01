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
			Screen.Color = Colors.Gray;
			Screen.Write("Gen-N: ");

			PrintResult(GenericTest1());
			PrintResult(GenericTest2());
		}

		public static bool GenericTest1()
		{
			var list = new LinkedList<int>();
			list.Add(10);
			return list.Find(10).value == 10;
		}

		public static bool GenericTest2()
		{
			var list = new LinkedList<int>();
			list.Add(10);
			return list.First.value != 11;
		}
	}

}
