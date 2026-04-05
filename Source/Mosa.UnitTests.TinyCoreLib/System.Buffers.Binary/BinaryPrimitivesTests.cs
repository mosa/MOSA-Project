// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Buffers.Binary;

namespace Mosa.UnitTests.TinyCoreLib;

public static class BinaryPrimitivesTests
{
	// == ReadUInt16BigEndian tests

	[MosaUnitTest(Series = "U1U1")]
	public static ushort Test_ReadUInt16BigEndian(byte b1, byte b2)
		=> BinaryPrimitives.ReadUInt16BigEndian([b1, b2]);

	[MosaUnitTest]
	public static ushort Test_ReadUInt16BigEndian_MaxValue()
		=> BinaryPrimitives.ReadUInt16BigEndian([0xFF, 0xFF]);

	[MosaUnitTest]
	public static ushort Test_ReadUInt16BigEndian_MinValue()
		=> BinaryPrimitives.ReadUInt16BigEndian([0x00, 0x00]);

	// == ReadUInt16LittleEndian tests

	[MosaUnitTest(Series = "U1U1")]
	public static ushort Test_ReadUInt16LittleEndian(byte b1, byte b2)
		=> BinaryPrimitives.ReadUInt16LittleEndian([b1, b2]);

	[MosaUnitTest]
	public static ushort Test_ReadUInt16LittleEndian_MaxValue()
		=> BinaryPrimitives.ReadUInt16LittleEndian([0xFF, 0xFF]);

	[MosaUnitTest]
	public static ushort Test_ReadUInt16LittleEndian_MinValue()
		=> BinaryPrimitives.ReadUInt16LittleEndian([0x00, 0x00]);

	// == ReverseEndianness(ushort) tests

	[MosaUnitTest(Series = "U2")]
	public static ushort Test_ReverseEndianness_UInt16(ushort value)
		=> BinaryPrimitives.ReverseEndianness(value);

	[MosaUnitTest]
	public static ushort Test_ReverseEndianness_UInt16_MaxValue()
		=> BinaryPrimitives.ReverseEndianness(ushort.MaxValue);

	[MosaUnitTest]
	public static ushort Test_ReverseEndianness_UInt16_MinValue()
		=> BinaryPrimitives.ReverseEndianness(ushort.MinValue);

	[MosaUnitTest]
	public static ushort Test_ReverseEndianness_UInt16_DoubleReverse(ushort value)
		=> BinaryPrimitives.ReverseEndianness(BinaryPrimitives.ReverseEndianness(value));
}
