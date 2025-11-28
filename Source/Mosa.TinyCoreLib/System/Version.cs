using System.Diagnostics.CodeAnalysis;

namespace System;

public sealed class Version : ICloneable, IComparable, IComparable<Version?>, IEquatable<Version?>, IFormattable, ISpanFormattable, IUtf8SpanFormattable
{
	public int Build
	{
		get
		{
			throw null;
		}
	}

	public int Major
	{
		get
		{
			throw null;
		}
	}

	public short MajorRevision
	{
		get
		{
			throw null;
		}
	}

	public int Minor
	{
		get
		{
			throw null;
		}
	}

	public short MinorRevision
	{
		get
		{
			throw null;
		}
	}

	public int Revision
	{
		get
		{
			throw null;
		}
	}

	public Version()
	{
	}

	public Version(int major, int minor)
	{
	}

	public Version(int major, int minor, int build)
	{
	}

	public Version(int major, int minor, int build, int revision)
	{
	}

	public Version(string version)
	{
	}

	public object Clone()
	{
		throw null;
	}

	public int CompareTo(object? version)
	{
		throw null;
	}

	public int CompareTo(Version? value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] Version? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(Version? v1, Version? v2)
	{
		throw null;
	}

	public static bool operator >(Version? v1, Version? v2)
	{
		throw null;
	}

	public static bool operator >=(Version? v1, Version? v2)
	{
		throw null;
	}

	public static bool operator !=(Version? v1, Version? v2)
	{
		throw null;
	}

	public static bool operator <(Version? v1, Version? v2)
	{
		throw null;
	}

	public static bool operator <=(Version? v1, Version? v2)
	{
		throw null;
	}

	public static Version Parse(ReadOnlySpan<char> input)
	{
		throw null;
	}

	public static Version Parse(string input)
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

	public override string ToString()
	{
		throw null;
	}

	public string ToString(int fieldCount)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, int fieldCount, out int charsWritten)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, int fieldCount, out int bytesWritten)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> input, [NotNullWhen(true)] out Version? result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out Version? result)
	{
		throw null;
	}
}
