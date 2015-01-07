/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Collections.Generic;

namespace Mosa.TestWorld.x86.Tests
{
	public class ArrayTest : KernelTest
	{
		public ArrayTest()
			: base("Array")
		{
			testMethods.AddLast(GenericInterfaceTest);
			// TODO: add more tests
		}

		public static bool GenericInterfaceTest()
		{
			int[] list = new int[] { 1, 3, 5 };
			IList<int> iList = list;

			int result = 0;
			foreach (var i in iList)
				result += i;
			return (result == 9);
		}
	}
}