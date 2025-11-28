using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

public readonly struct StandardFormat : IEquatable<StandardFormat>
{
	private readonly int _dummyPrimitive;

	public const byte MaxPrecision = 99;

	public const byte NoPrecision = byte.MaxValue;

	public bool HasPrecision
	{
		get
		{
			throw null;
		}
	}

	public bool IsDefault
	{
		get
		{
			throw null;
		}
	}

	public byte Precision
	{
		get
		{
			throw null;
		}
	}

	public char Symbol
	{
		get
		{
			throw null;
		}
	}

	public StandardFormat(char symbol, byte precision = byte.MaxValue)
	{
		throw null;
	}

	public bool Equals(StandardFormat other)
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

	public static bool operator ==(StandardFormat left, StandardFormat right)
	{
		throw null;
	}

	public static implicit operator StandardFormat(char symbol)
	{
		throw null;
	}

	public static bool operator !=(StandardFormat left, StandardFormat right)
	{
		throw null;
	}

	public static StandardFormat Parse([StringSyntax("NumericFormat")] ReadOnlySpan<char> format)
	{
		throw null;
	}

	public static StandardFormat Parse([StringSyntax("NumericFormat")] string? format)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryParse([StringSyntax("NumericFormat")] ReadOnlySpan<char> format, out StandardFormat result)
	{
		throw null;
	}
}
