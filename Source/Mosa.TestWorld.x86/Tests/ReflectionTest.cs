/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
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
			Type bootType = typeof(Mosa.TestWorld.x86.Boot);
			Type foundType = Type.GetType("Mosa.TestWorld.x86.Boot");
			return foundType.Equals(bootType);
		}
	}
}