using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System;

public sealed class String : IEnumerable<char>, IEnumerable, ICloneable, IComparable, IComparable<string?>, IConvertible, IEquatable<string?>, IParsable<string>, ISpanParsable<string>
{
	public static readonly string Empty;

	[IndexerName("Chars")]
	public char this[int index]
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public unsafe String(char* value)
	{
	}

	[CLSCompliant(false)]
	public unsafe String(char* value, int startIndex, int length)
	{
	}

	public String(char c, int count)
	{
	}

	public String(char[]? value)
	{
	}

	public String(char[] value, int startIndex, int length)
	{
	}

	public String(ReadOnlySpan<char> value)
	{
	}

	[CLSCompliant(false)]
	public unsafe String(sbyte* value)
	{
	}

	[CLSCompliant(false)]
	public unsafe String(sbyte* value, int startIndex, int length)
	{
	}

	[CLSCompliant(false)]
	public unsafe String(sbyte* value, int startIndex, int length, Encoding enc)
	{
	}

	public object Clone()
	{
		throw null;
	}

	public static int Compare(string? strA, int indexA, string? strB, int indexB, int length)
	{
		throw null;
	}

	public static int Compare(string? strA, int indexA, string? strB, int indexB, int length, bool ignoreCase)
	{
		throw null;
	}

	public static int Compare(string? strA, int indexA, string? strB, int indexB, int length, bool ignoreCase, CultureInfo? culture)
	{
		throw null;
	}

	public static int Compare(string? strA, int indexA, string? strB, int indexB, int length, CultureInfo? culture, CompareOptions options)
	{
		throw null;
	}

	public static int Compare(string? strA, int indexA, string? strB, int indexB, int length, StringComparison comparisonType)
	{
		throw null;
	}

	public static int Compare(string? strA, string? strB)
	{
		throw null;
	}

	public static int Compare(string? strA, string? strB, bool ignoreCase)
	{
		throw null;
	}

	public static int Compare(string? strA, string? strB, bool ignoreCase, CultureInfo? culture)
	{
		throw null;
	}

	public static int Compare(string? strA, string? strB, CultureInfo? culture, CompareOptions options)
	{
		throw null;
	}

	public static int Compare(string? strA, string? strB, StringComparison comparisonType)
	{
		throw null;
	}

	public static int CompareOrdinal(string? strA, int indexA, string? strB, int indexB, int length)
	{
		throw null;
	}

	public static int CompareOrdinal(string? strA, string? strB)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(string? strB)
	{
		throw null;
	}

	public static string Concat(IEnumerable<string?> values)
	{
		throw null;
	}

	public static string Concat(object? arg0)
	{
		throw null;
	}

	public static string Concat(object? arg0, object? arg1)
	{
		throw null;
	}

	public static string Concat(object? arg0, object? arg1, object? arg2)
	{
		throw null;
	}

	public static string Concat(params object?[] args)
	{
		throw null;
	}

	public static string Concat(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1)
	{
		throw null;
	}

	public static string Concat(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1, ReadOnlySpan<char> str2)
	{
		throw null;
	}

	public static string Concat(ReadOnlySpan<char> str0, ReadOnlySpan<char> str1, ReadOnlySpan<char> str2, ReadOnlySpan<char> str3)
	{
		throw null;
	}

	public static string Concat(string? str0, string? str1)
	{
		throw null;
	}

	public static string Concat(string? str0, string? str1, string? str2)
	{
		throw null;
	}

	public static string Concat(string? str0, string? str1, string? str2, string? str3)
	{
		throw null;
	}

	public static string Concat(params string?[] values)
	{
		throw null;
	}

	public static string Concat<T>(IEnumerable<T> values)
	{
		throw null;
	}

	public bool Contains(char value)
	{
		throw null;
	}

	public bool Contains(char value, StringComparison comparisonType)
	{
		throw null;
	}

	public bool Contains(string value)
	{
		throw null;
	}

	public bool Contains(string value, StringComparison comparisonType)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API should not be used to create mutable strings. See https://go.microsoft.com/fwlink/?linkid=2084035 for alternatives.")]
	public static string Copy(string str)
	{
		throw null;
	}

