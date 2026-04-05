// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.TinyCoreLib;

public static class RuntimeFieldHandleTests
{
	// == Constructor tests (via FromIntPtr)

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_Constructor(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeFieldHandle.FromIntPtr(ptr);
		return handle.Value == ptr;
	}

	// == Value property tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_Value_Property(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeFieldHandle.FromIntPtr(ptr);
		return handle.Value == ptr;
	}

	// == Equals tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_Equals_Same(int value)
	{
		var ptr = new IntPtr(value);
		var handle1 = RuntimeFieldHandle.FromIntPtr(ptr);
		var handle2 = RuntimeFieldHandle.FromIntPtr(ptr);
		return handle1.Equals(handle2) && handle1.Equals((object)handle2);
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_Equals_Different(int value)
	{
		var handle1 = RuntimeFieldHandle.FromIntPtr(new IntPtr(value));
		var handle2 = RuntimeFieldHandle.FromIntPtr(new IntPtr(value + 1));
		return !handle1.Equals(handle2);
	}

	// == GetHashCode tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_GetHashCode(int value)
	{
		var ptr = new IntPtr(value);
		var handle1 = RuntimeFieldHandle.FromIntPtr(ptr);
		var handle2 = RuntimeFieldHandle.FromIntPtr(ptr);
		return handle1.GetHashCode() == handle2.GetHashCode();
	}

	// == FromIntPtr tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_FromIntPtr(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeFieldHandle.FromIntPtr(ptr);
		return handle.Value == ptr;
	}

	// == ToIntPtr tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_ToIntPtr(int value)
	{
		var ptr = new IntPtr(value);
		var handle = RuntimeFieldHandle.FromIntPtr(ptr);
		return RuntimeFieldHandle.ToIntPtr(handle) == ptr;
	}

	// == Operator == tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_OperatorEquals(int value)
	{
		var ptr = new IntPtr(value);
		var handle1 = RuntimeFieldHandle.FromIntPtr(ptr);
		var handle2 = RuntimeFieldHandle.FromIntPtr(ptr);
		return handle1 == handle2;
	}

	// == Operator != tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RuntimeFieldHandle_OperatorNotEquals(int value)
	{
		var handle1 = RuntimeFieldHandle.FromIntPtr(new IntPtr(value));
		var handle2 = RuntimeFieldHandle.FromIntPtr(new IntPtr(value + 1));
		return handle1 != handle2;
	}
}
