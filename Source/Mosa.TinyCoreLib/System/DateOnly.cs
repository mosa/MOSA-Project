using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

public readonly struct DateOnly : IComparable, IComparable<DateOnly>, IEquatable<DateOnly>, IFormattable, IParsable<DateOnly>, ISpanFormattable, ISpanParsable<DateOnly>, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public int Day
	{
		get
		{
			throw null;
		}
	}

	public int DayNumber
	{
		get
		{
			throw null;
		}
	}

	public DayOfWeek DayOfWeek
	{
		get
		{
			throw null;
		}
	}

	public int DayOfYear
	{
		get
		{
			throw null;
		}
	}

	public static DateOnly MaxValue
	{
		get
		{
			throw null;
		}
	}

	public static DateOnly MinValue
	{
		get
		{
			throw null;
		}
	}

	public int Month
	{
		get
		{
			throw null;
		}
	}

	public int Year
	{
		get
		{
			throw null;
		}
	}

	public DateOnly(int year, int month, int day)
	{
		throw null;
	}

	public DateOnly(int year, int month, int day, Calendar calendar)
	{
		throw null;
	}

	public DateOnly AddDays(int value)
	{
		throw null;
	}

	public DateOnly AddMonths(int value)
	{
		throw null;
	}

	public DateOnly AddYears(int value)
	{
		throw null;
	}

	public int CompareTo(DateOnly value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public bool Equals(DateOnly value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static DateOnly FromDateTime(DateTime dateTime)
	{
		throw null;
	}

	public static DateOnly FromDayNumber(int dayNumber)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(DateOnly left, DateOnly right)
	{
		throw null;
	}

	public static bool operator >(DateOnly left, DateOnly right)
	{
		throw null;
	}

	public static bool operator >=(DateOnly left, DateOnly right)
	{
		throw null;
	}

	public static bool operator !=(DateOnly left, DateOnly right)
	{
		throw null;
	}

	public static bool operator <(DateOnly left, DateOnly right)
	{
		throw null;
	}

	public static bool operator <=(DateOnly left, DateOnly right)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out int year, out int month, out int day)
	{
		throw null;
	}

	public static DateOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static DateOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateOnly Parse(string s)
	{
		throw null;
	}

	public static DateOnly Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static DateOnly Parse(string s, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax("DateOnlyFormat")] ReadOnlySpan<char> format, IFormatProvider? provider = null, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax("DateOnlyFormat")] string[] formats)
	{
		throw null;
	}

	public static DateOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax("DateOnlyFormat")] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateOnly ParseExact(string s, [StringSyntax("DateOnlyFormat")] string format)
	{
		throw null;
	}

	public static DateOnly ParseExact(string s, [StringSyntax("DateOnlyFormat")] string format, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateOnly ParseExact(string s, [StringSyntax("DateOnlyFormat")] string[] formats)
	{
		throw null;
	}

	public static DateOnly ParseExact(string s, [StringSyntax("DateOnlyFormat")] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public DateTime ToDateTime(TimeOnly time)
	{
		throw null;
	}

	public DateTime ToDateTime(TimeOnly time, DateTimeKind kind)
	{
		throw null;
	}

	public string ToLongDateString()
	{
		throw null;
	}

	public string ToShortDateString()
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

	public string ToString([StringSyntax("DateOnlyFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("DateOnlyFormat")] string? format, IFormatProvider? provider)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("DateOnlyFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("DateOnlyFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles style, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, DateTimeStyles style, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax("DateOnlyFormat")] ReadOnlySpan<char> format, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax("DateOnlyFormat")] ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)][StringSyntax("DateOnlyFormat")] string?[]? formats, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)][StringSyntax("DateOnlyFormat")] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("DateOnlyFormat")] string? format, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("DateOnlyFormat")] string? format, IFormatProvider? provider, DateTimeStyles style, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("DateOnlyFormat")] string?[]? formats, out DateOnly result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("DateOnlyFormat")] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateOnly result)
	{
		throw null;
	}
}
