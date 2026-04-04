// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.UnitTests.TinyCoreLib;

public static class UnsafeTests
{
	[MosaUnitTest]
	public static int Test_Unsafe_SizeOf_Int() => Unsafe.SizeOf<int>();

	[MosaUnitTest]
	public static int Test_Unsafe_SizeOf_Long() => Unsafe.SizeOf<long>();

	[MosaUnitTest]
	public static int Test_Unsafe_SizeOf_Byte() => Unsafe.SizeOf<byte>();

	[MosaUnitTest]
	public static int Test_Unsafe_SizeOf_Struct() => Unsafe.SizeOf<TestStruct>();

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Unsafe_As_Reference(int value)
	{
		object obj = new TestClass { Value = value };
		ref var refObj = ref Unsafe.As<object, TestClass?>(ref obj);
		return refObj != null && refObj!.Value == value;
	}

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static unsafe bool Test_Unsafe_Add_Pointer(int val1, int val2, int val3, int val4)
	{
		int[] array = [val1, val2, val3, val4];
		fixed (int* ptr = array)
		{
			ref int refValue = ref Unsafe.Add(ref ptr[0], 2);
			return refValue == val3;
		}
	}

	// Helper types for testing
	private struct TestStruct
	{
		public int Field1;
		public int Field2;
	}

	private class TestClass
	{
		public int Value { get; set; }
	}
}
