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
			testMethods.AddLast(PointerTest);
			testMethods.AddLast(HandleTest);
			testMethods.AddLast(FindTypeOfTest);
			testMethods.AddLast(FindTypeByNameTest);
			testMethods.AddLast(CompareTypeHandlesTest);
			testMethods.AddLast(TypeHandleFromObjectTest);
			testMethods.AddLast(DeclaringTypeTest);
			testMethods.AddLast(ElementTypeTest);
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

		public static bool DeclaringTypeTest()
		{
			Type foundType = Type.GetType("System.Int32&");
			Type declaringType = Type.GetType("System.Int32");
			return foundType.DeclaringType != null && foundType.DeclaringType.Equals(declaringType);
		}

		public static bool ElementTypeTest()
		{
			Type foundType = Type.GetType("System.Int32[]");
			Type elementType = Type.GetType("System.Int32");
			return foundType.HasElementType && foundType.GetElementType().Equals(elementType);
		}
	}
}