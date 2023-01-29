// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Generics;

public static class GenericCallTests
{
	private static T GenericCallTarget<T>(T value)
	{
		return value;
	}

	[MosaUnitTest(Series = "U1")]
	public static bool GenericCallU1(byte value)
	{
		return value == GenericCallTarget<byte>(value);
	}

	[MosaUnitTest(Series = "U2")]
	public static bool GenericCallU2(ushort value)
	{
		return value == GenericCallTarget<ushort>(value);
	}

	[MosaUnitTest(Series = "U4")]
	public static bool GenericCallU4(uint value)
	{
		return value == GenericCallTarget<uint>(value);
	}

	[MosaUnitTest(Series = "U8")]
	public static bool GenericCallU8(ulong value)
	{
		return value == GenericCallTarget<ulong>(value);
	}

	[MosaUnitTest(Series = "I1")]
	public static bool GenericCallI1(sbyte value)
	{
		return value == GenericCallTarget<sbyte>(value);
	}

	[MosaUnitTest(Series = "I2")]
	public static bool GenericCallI2(short value)
	{
		return value == GenericCallTarget<short>(value);
	}

	[MosaUnitTest(Series = "I4")]
	public static bool GenericCallI4(int value)
	{
		return value == GenericCallTarget<int>(value);
	}

	[MosaUnitTest(Series = "I8")]
	public static bool GenericCallI8(long value)
	{
		return value == GenericCallTarget<long>(value);
	}

	[MosaUnitTest(Series = "C")]
	public static bool GenericCallC(char value)
	{
		return value == GenericCallTarget<char>(value);
	}
}
