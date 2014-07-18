/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.TestWorld.x86.Tests
{
	public class ReflectionTest : KernelTest
	{
		public ReflectionTest()
			: base("Reflection")
		{
			testMethods.Add(FindTypeByNameTest);
		}

		public static bool FindTypeByNameTest()
		{
			Type bootType = typeof(System.String);
			Type foundType = Type.GetType("System.String");
			//if (foundType == null) // Need to wait until the register allocator bug is fixed
			//	return false;
			return bootType.Equals(foundType);
		}
	}
}