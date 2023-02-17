// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Basic;

public static class StaticFieldTestU1
{
	private static byte field;

	[MosaUnitTest(Series = "U1")]
	public static bool StaticFieldU1(byte value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "U1")]
	public static byte StaticReturnFieldU1(byte value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestU2
{
	private static ushort field;

	[MosaUnitTest(Series = "U2")]
	public static bool StaticFieldU2(ushort value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "U2")]
	public static ushort StaticReturnFieldU2(ushort value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestU4
{
	private static uint field;

	[MosaUnitTest(Series = "U4")]
	public static bool StaticFieldU4(uint value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "U4")]
	public static uint StaticReturnFieldU4(uint value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestU8
{
	private static ulong field;

	[MosaUnitTest(Series = "U8")]
	public static bool StaticFieldU8(ulong value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "U8")]
	public static ulong StaticReturnFieldU8(ulong value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestI1
{
	private static sbyte field;

	[MosaUnitTest(Series = "I1")]
	public static bool StaticFieldI1(sbyte value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "I1")]
	public static sbyte StaticReturnFieldI1(sbyte value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestI2
{
	private static short field;

	[MosaUnitTest(Series = "I2")]
	public static bool StaticFieldI2(short value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "I2")]
	public static short StaticReturnFieldI2(short value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestI4
{
	private static int field;

	[MosaUnitTest(Series = "I4")]
	public static bool StaticFieldI4(int value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "I4")]
	public static int StaticReturnFieldI4(int value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestI8
{
	private static long field;

	[MosaUnitTest(Series = "I8")]
	public static bool StaticFieldI8(long value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "I8")]
	public static long StaticReturnFieldI8(long value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestR4
{
	private static float field;

	[MosaUnitTest(Series = "R4")]
	public static bool StaticFieldR4(float value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "R4")]
	public static float StaticReturnFieldR4(float value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestR8
{
	private static double field;

	[MosaUnitTest(Series = "R8")]
	public static bool StaticFieldR8(double value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "R8")]
	public static double StaticReturnFieldR8(double value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestB
{
	private static bool field;

	[MosaUnitTest(Series = "B")]
	public static bool StaticFieldB(bool value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "B")]
	public static bool StaticReturnFieldB(bool value)
	{
		field = value;
		return field;
	}
}

public static class StaticFieldTestC
{
	private static char field;

	[MosaUnitTest(Series = "C")]
	public static bool StaticFieldC(char value)
	{
		field = value;
		return value == field;
	}

	[MosaUnitTest(Series = "C")]
	public static char StaticReturnFieldC(char value)
	{
		field = value;
		return field;
	}
}
