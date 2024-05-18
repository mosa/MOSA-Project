using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Boolean : IComparable, IComparable<bool>, IConvertible, IEquatable<bool>, IParsable<bool>, ISpanParsable<bool>
{
	private readonly bool _dummyPrimitive;

	public static readonly string FalseString;

	public static readonly string TrueString;

	public int CompareTo(bool value)
	{
		throw null;
	}

	public int CompareTo(object? obj)
	{
		throw null;
	}

	public bool Equals(bool obj)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public TypeCode GetTypeCode()
	{
		throw null;
	}

	public static bool Parse(ReadOnlySpan<char> value)
	{
		throw null;
	}

	public static bool Parse(string value)
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

	static bool IParsable<bool>.Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool IParsable<bool>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out bool result)
	{
		throw null;
	}

	static bool ISpanParsable<bool>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	static bool ISpanParsable<bool>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out bool result)
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

	public bool TryFormat(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> value, out bool result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? value, out bool result)
	{
		throw null;
	}
}
