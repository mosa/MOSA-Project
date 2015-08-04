// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			testMethods.AddLast(TypeActivator);
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
			return (foundType != null && foundType == Type.GetTypeFromHandle(foundType.TypeHandle));
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
			return (foundType != null && foundType.DeclaringType != null && foundType.DeclaringType.Equals(declaringType));
		}

		public static bool ElementTypeTest()
		{
			Type foundType = Type.GetType("System.Int32[]");
			Type elementType = Type.GetType("System.Int32");
			return (foundType != null && foundType.HasElementType && foundType.GetElementType().Equals(elementType));
		}

		public static bool TypeActivator()
		{
			Type foundType = Type.GetType("Mosa.TestWorld.x86.Tests.TestClass123");
			var obj = (TestClass123)Activator.CreateInstance(foundType);
			return (obj.i == 52);
		}
	}

	public class TestClass123
	{
		public int i = 0;

		public TestClass123()
		{
			i = 52;
		}
	}
}
