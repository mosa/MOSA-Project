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
			if (bootType == null)
				return false;
			Type foundType = Type.GetType("System.String");
			if (foundType == null)
				return false;
			return bootType.Equals(foundType);
		}
	}
}