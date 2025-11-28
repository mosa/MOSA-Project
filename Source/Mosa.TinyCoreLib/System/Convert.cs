using System.Diagnostics.CodeAnalysis;

namespace System;

public static class Convert
{
	public static readonly object DBNull;

	[return: NotNullIfNotNull("value")]
	public static object? ChangeType(object? value, Type conversionType)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static object? ChangeType(object? value, Type conversionType, IFormatProvider? provider)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static object? ChangeType(object? value, TypeCode typeCode)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static object? ChangeType(object? value, TypeCode typeCode, IFormatProvider? provider)
	{
		throw null;
	}

	public static byte[] FromBase64CharArray(char[] inArray, int offset, int length)
	{
		throw null;
	}

	public static byte[] FromBase64String(string s)
	{
		throw null;
	}

	public static byte[] FromHexString(ReadOnlySpan<char> chars)
	{
		throw null;
	}

	public static byte[] FromHexString(string s)
	{
		throw null;
	}

	public static TypeCode GetTypeCode(object? value)
	{
		throw null;
	}

	public static bool IsDBNull([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
	{
		throw null;
	}

	public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
	{
		throw null;
	}

	public static string ToBase64String(byte[] inArray)
	{
		throw null;
	}

	public static string ToBase64String(byte[] inArray, Base64FormattingOptions options)
	{
		throw null;
	}

	public static string ToBase64String(byte[] inArray, int offset, int length)
	{
		throw null;
	}

	public static string ToBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
	{
		throw null;
	}

	public static string ToBase64String(ReadOnlySpan<byte> bytes, Base64FormattingOptions options = Base64FormattingOptions.None)
	{
		throw null;
	}

	public static bool ToBoolean(bool value)
	{
		throw null;
	}

	public static bool ToBoolean(byte value)
	{
		throw null;
	}

	public static bool ToBoolean(char value)
	{
		throw null;
	}

	public static bool ToBoolean(DateTime value)
	{
		throw null;
	}

	public static bool ToBoolean(decimal value)
	{
		throw null;
	}

	public static bool ToBoolean(double value)
	{
		throw null;
	}

	public static bool ToBoolean(short value)
	{
		throw null;
	}

	public static bool ToBoolean(int value)
	{
		throw null;
	}

	public static bool ToBoolean(long value)
	{
		throw null;
	}

	public static bool ToBoolean([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static bool ToBoolean([NotNullWhen(true)] object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool ToBoolean(sbyte value)
	{
		throw null;
	}

	public static bool ToBoolean(float value)
	{
		throw null;
	}

	public static bool ToBoolean([NotNullWhen(true)] string? value)
	{
		throw null;
	}

	public static bool ToBoolean([NotNullWhen(true)] string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool ToBoolean(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool ToBoolean(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool ToBoolean(ulong value)
	{
		throw null;
	}

	public static byte ToByte(bool value)
	{
		throw null;
	}

	public static byte ToByte(byte value)
	{
		throw null;
	}

	public static byte ToByte(char value)
	{
		throw null;
	}

	public static byte ToByte(DateTime value)
	{
		throw null;
	}

	public static byte ToByte(decimal value)
	{
		throw null;
	}

	public static byte ToByte(double value)
	{
		throw null;
	}

	public static byte ToByte(short value)
	{
		throw null;
	}

	public static byte ToByte(int value)
	{
		throw null;
	}

	public static byte ToByte(long value)
	{
		throw null;
	}

	public static byte ToByte(object? value)
	{
		throw null;
	}

	public static byte ToByte(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static byte ToByte(sbyte value)
	{
		throw null;
	}

	public static byte ToByte(float value)
	{
		throw null;
	}

	public static byte ToByte(string? value)
	{
		throw null;
	}

	public static byte ToByte(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	public static byte ToByte(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static byte ToByte(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static byte ToByte(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static byte ToByte(ulong value)
	{
		throw null;
	}

	public static char ToChar(bool value)
	{
		throw null;
	}

	public static char ToChar(byte value)
	{
		throw null;
	}

	public static char ToChar(char value)
	{
		throw null;
	}

	public static char ToChar(DateTime value)
	{
		throw null;
	}

	public static char ToChar(decimal value)
	{
		throw null;
	}

	public static char ToChar(double value)
	{
		throw null;
	}

	public static char ToChar(short value)
	{
		throw null;
	}

	public static char ToChar(int value)
	{
		throw null;
	}

	public static char ToChar(long value)
	{
		throw null;
	}

	public static char ToChar(object? value)
	{
		throw null;
	}

	public static char ToChar(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static char ToChar(sbyte value)
	{
		throw null;
	}

	public static char ToChar(float value)
	{
		throw null;
	}

	public static char ToChar(string value)
	{
		throw null;
	}

	public static char ToChar(string value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static char ToChar(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static char ToChar(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static char ToChar(ulong value)
	{
		throw null;
	}

	public static DateTime ToDateTime(bool value)
	{
		throw null;
	}

	public static DateTime ToDateTime(byte value)
	{
		throw null;
	}

	public static DateTime ToDateTime(char value)
	{
		throw null;
	}

	public static DateTime ToDateTime(DateTime value)
	{
		throw null;
	}

	public static DateTime ToDateTime(decimal value)
	{
		throw null;
	}

	public static DateTime ToDateTime(double value)
	{
		throw null;
	}

	public static DateTime ToDateTime(short value)
	{
		throw null;
	}

	public static DateTime ToDateTime(int value)
	{
		throw null;
	}

	public static DateTime ToDateTime(long value)
	{
		throw null;
	}

	public static DateTime ToDateTime(object? value)
	{
		throw null;
	}

	public static DateTime ToDateTime(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static DateTime ToDateTime(sbyte value)
	{
		throw null;
	}

	public static DateTime ToDateTime(float value)
	{
		throw null;
	}

	public static DateTime ToDateTime(string? value)
	{
		throw null;
	}

	public static DateTime ToDateTime(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static DateTime ToDateTime(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static DateTime ToDateTime(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static DateTime ToDateTime(ulong value)
	{
		throw null;
	}

	public static decimal ToDecimal(bool value)
	{
		throw null;
	}

	public static decimal ToDecimal(byte value)
	{
		throw null;
	}

	public static decimal ToDecimal(char value)
	{
		throw null;
	}

	public static decimal ToDecimal(DateTime value)
	{
		throw null;
	}

	public static decimal ToDecimal(decimal value)
	{
		throw null;
	}

	public static decimal ToDecimal(double value)
	{
		throw null;
	}

	public static decimal ToDecimal(short value)
	{
		throw null;
	}

	public static decimal ToDecimal(int value)
	{
		throw null;
	}

	public static decimal ToDecimal(long value)
	{
		throw null;
	}

	public static decimal ToDecimal(object? value)
	{
		throw null;
	}

	public static decimal ToDecimal(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static decimal ToDecimal(sbyte value)
	{
		throw null;
	}

	public static decimal ToDecimal(float value)
	{
		throw null;
	}

	public static decimal ToDecimal(string? value)
	{
		throw null;
	}

	public static decimal ToDecimal(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static decimal ToDecimal(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static decimal ToDecimal(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static decimal ToDecimal(ulong value)
	{
		throw null;
	}

	public static double ToDouble(bool value)
	{
		throw null;
	}

	public static double ToDouble(byte value)
	{
		throw null;
	}

	public static double ToDouble(char value)
	{
		throw null;
	}

	public static double ToDouble(DateTime value)
	{
		throw null;
	}

	public static double ToDouble(decimal value)
	{
		throw null;
	}

	public static double ToDouble(double value)
	{
		throw null;
	}

	public static double ToDouble(short value)
	{
		throw null;
	}

	public static double ToDouble(int value)
	{
		throw null;
	}

	public static double ToDouble(long value)
	{
		throw null;
	}

	public static double ToDouble(object? value)
	{
		throw null;
	}

	public static double ToDouble(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static double ToDouble(sbyte value)
	{
		throw null;
	}

	public static double ToDouble(float value)
	{
		throw null;
	}

	public static double ToDouble(string? value)
	{
		throw null;
	}

	public static double ToDouble(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static double ToDouble(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static double ToDouble(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static double ToDouble(ulong value)
	{
		throw null;
	}

	public static string ToHexString(byte[] inArray)
	{
		throw null;
	}

	public static string ToHexString(byte[] inArray, int offset, int length)
	{
		throw null;
	}

	public static string ToHexString(ReadOnlySpan<byte> bytes)
	{
		throw null;
	}

	public static short ToInt16(bool value)
	{
		throw null;
	}

	public static short ToInt16(byte value)
	{
		throw null;
	}

	public static short ToInt16(char value)
	{
		throw null;
	}

	public static short ToInt16(DateTime value)
	{
		throw null;
	}

	public static short ToInt16(decimal value)
	{
		throw null;
	}

	public static short ToInt16(double value)
	{
		throw null;
	}

	public static short ToInt16(short value)
	{
		throw null;
	}

	public static short ToInt16(int value)
	{
		throw null;
	}

	public static short ToInt16(long value)
	{
		throw null;
	}

	public static short ToInt16(object? value)
	{
		throw null;
	}

	public static short ToInt16(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static short ToInt16(sbyte value)
	{
		throw null;
	}

	public static short ToInt16(float value)
	{
		throw null;
	}

	public static short ToInt16(string? value)
	{
		throw null;
	}

	public static short ToInt16(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	public static short ToInt16(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static short ToInt16(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static short ToInt16(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static short ToInt16(ulong value)
	{
		throw null;
	}

	public static int ToInt32(bool value)
	{
		throw null;
	}

	public static int ToInt32(byte value)
	{
		throw null;
	}

	public static int ToInt32(char value)
	{
		throw null;
	}

	public static int ToInt32(DateTime value)
	{
		throw null;
	}

	public static int ToInt32(decimal value)
	{
		throw null;
	}

	public static int ToInt32(double value)
	{
		throw null;
	}

	public static int ToInt32(short value)
	{
		throw null;
	}

	public static int ToInt32(int value)
	{
		throw null;
	}

	public static int ToInt32(long value)
	{
		throw null;
	}

	public static int ToInt32(object? value)
	{
		throw null;
	}

	public static int ToInt32(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int ToInt32(sbyte value)
	{
		throw null;
	}

	public static int ToInt32(float value)
	{
		throw null;
	}

	public static int ToInt32(string? value)
	{
		throw null;
	}

	public static int ToInt32(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	public static int ToInt32(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int ToInt32(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int ToInt32(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int ToInt32(ulong value)
	{
		throw null;
	}

	public static long ToInt64(bool value)
	{
		throw null;
	}

	public static long ToInt64(byte value)
	{
		throw null;
	}

	public static long ToInt64(char value)
	{
		throw null;
	}

	public static long ToInt64(DateTime value)
	{
		throw null;
	}

	public static long ToInt64(decimal value)
	{
		throw null;
	}

	public static long ToInt64(double value)
	{
		throw null;
	}

	public static long ToInt64(short value)
	{
		throw null;
	}

	public static long ToInt64(int value)
	{
		throw null;
	}

	public static long ToInt64(long value)
	{
		throw null;
	}

	public static long ToInt64(object? value)
	{
		throw null;
	}

	public static long ToInt64(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static long ToInt64(sbyte value)
	{
		throw null;
	}

	public static long ToInt64(float value)
	{
		throw null;
	}

	public static long ToInt64(string? value)
	{
		throw null;
	}

	public static long ToInt64(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	public static long ToInt64(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static long ToInt64(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static long ToInt64(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static long ToInt64(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(bool value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(byte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(char value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(DateTime value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(double value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(short value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(object? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(string? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(string value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(ulong value)
	{
		throw null;
	}

	public static float ToSingle(bool value)
	{
		throw null;
	}

	public static float ToSingle(byte value)
	{
		throw null;
	}

	public static float ToSingle(char value)
	{
		throw null;
	}

	public static float ToSingle(DateTime value)
	{
		throw null;
	}

	public static float ToSingle(decimal value)
	{
		throw null;
	}

	public static float ToSingle(double value)
	{
		throw null;
	}

	public static float ToSingle(short value)
	{
		throw null;
	}

	public static float ToSingle(int value)
	{
		throw null;
	}

	public static float ToSingle(long value)
	{
		throw null;
	}

	public static float ToSingle(object? value)
	{
		throw null;
	}

	public static float ToSingle(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static float ToSingle(sbyte value)
	{
		throw null;
	}

	public static float ToSingle(float value)
	{
		throw null;
	}

	public static float ToSingle(string? value)
	{
		throw null;
	}

	public static float ToSingle(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static float ToSingle(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static float ToSingle(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static float ToSingle(ulong value)
	{
		throw null;
	}

	public static string ToString(bool value)
	{
		throw null;
	}

	public static string ToString(bool value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(byte value)
	{
		throw null;
	}

	public static string ToString(byte value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(byte value, int toBase)
	{
		throw null;
	}

	public static string ToString(char value)
	{
		throw null;
	}

	public static string ToString(char value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(DateTime value)
	{
		throw null;
	}

	public static string ToString(DateTime value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(decimal value)
	{
		throw null;
	}

	public static string ToString(decimal value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(double value)
	{
		throw null;
	}

	public static string ToString(double value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(short value)
	{
		throw null;
	}

	public static string ToString(short value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(short value, int toBase)
	{
		throw null;
	}

	public static string ToString(int value)
	{
		throw null;
	}

	public static string ToString(int value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(int value, int toBase)
	{
		throw null;
	}

	public static string ToString(long value)
	{
		throw null;
	}

	public static string ToString(long value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(long value, int toBase)
	{
		throw null;
	}

	public static string? ToString(object? value)
	{
		throw null;
	}

	public static string? ToString(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(sbyte value, IFormatProvider? provider)
	{
		throw null;
	}

	public static string ToString(float value)
	{
		throw null;
	}

	public static string ToString(float value, IFormatProvider? provider)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static string? ToString(string? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static string? ToString(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ushort value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(uint value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ulong value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(bool value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(byte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(char value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(DateTime value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(double value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(short value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(object? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(string? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(bool value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(byte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(char value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(DateTime value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(double value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(short value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(object? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(string? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(ulong value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(bool value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(byte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(char value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(DateTime value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(decimal value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(double value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(short value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(object? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(object? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(string? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(string? value, IFormatProvider? provider)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(string? value, int fromBase)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(ulong value)
	{
		throw null;
	}

	public static bool TryFromBase64Chars(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryFromBase64String(string s, Span<byte> bytes, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryToBase64Chars(ReadOnlySpan<byte> bytes, Span<char> chars, out int charsWritten, Base64FormattingOptions options = Base64FormattingOptions.None)
	{
		throw null;
	}
}
