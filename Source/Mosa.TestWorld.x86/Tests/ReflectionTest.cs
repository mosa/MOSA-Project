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
			testMethods.Add(FindTypeOfTest);
			testMethods.Add(FindTypeByNameTest);
			testMethods.Add(CompareTypeHandlesTest);
			testMethods.Add(TypeHandleFromObjectTest);
		}

		public static bool FindTypeOfTest()
		{
			Type bootType = typeof(System.String);
			return (bootType != null);
		}

		public static bool FindTypeByNameTest()
		{
			Type foundType = Type.GetType("System.String");
			return (foundType != null);
		}

		public static bool CompareTypeHandlesTest()
		{
			Type foundType1 = Type.GetType("System.String");
			Type foundType2 = Type.GetType("System.String");
			return foundType1.TypeHandle == foundType2.TypeHandle;
		}

		public static bool TypeHandleFromObjectTest()
		{
			string hello = "hi";
			return (hello.GetType() != null);
		}
	}
}