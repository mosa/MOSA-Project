using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

public readonly struct TimeOnly : IComparable, IComparable<TimeOnly>, IEquatable<TimeOnly>, IFormattable, IParsable<TimeOnly>, ISpanFormattable, ISpanParsable<TimeOnly>, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public int Hour
	{
		get
		{
			throw null;
		}
	}

	public static TimeOnly MaxValue
	{
		get
		{
			throw null;
		}
	}

	public int Microsecond
	{
		get
		{
			throw null;
		}
	}

	public int Millisecond
	{
		get
		{
			throw null;
		}
	}

	public int Minute
	{
		get
		{
			throw null;
		}
	}

	public static TimeOnly MinValue
	{
		get
		{
			throw null;
		}
	}

	public int Nanosecond
	{
		get
		{
			throw null;
		}
	}

	public int Second
	{
		get
		{
			throw null;
		}
	}

	public long Ticks
	{
		get
		{
			throw null;
		}
	}

	public TimeOnly(int hour, int minute)
	{
		throw null;
	}

	public TimeOnly(int hour, int minute, int second)
	{
		throw null;
	}

	public TimeOnly(int hour, int minute, int second, int millisecond)
	{
		throw null;
	}

	public TimeOnly(int hour, int minute, int second, int millisecond, int microsecond)
	{
		throw null;
	}

	public TimeOnly(long ticks)
	{
		throw null;
	}

	public TimeOnly Add(TimeSpan value)
	{
		throw null;
	}

	public TimeOnly Add(TimeSpan value, out int wrappedDays)
	{
		throw null;
	}

	public TimeOnly AddHours(double value)
	{
		throw null;
	}

	public TimeOnly AddHours(double value, out int wrappedDays)
	{
		throw null;
	}

	public TimeOnly AddMinutes(double value)
	{
		throw null;
	}

	public TimeOnly AddMinutes(double value, out int wrappedDays)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(TimeOnly value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public bool Equals(TimeOnly value)
	{
		throw null;
	}

	public static TimeOnly FromDateTime(DateTime dateTime)
	{
		throw null;
	}

	public static TimeOnly FromTimeSpan(TimeSpan timeSpan)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public bool IsBetween(TimeOnly start, TimeOnly end)
	{
		throw null;
	}

	public static bool operator ==(TimeOnly left, TimeOnly right)
	{
		throw null;
	}

	public static bool operator >(TimeOnly left, TimeOnly right)
	{
		throw null;
	}

	public static bool operator >=(TimeOnly left, TimeOnly right)
	{
		throw null;
	}

	public static bool operator !=(TimeOnly left, TimeOnly right)
	{
		throw null;
	}

	public static bool operator <(TimeOnly left, TimeOnly right)
	{
		throw null;
	}

	public static bool operator <=(TimeOnly left, TimeOnly right)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out int hour, out int minute)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out int hour, out int minute, out int second)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out int hour, out int minute, out int second, out int millisecond)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out int hour, out int minute, out int second, out int millisecond, out int microsecond)
	{
		throw null;
	}

	public static TimeSpan operator -(TimeOnly t1, TimeOnly t2)
	{
		throw null;
	}

	public static TimeOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static TimeOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static TimeOnly Parse(string s)
	{
		throw null;
	}

	public static TimeOnly Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static TimeOnly Parse(string s, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static TimeOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax("TimeOnlyFormat")] ReadOnlySpan<char> format, IFormatProvider? provider = null, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static TimeOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax("TimeOnlyFormat")] string[] formats)
	{
		throw null;
	}

	public static TimeOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax("TimeOnlyFormat")] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static TimeOnly ParseExact(string s, [StringSyntax("TimeOnlyFormat")] string format)
	{
		throw null;
	}

	public static TimeOnly ParseExact(string s, [StringSyntax("TimeOnlyFormat")] string format, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static TimeOnly ParseExact(string s, [StringSyntax("TimeOnlyFormat")] string[] formats)
	{
		throw null;
	}

	public static TimeOnly ParseExact(string s, [StringSyntax("TimeOnlyFormat")] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public string ToLongTimeString()
	{
		throw null;
	}

	public string ToShortTimeString()
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

	public string ToString([StringSyntax("TimeOnlyFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("TimeOnlyFormat")] string? format, IFormatProvider? provider)
	{
		throw null;
	}

	public TimeSpan ToTimeSpan()
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("TimeOnlyFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("TimeOnlyFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax("TimeOnlyFormat")] ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax("TimeOnlyFormat")] ReadOnlySpan<char> format, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)][StringSyntax("TimeOnlyFormat")] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)][StringSyntax("TimeOnlyFormat")] string?[]? formats, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("TimeOnlyFormat")] string? format, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("TimeOnlyFormat")] string? format, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("TimeOnlyFormat")] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("TimeOnlyFormat")] string?[]? formats, out TimeOnly result)
	{
		throw null;
	}
}
