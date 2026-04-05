// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.TinyCoreLib;

public static class RuntimeMethodHandleTests
{
	// == Constructor tests (via FromIntPtr)

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_Constructor(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeMethodHandle.FromIntPtr(ptr);
		return handle.Value == ptr;
	}

	// == Value property tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_Value_Property(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeMethodHandle.FromIntPtr(ptr);
		return handle.Value == ptr;
	}

	// == Equals tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_Equals_Same(int value)
	{
		var ptr = new IntPtr(value);
		var handle1 = RuntimeMethodHandle.FromIntPtr(ptr);
		var handle2 = RuntimeMethodHandle.FromIntPtr(ptr);
		return handle1.Equals(handle2) && handle1.Equals((object)handle2);
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_Equals_Different(int value)
	{
		var handle1 = RuntimeMethodHandle.FromIntPtr(new IntPtr(value));
		var handle2 = RuntimeMethodHandle.FromIntPtr(new IntPtr(value + 1));
		return !handle1.Equals(handle2);
	}

	// == GetHashCode tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_GetHashCode(int value)
	{
		var ptr = new IntPtr(value);
		var handle1 = RuntimeMethodHandle.FromIntPtr(ptr);
		var handle2 = RuntimeMethodHandle.FromIntPtr(ptr);
		return handle1.GetHashCode() == handle2.GetHashCode();
	}

	// == FromIntPtr tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_FromIntPtr(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeMethodHandle.FromIntPtr(ptr);
		return handle.Value == ptr;
	}

	// == ToIntPtr tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_ToIntPtr(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeMethodHandle.FromIntPtr(ptr);
		return RuntimeMethodHandle.ToIntPtr(handle) == ptr;
	}

	// == Operator == tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_OperatorEquals(int value)
	{
		var ptr = new IntPtr(value);
		var handle1 = RuntimeMethodHandle.FromIntPtr(ptr);
		var handle2 = RuntimeMethodHandle.FromIntPtr(ptr);
		return handle1 == handle2;
	}

	// == Operator != tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeMethodHandle_OperatorNotEquals(int value)
	{
		var handle1 = RuntimeMethodHandle.FromIntPtr(new IntPtr(value));
		var handle2 = RuntimeMethodHandle.FromIntPtr(new IntPtr(value + 1));
		return handle1 != handle2;
	}
}
