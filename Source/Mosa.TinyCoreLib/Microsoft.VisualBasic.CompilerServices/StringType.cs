using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class StringType
{
	internal StringType()
	{
	}

	public static string FromBoolean(bool Value)
	{
		throw null;
	}

	public static string FromByte(byte Value)
	{
		throw null;
	}

	public static string FromChar(char Value)
	{
		throw null;
	}

	public static string FromDate(DateTime Value)
	{
		throw null;
	}

	public static string FromDecimal(decimal Value)
	{
		throw null;
	}

	public static string FromDecimal(decimal Value, NumberFormatInfo? NumberFormat)
	{
		throw null;
	}

	public static string FromDouble(double Value)
	{
		throw null;
	}

	public static string FromDouble(double Value, NumberFormatInfo? NumberFormat)
	{
		throw null;
	}

	public static string FromInteger(int Value)
	{
		throw null;
	}

	public static string FromLong(long Value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("Value")]
	public static string? FromObject(object? Value)
	{
		throw null;
	}

	public static string FromShort(short Value)
	{
		throw null;
	}

	public static string FromSingle(float Value)
	{
		throw null;
	}

	public static string FromSingle(float Value, NumberFormatInfo? NumberFormat)
	{
		throw null;
	}

	public static void MidStmtStr(ref string? sDest, int StartPosition, int MaxInsertLength, string sInsert)
	{
	}

	public static int StrCmp(string? sLeft, string? sRight, bool TextCompare)
	{
		throw null;
	}

	public static bool StrLike(string? Source, string? Pattern, CompareMethod CompareOption)
	{
		throw null;
	}

	public static bool StrLikeBinary(string? Source, string? Pattern)
	{
		throw null;
	}

	public static bool StrLikeText(string? Source, string? Pattern)
	{
		throw null;
	}
}
