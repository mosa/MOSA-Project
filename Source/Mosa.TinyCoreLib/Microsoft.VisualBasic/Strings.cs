using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class Strings
{
	internal Strings()
	{
	}

	public static int Asc(char String)
	{
		throw null;
	}

	public static int Asc(string String)
	{
		throw null;
	}

	public static int AscW(char String)
	{
		throw null;
	}

	public static int AscW(string String)
	{
		throw null;
	}

	public static char Chr(int CharCode)
	{
		throw null;
	}

	public static char ChrW(int CharCode)
	{
		throw null;
	}

	public static string[]? Filter(object?[] Source, string? Match, bool Include = true, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static string[]? Filter(string?[] Source, string? Match, bool Include = true, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static string Format(object? Expression, string? Style = "")
	{
		throw null;
	}

	public static string FormatCurrency(object? Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
	{
		throw null;
	}

	public static string FormatDateTime(DateTime Expression, DateFormat NamedFormat = DateFormat.GeneralDate)
	{
		throw null;
	}

	public static string FormatNumber(object? Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
	{
		throw null;
	}

	public static string FormatPercent(object? Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
	{
		throw null;
	}

	public static char GetChar(string str, int Index)
	{
		throw null;
	}

	public static int InStr(int Start, string? String1, string? String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static int InStr(string? String1, string? String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static int InStrRev(string? StringCheck, string? StringMatch, int Start = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static string? Join(object?[] SourceArray, string? Delimiter = " ")
	{
		throw null;
	}

	public static string? Join(string?[] SourceArray, string? Delimiter = " ")
	{
		throw null;
	}

	public static char LCase(char Value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("Value")]
	public static string? LCase(string? Value)
	{
		throw null;
	}

	public static string Left(string? str, int Length)
	{
		throw null;
	}

	public static int Len(bool Expression)
	{
		throw null;
	}

	public static int Len(byte Expression)
	{
		throw null;
	}

	public static int Len(char Expression)
	{
		throw null;
	}

	public static int Len(DateTime Expression)
	{
		throw null;
	}

	public static int Len(decimal Expression)
	{
		throw null;
	}

	public static int Len(double Expression)
	{
		throw null;
	}

	public static int Len(short Expression)
	{
		throw null;
	}

	public static int Len(int Expression)
	{
		throw null;
	}

	public static int Len(long Expression)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The object's type cannot be statically analyzed and its members may be trimmed")]
	public static int Len(object? Expression)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int Len(sbyte Expression)
	{
		throw null;
	}

	public static int Len(float Expression)
	{
		throw null;
	}

	public static int Len(string? Expression)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int Len(ushort Expression)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int Len(uint Expression)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static int Len(ulong Expression)
	{
		throw null;
	}

	public static string LSet(string? Source, int Length)
	{
		throw null;
	}

	public static string LTrim(string? str)
	{
		throw null;
	}

	public static string? Mid(string? str, int Start)
	{
		throw null;
	}

	public static string Mid(string? str, int Start, int Length)
	{
		throw null;
	}

	public static string? Replace(string? Expression, string? Find, string? Replacement, int Start = 1, int Count = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static string Right(string? str, int Length)
	{
		throw null;
	}

	public static string RSet(string? Source, int Length)
	{
		throw null;
	}

	public static string RTrim(string? str)
	{
		throw null;
	}

	public static string Space(int Number)
	{
		throw null;
	}

	public static string[] Split(string? Expression, string? Delimiter = " ", int Limit = -1, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	public static int StrComp(string? String1, string? String2, [OptionCompare] CompareMethod Compare = CompareMethod.Binary)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static string? StrConv(string? str, VbStrConv Conversion, int LocaleID = 0)
	{
		throw null;
	}

	public static string StrDup(int Number, char Character)
	{
		throw null;
	}

	public static object StrDup(int Number, object Character)
	{
		throw null;
	}

	public static string StrDup(int Number, string Character)
	{
		throw null;
	}

	public static string StrReverse(string? Expression)
	{
		throw null;
	}

	public static string Trim(string? str)
	{
		throw null;
	}

	public static char UCase(char Value)
	{
		throw null;
	}

	public static string UCase(string? Value)
	{
		throw null;
	}
}
