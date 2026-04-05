// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.UnitTests.TinyCoreLib;

public static class MemoryStreamTests
{
	// == Constructor tests

	[MosaUnitTest]
	public static bool Test_MemoryStream_Constructor_Default()
	{
		var ms = new MemoryStream();
		return ms.Length == 0L && ms.Position == 0L && ms.CanRead && ms.CanWrite && ms.CanSeek;
	}

	[MosaUnitTest(Series = "I4Small")]
	public static bool Test_MemoryStream_Constructor_WithCapacity(int capacity)
	{
		var ms = new MemoryStream(capacity);
		return ms.Length == 0L && ms.Capacity >= capacity;
	}

	[MosaUnitTest]
	public static bool Test_MemoryStream_Constructor_FromByteArray()
	{
		byte[] data = { 1, 2, 3, 4, 5 };
		var ms = new MemoryStream(data);
		return ms.Length == 5L && ms.Position == 0L;
	}

	// == Write tests

	[MosaUnitTest(Series = "U1")]
	public static bool Test_MemoryStream_Write_SingleByte(byte value)
	{
		var ms = new MemoryStream();
		ms.WriteByte(value);
		return ms.Length == 1L && ms.Position == 1L;
	}

	[MosaUnitTest]
	public static bool Test_MemoryStream_Write_ByteArray()
	{
		var ms = new MemoryStream();
		byte[] data = { 1, 2, 3, 4, 5 };
		ms.Write(data, 0, data.Length);
		return ms.Length == 5L && ms.Position == 5L;
	}

	[MosaUnitTest]
	public static long Test_MemoryStream_Write_Multiple()
	{
		var ms = new MemoryStream();
		byte[] data1 = { 1, 2, 3 };
		byte[] data2 = { 4, 5, 6 };
		ms.Write(data1, 0, data1.Length);
		ms.Write(data2, 0, data2.Length);
		return ms.Length;
	}

	// == Read tests

	[MosaUnitTest(Series = "U1")]
	public static bool Test_MemoryStream_Read_SingleByte(byte b)
	{
		byte[] data = { b };
		var ms = new MemoryStream(data);
		var value = ms.ReadByte();
		return value == b && ms.Position == 1L;
	}

	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_MemoryStream_Read_ByteArray(byte first, byte last)
	{
		byte[] data = { first, 2, 3, 4, last };
		var ms = new MemoryStream(data);
		var buffer = new byte[5];
		var bytesRead = ms.Read(buffer, 0, buffer.Length);
		return bytesRead == 5 && buffer[0] == first && buffer[1] == 2 && buffer[4] == last;
	}

	[MosaUnitTest(Series = "U1U1U1")]
	public static bool Test_MemoryStream_Read_Partial(byte b0, byte b1, byte b2)
	{
		byte[] data = { b0, b1, b2, 4, 5 };
		var ms = new MemoryStream(data);
		var buffer = new byte[3];
		var bytesRead = ms.Read(buffer, 0, buffer.Length);
		return bytesRead == 3 && buffer[0] == b0 && buffer[1] == b1 && buffer[2] == b2;
	}

	// == Position tests

	[MosaUnitTest]
	public static bool Test_MemoryStream_Position_Get()
	{
		byte[] data = { 1, 2, 3, 4, 5 };
		var ms = new MemoryStream(data);
		var initial = ms.Position;
		ms.ReadByte();
		return initial == 0L && ms.Position == 1L;
	}

	[MosaUnitTest(Series = "U1")]
	public static bool Test_MemoryStream_Position_Set(byte b)
	{
		byte[] data = { 0, 1, 2, b, 4 };
		var ms = new MemoryStream(data);
		ms.Position = 3;
		var value = ms.ReadByte();
		return ms.Position == 4L && value == b;
	}

	// == Length tests

	[MosaUnitTest]
	public static long Test_MemoryStream_Length_Empty()
	{
		var ms = new MemoryStream();
		return ms.Length;
	}

	[MosaUnitTest]
	public static long Test_MemoryStream_Length_AfterWrite()
	{
		var ms = new MemoryStream();
		byte[] data = { 1, 2, 3 };
		ms.Write(data, 0, data.Length);
		return ms.Length;
	}

	// == Seek tests

	[MosaUnitTest]
	public static bool Test_MemoryStream_Seek_FromBegin()
	{
		byte[] data = { 1, 2, 3, 4, 5 };
		var ms = new MemoryStream(data);
		var pos = ms.Seek(3, SeekOrigin.Begin);
		return pos == 3L && ms.Position == 3L;
	}

	[MosaUnitTest]
	public static long Test_MemoryStream_Seek_FromCurrent()
	{
		byte[] data = { 1, 2, 3, 4, 5 };
		var ms = new MemoryStream(data);
		ms.Position = 2;
		return ms.Seek(2, SeekOrigin.Current);
	}

	[MosaUnitTest]
	public static long Test_MemoryStream_Seek_FromEnd()
	{
		byte[] data = { 1, 2, 3, 4, 5 };
		var ms = new MemoryStream(data);
		return ms.Seek(-2, SeekOrigin.End);
	}

	// == ToArray tests

	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_MemoryStream_ToArray_ValidData(byte first, byte last)
	{
		var ms = new MemoryStream();
		byte[] data = { first, 2, 3, 4, last };
		ms.Write(data, 0, data.Length);
		var result = ms.ToArray();
		return result.Length == 5 && result[0] == first && result[4] == last;
	}

	[MosaUnitTest]
	public static int Test_MemoryStream_ToArray_Empty()
	{
		var ms = new MemoryStream();
		return ms.ToArray().Length;
	}

	// == GetBuffer tests

	[MosaUnitTest(Series = "U1U1U1")]
	public static bool Test_MemoryStream_GetBuffer_Valid(byte b0, byte b1, byte b2)
	{
		var ms = new MemoryStream();
		byte[] data = { b0, b1, b2 };
		ms.Write(data, 0, data.Length);
		var buffer = ms.GetBuffer();
		return buffer.Length >= 3 && buffer[0] == b0 && buffer[1] == b1 && buffer[2] == b2;
	}

	// == WriteTo tests

	[MosaUnitTest(Series = "U1U1")]
	public static bool Test_MemoryStream_WriteTo_AnotherStream(byte first, byte last)
	{
		var source = new MemoryStream();
		var dest = new MemoryStream();
		byte[] data = { first, 2, 3, 4, last };
		source.Write(data, 0, data.Length);
		source.Position = 0;
		source.WriteTo(dest);
		var result = dest.ToArray();
		return dest.Length == 5L && result[0] == first && result[4] == last;
	}
}
