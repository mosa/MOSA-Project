// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests;

public static class BoxingTests 
{

	[MosaUnitTest(Series = "U1")]
	public static byte BoxU1(byte value)
	{
		object o = value;
		return (byte)o;
	}

	[MosaUnitTest(Series = "U1")]
	public static bool EqualsU1(byte value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "U2")]
	public static ushort BoxU2(ushort value)
	{
		object o = value;
		return (ushort)o;
	}

	[MosaUnitTest(Series = "U2")]
	public static bool EqualsU2(ushort value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "U4")]
	public static uint BoxU4(uint value)
	{
		object o = value;
		return (uint)o;
	}

	[MosaUnitTest(Series = "U4")]
	public static bool EqualsU4(uint value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "U8")]
	public static ulong BoxU8(ulong value)
	{
		object o = value;
		return (ulong)o;
	}

	[MosaUnitTest(Series = "U8")]
	public static bool EqualsU8(ulong value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "I1")]
	public static sbyte BoxI1(sbyte value)
	{
		object o = value;
		return (sbyte)o;
	}

	[MosaUnitTest(Series = "I1")]
	public static bool EqualsI1(sbyte value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "I2")]
	public static short BoxI2(short value)
	{
		object o = value;
		return (short)o;
	}

	[MosaUnitTest(Series = "I2")]
	public static bool EqualsI2(short value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "I4")]
	public static int BoxI4(int value)
	{
		object o = value;
		return (int)o;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool EqualsI4(int value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "I8")]
	public static long BoxI8(long value)
	{
		object o = value;
		return (long)o;
	}

	[MosaUnitTest(Series = "I8")]
	public static bool EqualsI8(long value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "R4")]
	public static float BoxR4(float value)
	{
		object o = value;
		return (float)o;
	}

	[MosaUnitTest(Series = "R4")]
	public static bool EqualsR4(float value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "R8")]
	public static double BoxR8(double value)
	{
		object o = value;
		return (double)o;
	}

	[MosaUnitTest(Series = "R8")]
	public static bool EqualsR8(double value)
	{
		object o = value;
		return o.Equals(value);
	}

	[MosaUnitTest(Series = "C")]
	public static char BoxC(char value)
	{
		object o = value;
		return (char)o;
	}

	[MosaUnitTest(Series = "C")]
	public static bool EqualsC(char value)
	{
		object o = value;
		return o.Equals(value);
	}
}