	public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
	{
	}

	public void CopyTo(Span<char> destination)
	{
	}

	public static string Create(IFormatProvider? provider, [InterpolatedStringHandlerArgument("provider")] ref DefaultInterpolatedStringHandler handler)
	{
		throw null;
	}

	public static string Create(IFormatProvider? provider, Span<char> initialBuffer, [InterpolatedStringHandlerArgument(new string[] { "provider", "initialBuffer" })] ref DefaultInterpolatedStringHandler handler)
	{
		throw null;
	}

	public static string Create<TState>(int length, TState state, SpanAction<char, TState> action)
	{
		throw null;
	}

	public bool EndsWith(char value)
	{
		throw null;
	}

	public bool EndsWith(string value)
	{
		throw null;
	}

	public bool EndsWith(string value, bool ignoreCase, CultureInfo? culture)
	{
		throw null;
	}

	public bool EndsWith(string value, StringComparison comparisonType)
	{
		throw null;
	}

	public StringRuneEnumerator EnumerateRunes()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] string? value)
	{
		throw null;
	}

	public static bool Equals(string? a, string? b)
	{
		throw null;
	}

	public static bool Equals(string? a, string? b, StringComparison comparisonType)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] string? value, StringComparison comparisonType)
	{
		throw null;
	}

	public static string Format(IFormatProvider? provider, [StringSyntax("CompositeFormat")] string format, object? arg0)
	{
		throw null;
	}

	public static string Format(IFormatProvider? provider, [StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
		throw null;
	}

	public static string Format(IFormatProvider? provider, [StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
		throw null;
	}

	public static string Format(IFormatProvider? provider, [StringSyntax("CompositeFormat")] string format, params object?[] args)
	{
		throw null;
	}

	public static string Format([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
		throw null;
	}

	public static string Format([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
		throw null;
	}

	public static string Format([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
		throw null;
	}

	public static string Format([StringSyntax("CompositeFormat")] string format, params object?[] args)
	{
		throw null;
	}

	public static string Format<TArg0>(IFormatProvider? provider, CompositeFormat format, TArg0 arg0)
	{
		throw null;
	}

	public static string Format<TArg0, TArg1>(IFormatProvider? provider, CompositeFormat format, TArg0 arg0, TArg1 arg1)
	{
		throw null;
	}

	public static string Format<TArg0, TArg1, TArg2>(IFormatProvider? provider, CompositeFormat format, TArg0 arg0, TArg1 arg1, TArg2 arg2)
	{
		throw null;
	}

	public static string Format(IFormatProvider? provider, CompositeFormat format, params object?[] args)
	{
		throw null;
	}

	public static string Format(IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args)
	{
		throw null;
	}

	public CharEnumerator GetEnumerator()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static int GetHashCode(ReadOnlySpan<char> value)
	{
		throw null;
	}

	public static int GetHashCode(ReadOnlySpan<char> value, StringComparison comparisonType)
	{
		throw null;
	}

	public int GetHashCode(StringComparison comparisonType)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ref readonly char GetPinnableReference()
	{
		throw null;
	}

	public TypeCode GetTypeCode()
	{
		throw null;
	}

	public int IndexOf(char value)
	{
		throw null;
	}

	public int IndexOf(char value, int startIndex)
	{
		throw null;
	}

	public int IndexOf(char value, int startIndex, int count)
	{
		throw null;
	}

	public int IndexOf(char value, StringComparison comparisonType)
	{
		throw null;
	}

	public int IndexOf(string value)
	{
		throw null;
	}

	public int IndexOf(string value, int startIndex)
	{
		throw null;
	}

	public int IndexOf(string value, int startIndex, int count)
	{
		throw null;
	}

	public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
	{
		throw null;
	}

	public int IndexOf(string value, int startIndex, StringComparison comparisonType)
	{
		throw null;
	}

	public int IndexOf(string value, StringComparison comparisonType)
	{
		throw null;
	}

	public int IndexOfAny(char[] anyOf)
	{
		throw null;
	}

	public int IndexOfAny(char[] anyOf, int startIndex)
	{
		throw null;
	}

	public int IndexOfAny(char[] anyOf, int startIndex, int count)
	{
		throw null;
	}

	public string Insert(int startIndex, string value)
	{
		throw null;
	}

	public static string Intern(string str)
	{
		throw null;
	}

	public static string? IsInterned(string str)
	{
		throw null;
	}

	public bool IsNormalized()
	{
		throw null;
	}

	public bool IsNormalized(NormalizationForm normalizationForm)
	{
		throw null;
	}

	public static bool IsNullOrEmpty([NotNullWhen(false)] string? value)
	{
		throw null;
	}

	public static bool IsNullOrWhiteSpace([NotNullWhen(false)] string? value)
	{
		throw null;
	}

	public static string Join(char separator, params object?[] values)
	{
		throw null;
	}

	public static string Join(char separator, params string?[] value)
	{
		throw null;
	}

	public static string Join(char separator, string?[] value, int startIndex, int count)
	{
		throw null;
	}

	public static string Join(string? separator, IEnumerable<string?> values)
	{
		throw null;
	}

	public static string Join(string? separator, params object?[] values)
	{
		throw null;
	}

	public static string Join(string? separator, params string?[] value)
	{
		throw null;
	}

	public static string Join(string? separator, string?[] value, int startIndex, int count)
	{
		throw null;
	}

	public static string Join<T>(char separator, IEnumerable<T> values)
	{
		throw null;
	}

	public static string Join<T>(string? separator, IEnumerable<T> values)
	{
		throw null;
	}

	public int LastIndexOf(char value)
	{
		throw null;
	}

	public int LastIndexOf(char value, int startIndex)
	{
		throw null;
	}

	public int LastIndexOf(char value, int startIndex, int count)
	{
		throw null;
	}

	public int LastIndexOf(string value)
	{
		throw null;
	}

	public int LastIndexOf(string value, int startIndex)
	{
		throw null;
	}

	public int LastIndexOf(string value, int startIndex, int count)
	{
		throw null;
	}

	public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
	{
		throw null;
	}

	public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
	{
		throw null;
	}

	public int LastIndexOf(string value, StringComparison comparisonType)
	{
		throw null;
	}

	public int LastIndexOfAny(char[] anyOf)
	{
		throw null;
	}

	public int LastIndexOfAny(char[] anyOf, int startIndex)
	{
		throw null;
	}

	public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
	{
		throw null;
	}

	public string Normalize()
	{
		throw null;
	}

	public string Normalize(NormalizationForm normalizationForm)
	{
		throw null;
	}

	public static bool operator ==(string? a, string? b)
	{
		throw null;
	}

	public static implicit operator ReadOnlySpan<char>(string? value)
	{
		throw null;
	}

	public static bool operator !=(string? a, string? b)
	{
		throw null;
	}

	public string PadLeft(int totalWidth)
	{
		throw null;
	}

	public string PadLeft(int totalWidth, char paddingChar)
	{
		throw null;
	}

	public string PadRight(int totalWidth)
	{
		throw null;
	}

	public string PadRight(int totalWidth, char paddingChar)
	{
		throw null;
	}

	public string Remove(int startIndex)
	{
		throw null;
	}

	public string Remove(int startIndex, int count)
	{
		throw null;
	}

	public string Replace(char oldChar, char newChar)
	{
		throw null;
	}

	public string Replace(string oldValue, string? newValue)
	{
		throw null;
	}

	public string Replace(string oldValue, string? newValue, bool ignoreCase, CultureInfo? culture)
	{
		throw null;
	}

	public string Replace(string oldValue, string? newValue, StringComparison comparisonType)
	{
		throw null;
	}

	public string ReplaceLineEndings()
	{
		throw null;
	}

	public string ReplaceLineEndings(string replacementText)
	{
		throw null;
	}

	public string[] Split(char separator, int count, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public string[] Split(char separator, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public string[] Split(params char[]? separator)
	{
		throw null;
	}

	public string[] Split(char[]? separator, int count)
	{
		throw null;
	}

	public string[] Split(char[]? separator, int count, StringSplitOptions options)
	{
		throw null;
	}

	public string[] Split(char[]? separator, StringSplitOptions options)
	{
		throw null;
	}

	public string[] Split(string? separator, int count, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public string[] Split(string? separator, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public string[] Split(string[]? separator, int count, StringSplitOptions options)
	{
		throw null;
	}

	public string[] Split(string[]? separator, StringSplitOptions options)
	{
		throw null;
	}

	public bool StartsWith(char value)
	{
		throw null;
	}

	public bool StartsWith(string value)
	{
		throw null;
	}

	public bool StartsWith(string value, bool ignoreCase, CultureInfo? culture)
	{
		throw null;
	}

	public bool StartsWith(string value, StringComparison comparisonType)
	{
		throw null;
	}

	public string Substring(int startIndex)
	{
		throw null;
	}

	public string Substring(int startIndex, int length)
	{
		throw null;
	}

	IEnumerator<char> IEnumerable<char>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	bool IConvertible.ToBoolean(IFormatProvider? provider)
	{
		throw null;
	}

	byte IConvertible.ToByte(IFormatProvider? provider)
	{
		throw null;
	}

	char IConvertible.ToChar(IFormatProvider? provider)
	{
		throw null;
	}

	DateTime IConvertible.ToDateTime(IFormatProvider? provider)
	{
		throw null;
	}

	decimal IConvertible.ToDecimal(IFormatProvider? provider)
	{
		throw null;
	}

	double IConvertible.ToDouble(IFormatProvider? provider)
	{
		throw null;
	}

	short IConvertible.ToInt16(IFormatProvider? provider)
	{
		throw null;
	}

	int IConvertible.ToInt32(IFormatProvider? provider)
	{
		throw null;
	}

	long IConvertible.ToInt64(IFormatProvider? provider)
	{
		throw null;
	}

	sbyte IConvertible.ToSByte(IFormatProvider? provider)
	{
		throw null;
	}

	float IConvertible.ToSingle(IFormatProvider? provider)
	{
		throw null;
	}

	object IConvertible.ToType(Type type, IFormatProvider? provider)
	{
		throw null;
	}

	ushort IConvertible.ToUInt16(IFormatProvider? provider)
	{
		throw null;
	}

	uint IConvertible.ToUInt32(IFormatProvider? provider)
	{
		throw null;
	}

	ulong IConvertible.ToUInt64(IFormatProvider? provider)
	{
		throw null;
	}

	static string IParsable<string>.Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool IParsable<string>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out string result)
	{
		throw null;
	}

	static string ISpanParsable<string>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool ISpanParsable<string>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out string result)
	{
		throw null;
	}

	public char[] ToCharArray()
	{
		throw null;
	}

	public char[] ToCharArray(int startIndex, int length)
	{
		throw null;
	}

	public string ToLower()
	{
		throw null;
	}

	public string ToLower(CultureInfo? culture)
	{
		throw null;
	}

	public string ToLowerInvariant()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString(IFormatProvider? provider)
	{
		throw null;
	}

	public string ToUpper()
	{
		throw null;
	}

	public string ToUpper(CultureInfo? culture)
	{
		throw null;
	}

	public string ToUpperInvariant()
	{
		throw null;
	}

	public string Trim()
	{
		throw null;
	}

	public string Trim(char trimChar)
	{
		throw null;
	}

	public string Trim(params char[]? trimChars)
	{
		throw null;
	}

	public string TrimEnd()
	{
		throw null;
	}

	public string TrimEnd(char trimChar)
	{
		throw null;
	}

	public string TrimEnd(params char[]? trimChars)
	{
		throw null;
	}

	public string TrimStart()
	{
		throw null;
	}

	public string TrimStart(char trimChar)
	{
		throw null;
	}

	public string TrimStart(params char[]? trimChars)
	{
		throw null;
	}

	public bool TryCopyTo(Span<char> destination)
	{
		throw null;
	}
}
