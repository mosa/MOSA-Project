// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.UnitTests.TinyCoreLib;

public static class MemoryMarshalTests
{
	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_MemoryMarshal_Cast_ByteToInt(byte val1, byte val2)
	{
		byte[] bytes = [val1, 0, 0, 0, val2, 0, 0, 0];
		Span<byte> byteSpan = bytes;
		Span<int> intSpan = MemoryMarshal.Cast<byte, int>(byteSpan);

		return intSpan.Length == 2 && intSpan[0] == val1 && intSpan[1] == val2;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static int Test_MemoryMarshal_Cast_IntToByte(int va1, int val2)
	{
		int[] ints = [va1, val2];
		Span<int> intSpan = ints;
		Span<byte> byteSpan = MemoryMarshal.Cast<int, byte>(intSpan);

		return byteSpan.Length;
	}

	[MosaUnitTest]
	public static bool Test_MemoryMarshal_GetReference_Span()
	{
		int[] array = [1, 2, 3, 4];
		Span<int> span = array;
		ref var reference = ref MemoryMarshal.GetReference(span);

		var assert1 = reference == 1;
		reference = 99;
		return assert1 && array[0] == 99;
	}

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static int Test_MemoryMarshal_GetReference_ReadOnlySpan(int val1, int val2, int val3, int val4)
	{
		int[] array = [val1, val2, val3, val4];
		ReadOnlySpan<int> span = array;

		ref readonly var reference = ref MemoryMarshal.GetReference(span);
		return reference;
	}

	[MosaUnitTest(Series = "U1U1U1U1")]
	public static int Test_MemoryMarshal_Read_Int(byte val1, byte val2, byte val3, byte val4)
	{
		byte[] bytes = [val1, val2, val3, val4];
		ReadOnlySpan<byte> span = bytes;
		var value = MemoryMarshal.Read<int>(span);

		return value;
	}

	[MosaUnitTest(Series = "U1")]
	public static long Test_MemoryMarshal_Read_Long(byte val)
	{
		byte[] bytes = [val, 0, 0, 0, 0, 0, 0, 0];
		ReadOnlySpan<byte> span = bytes;
		var value = MemoryMarshal.Read<long>(span);

		return value;
	}

	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_MemoryMarshal_Read_Struct(byte val1, byte val2)
	{
		var bytes = new byte[8];
		bytes[0] = val1;
		bytes[4] = val2;

		ReadOnlySpan<byte> span = bytes;
		var value = MemoryMarshal.Read<TestStruct>(span);

		return value.Field1 == val1 && value.Field2 == val2;
	}

	// Helper type for testing
	private struct TestStruct
	{
		public int Field1;
		public int Field2;
	}
}
