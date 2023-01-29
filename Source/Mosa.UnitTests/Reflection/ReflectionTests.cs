// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.Reflection;

public static class ReflectionTests
{
	[MosaUnitTest]
	public static bool PointerTest()
	{
		IntPtr ptr1 = new IntPtr(30);
		IntPtr ptr2 = new IntPtr(30);
		return ptr1 == ptr2;
	}

	[MosaUnitTest]
	public static bool HandleTest()
	{
		RuntimeTypeHandle handle1 = new RuntimeTypeHandle();
		RuntimeTypeHandle handle2 = new RuntimeTypeHandle();
		return handle1.Equals(handle2);
	}

	[MosaUnitTest]
	public static bool FindTypeOfTest()
	{
		Type bootType = typeof(System.String);
		return bootType != null;
	}

	[MosaUnitTest]
	public static bool FindTypeByNameTest()
	{
		Type foundType = Type.GetType("System.String");
		return foundType != null;
	}

	[MosaUnitTest]
	public static bool CompareTypeHandlesTest()
	{
		Type foundType = Type.GetType("System.String");
		return foundType != null && foundType == Type.GetTypeFromHandle(foundType.TypeHandle);
	}

	[MosaUnitTest]
	public static bool TypeHandleFromObjectTest()
	{
		string hello = "hi";
		return hello.GetType() != null;
	}

	[MosaUnitTest]
	public static bool DeclaringTypeTest1()
	{
		Type foundType = Type.GetType("System.Int32&");
		Type declaringType = Type.GetType("System.Int32");
		return foundType.DeclaringType == null;
	}

	[MosaUnitTest]
	public static bool DeclaringTypeTest2()
	{
		Type foundType = Type.GetType("Mosa.UnitTests.Reflection.ReflectionTests+TestClass123");
		Type declaringType = Type.GetType("Mosa.UnitTests.Reflection.ReflectionTests");
		return foundType.DeclaringType.Equals(declaringType);
	}

	[MosaUnitTest]
	public static bool ElementTypeTest()
	{
		Type foundType = Type.GetType("System.Int32[]");
		Type elementType = Type.GetType("System.Int32");
		return foundType.GetElementType().Equals(elementType);
	}

	[MosaUnitTest]
	public static bool TypeActivator()
	{
		Type foundType = Type.GetType("Mosa.UnitTests.Reflection.ReflectionTests+TestClass123");
		var obj = (TestClass123)Activator.CreateInstance(foundType);

		if (obj == null)
			return false;

		return obj.i == 52;
	}

	private class TestClass123
	{
		public int i = 0;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TestClass123()
		{
			i = 52;
		}
	}
}
