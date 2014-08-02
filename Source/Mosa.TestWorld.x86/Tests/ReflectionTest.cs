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
			testMethods.Add(PointerTest);
			testMethods.Add(HandleTest);
			testMethods.Add(FindTypeOfTest);
			testMethods.Add(FindTypeByNameTest);
			testMethods.Add(CompareTypeHandlesTest);
			testMethods.Add(TypeHandleFromObjectTest);
		}

		public static bool PointerTest()
		{
			IntPtr ptr1 = new IntPtr(30);
			IntPtr ptr2 = new IntPtr(30);
			return (ptr1 == ptr2);
		}

		public static bool HandleTest()
		{
			RuntimeTypeHandle handle1 = new RuntimeTypeHandle();
			RuntimeTypeHandle handle2 = new RuntimeTypeHandle();
			return (handle1 == handle2);
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
			Type foundType = Type.GetType("System.String");
			return foundType == Type.GetTypeFromHandle(foundType.TypeHandle);
		}

		public static bool TypeHandleFromObjectTest()
		{
			string hello = "hi";
			return (hello.GetType() != null);
		}
	}
}