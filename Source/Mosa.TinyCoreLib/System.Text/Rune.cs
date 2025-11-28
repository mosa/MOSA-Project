using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Text;

public readonly struct Rune : IComparable, IComparable<Rune>, IEquatable<Rune>, IFormattable, ISpanFormattable, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public bool IsAscii
	{
		get
		{
			throw null;
		}
	}

	public bool IsBmp
	{
		get
		{
			throw null;
		}
	}

	public int Plane
	{
		get
		{
			throw null;
		}
	}

	public static Rune ReplacementChar
	{
		get
		{
			throw null;
		}
	}

	public int Utf16SequenceLength
	{
		get
		{
			throw null;
		}
	}

	public int Utf8SequenceLength
	{
		get
		{
			throw null;
		}
	}

	public int Value
	{
		get
		{
			throw null;
		}
	}

	public Rune(char ch)
	{
		throw null;
	}

	public Rune(char highSurrogate, char lowSurrogate)
	{
		throw null;
	}

	public Rune(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public Rune(uint value)
	{
		throw null;
	}

	public int CompareTo(Rune other)
	{
		throw null;
	}

	public static OperationStatus DecodeFromUtf16(ReadOnlySpan<char> source, out Rune result, out int charsConsumed)
	{
		throw null;
	}

	public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> source, out Rune result, out int bytesConsumed)
	{
		throw null;
	}

	public static OperationStatus DecodeLastFromUtf16(ReadOnlySpan<char> source, out Rune result, out int charsConsumed)
	{
		throw null;
	}

	public static OperationStatus DecodeLastFromUtf8(ReadOnlySpan<byte> source, out Rune value, out int bytesConsumed)
	{
		throw null;
	}

	public int EncodeToUtf16(Span<char> destination)
	{
		throw null;
	}

	public int EncodeToUtf8(Span<byte> destination)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(Rune other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static double GetNumericValue(Rune value)
	{
		throw null;
	}

	public static Rune GetRuneAt(string input, int index)
	{
		throw null;
	}

	public static UnicodeCategory GetUnicodeCategory(Rune value)
	{
		throw null;
	}

	public static bool IsControl(Rune value)
	{
		throw null;
	}

	public static bool IsDigit(Rune value)
	{
		throw null;
	}

	public static bool IsLetter(Rune value)
	{
		throw null;
	}

	public static bool IsLetterOrDigit(Rune value)
	{
		throw null;
	}

	public static bool IsLower(Rune value)
	{
		throw null;
	}

	public static bool IsNumber(Rune value)
	{
		throw null;
	}

	public static bool IsPunctuation(Rune value)
	{
		throw null;
	}

	public static bool IsSeparator(Rune value)
	{
		throw null;
	}

	public static bool IsSymbol(Rune value)
	{
		throw null;
	}

	public static bool IsUpper(Rune value)
	{
		throw null;
	}

	public static bool IsValid(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool IsValid(uint value)
	{
		throw null;
	}

	public static bool IsWhiteSpace(Rune value)
	{
		throw null;
	}

	public static bool operator ==(Rune left, Rune right)
	{
		throw null;
	}

	public static explicit operator Rune(char ch)
	{
		throw null;
	}

	public static explicit operator Rune(int value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Rune(uint value)
	{
		throw null;
	}

	public static bool operator >(Rune left, Rune right)
	{
		throw null;
	}

	public static bool operator >=(Rune left, Rune right)
	{
		throw null;
	}

	public static bool operator !=(Rune left, Rune right)
	{
		throw null;
	}

	public static bool operator <(Rune left, Rune right)
	{
		throw null;
	}

	public static bool operator <=(Rune left, Rune right)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	public static Rune ToLower(Rune value, CultureInfo culture)
	{
		throw null;
	}

	public static Rune ToLowerInvariant(Rune value)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static Rune ToUpper(Rune value, CultureInfo culture)
	{
		throw null;
	}

	public static Rune ToUpperInvariant(Rune value)
	{
		throw null;
	}

	public static bool TryCreate(char highSurrogate, char lowSurrogate, out Rune result)
	{
		throw null;
	}

	public static bool TryCreate(char ch, out Rune result)
	{
		throw null;
	}

	public static bool TryCreate(int value, out Rune result)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static bool TryCreate(uint value, out Rune result)
	{
		throw null;
	}

	public bool TryEncodeToUtf16(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public bool TryEncodeToUtf8(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryGetRuneAt(string input, int index, out Rune value)
	{
		throw null;
	}
}
