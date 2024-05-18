using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Guid : IComparable, IComparable<Guid>, IEquatable<Guid>, IFormattable, IParsable<Guid>, ISpanFormattable, ISpanParsable<Guid>, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public static readonly Guid Empty;

	public Guid(byte[] b)
	{
		throw null;
	}

	public Guid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
	{
		throw null;
	}

	public Guid(int a, short b, short c, byte[] d)
	{
		throw null;
	}

	public Guid(ReadOnlySpan<byte> b)
	{
		throw null;
	}

	public Guid(ReadOnlySpan<byte> b, bool bigEndian)
	{
		throw null;
	}

	public Guid(string g)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public Guid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
	{
		throw null;
	}

	public int CompareTo(Guid value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public bool Equals(Guid g)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static Guid NewGuid()
	{
		throw null;
	}

	public static bool operator ==(Guid a, Guid b)
	{
		throw null;
	}

	public static bool operator >(Guid left, Guid right)
	{
		throw null;
	}

	public static bool operator >=(Guid left, Guid right)
	{
		throw null;
	}

	public static bool operator !=(Guid a, Guid b)
	{
		throw null;
	}

	public static bool operator <(Guid left, Guid right)
	{
		throw null;
	}

	public static bool operator <=(Guid left, Guid right)
	{
		throw null;
	}

	public static Guid Parse(ReadOnlySpan<char> input)
	{
		throw null;
	}

	public static Guid Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Guid Parse(string input)
	{
		throw null;
	}

	public static Guid Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static Guid ParseExact(ReadOnlySpan<char> input, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format)
	{
		throw null;
	}

	public static Guid ParseExact(string input, [StringSyntax("GuidFormat")] string format)
	{
		throw null;
	}

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	public byte[] ToByteArray()
	{
		throw null;
	}

	public byte[] ToByteArray(bool bigEndian)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString([StringSyntax("GuidFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("GuidFormat")] string? format, IFormatProvider? provider)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>))
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>))
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> input, out Guid result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Guid result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, out Guid result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Guid result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format, out Guid result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("GuidFormat")] string? format, out Guid result)
	{
		throw null;
	}

	public bool TryWriteBytes(Span<byte> destination)
	{
		throw null;
	}

	public bool TryWriteBytes(Span<byte> destination, bool bigEndian, out int bytesWritten)
	{
		throw null;
	}
}
