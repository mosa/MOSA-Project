using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class Conversions
{
	internal Conversions()
	{
	}

	[RequiresUnreferencedCode("The Expression origin object cannot be statically analyzed and may be trimmed")]
	public static object? ChangeType(object? Expression, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type TargetType)
	{
		throw null;
	}

	[Obsolete("FallbackUserDefinedConversion has been deprecated and is not supported.", true)]
	[RequiresUnreferencedCode("The Expression origin object cannot be statically analyzed and may be trimmed")]
	public static object FallbackUserDefinedConversion(object Expression, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type TargetType)
	{
		throw null;
	}

	public static string FromCharAndCount(char Value, int Count)
	{
		throw null;
	}

	public static string FromCharArray(char[] Value)
	{
		throw null;
	}

	public static string FromCharArraySubset(char[] Value, int StartIndex, int Length)
	{
		throw null;
	}

	public static bool ToBoolean(object? Value)
	{
		throw null;
	}

	public static bool ToBoolean(string? Value)
	{
		throw null;
	}

	public static byte ToByte(object? Value)
	{
		throw null;
	}

	public static byte ToByte(string? Value)
	{
		throw null;
	}

	public static char ToChar(object? Value)
	{
		throw null;
	}

	public static char ToChar(string? Value)
	{
		throw null;
	}

	public static char[] ToCharArrayRankOne(object? Value)
	{
		throw null;
	}

	public static char[] ToCharArrayRankOne(string? Value)
	{
		throw null;
	}

	public static DateTime ToDate(object? Value)
	{
		throw null;
	}

	public static DateTime ToDate(string? Value)
	{
		throw null;
	}

	public static decimal ToDecimal(bool Value)
	{
		throw null;
	}

	public static decimal ToDecimal(object? Value)
	{
		throw null;
	}

	public static decimal ToDecimal(string? Value)
	{
		throw null;
	}

	public static double ToDouble(object? Value)
	{
		throw null;
	}

	public static double ToDouble(string? Value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("Value")]
	public static T? ToGenericParameter<T>(object? Value)
	{
		throw null;
	}

	public static int ToInteger(object? Value)
	{
		throw null;
	}

	public static int ToInteger(string? Value)
	{
		throw null;
	}

	public static long ToLong(object? Value)
	{
		throw null;
	}

	public static long ToLong(string? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(object? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(string? Value)
	{
		throw null;
	}

	public static short ToShort(object? Value)
	{
		throw null;
	}

	public static short ToShort(string? Value)
	{
		throw null;
	}

	public static float ToSingle(object? Value)
	{
		throw null;
	}

	public static float ToSingle(string? Value)
	{
		throw null;
	}

	public static string ToString(bool Value)
	{
		throw null;
	}

	public static string ToString(byte Value)
	{
		throw null;
	}

	public static string ToString(char Value)
	{
		throw null;
	}

	public static string ToString(DateTime Value)
	{
		throw null;
	}

	public static string ToString(decimal Value)
	{
		throw null;
	}

	public static string ToString(decimal Value, NumberFormatInfo? NumberFormat)
	{
		throw null;
	}

	public static string ToString(double Value)
	{
		throw null;
	}

	public static string ToString(double Value, NumberFormatInfo? NumberFormat)
	{
		throw null;
	}

	public static string ToString(short Value)
	{
		throw null;
	}

	public static string ToString(int Value)
	{
		throw null;
	}

	public static string ToString(long Value)
	{
		throw null;
	}

	public static string? ToString(object? Value)
	{
		throw null;
	}

	public static string ToString(float Value)
	{
		throw null;
	}

	public static string ToString(float Value, NumberFormatInfo? NumberFormat)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(uint Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ulong Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInteger(object? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInteger(string? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToULong(object? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToULong(string? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUShort(object? Value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUShort(string? Value)
	{
		throw null;
	}
}
