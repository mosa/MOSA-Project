using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

public readonly struct TimeSpan : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IFormattable, IParsable<TimeSpan>, ISpanFormattable, ISpanParsable<TimeSpan>, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public static readonly TimeSpan MaxValue;

	public static readonly TimeSpan MinValue;

	public const long NanosecondsPerTick = 100L;

	public const long TicksPerDay = 864000000000L;

	public const long TicksPerHour = 36000000000L;

	public const long TicksPerMicrosecond = 10L;

	public const long TicksPerMillisecond = 10000L;

	public const long TicksPerMinute = 600000000L;

	public const long TicksPerSecond = 10000000L;

	public static readonly TimeSpan Zero;

	public int Days
	{
		get
		{
			throw null;
		}
	}

	public int Hours
	{
		get
		{
			throw null;
		}
	}

	public int Microseconds
	{
		get
		{
			throw null;
		}
	}

	public int Milliseconds
	{
		get
		{
			throw null;
		}
	}

	public int Minutes
	{
		get
		{
			throw null;
		}
	}

	public int Nanoseconds
	{
		get
		{
			throw null;
		}
	}

	public int Seconds
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

	public double TotalDays
	{
		get
		{
			throw null;
		}
	}

	public double TotalHours
	{
		get
		{
			throw null;
		}
	}

	public double TotalMicroseconds
	{
		get
		{
			throw null;
		}
	}

	public double TotalMilliseconds
	{
		get
		{
			throw null;
		}
	}

	public double TotalMinutes
	{
		get
		{
			throw null;
		}
	}

	public double TotalNanoseconds
	{
		get
		{
			throw null;
		}
	}

	public double TotalSeconds
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan(int hours, int minutes, int seconds)
	{
		throw null;
	}

	public TimeSpan(int days, int hours, int minutes, int seconds)
	{
		throw null;
	}

	public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
	{
		throw null;
	}

	public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds, int microseconds)
	{
		throw null;
	}

	public TimeSpan(long ticks)
	{
		throw null;
	}

	public TimeSpan Add(TimeSpan ts)
	{
		throw null;
	}

	public static int Compare(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public int CompareTo(TimeSpan value)
	{
		throw null;
	}

	public TimeSpan Divide(double divisor)
	{
		throw null;
	}

	public double Divide(TimeSpan ts)
	{
		throw null;
	}

	public TimeSpan Duration()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public bool Equals(TimeSpan obj)
	{
		throw null;
	}

	public static bool Equals(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static TimeSpan FromDays(double value)
	{
		throw null;
	}

	public static TimeSpan FromHours(double value)
	{
		throw null;
	}

	public static TimeSpan FromMicroseconds(double value)
	{
		throw null;
	}

	public static TimeSpan FromMilliseconds(double value)
	{
		throw null;
	}

	public static TimeSpan FromMinutes(double value)
	{
		throw null;
	}

	public static TimeSpan FromSeconds(double value)
	{
		throw null;
	}

	public static TimeSpan FromTicks(long value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public TimeSpan Multiply(double factor)
	{
		throw null;
	}

	public TimeSpan Negate()
	{
		throw null;
	}

	public static TimeSpan operator +(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static TimeSpan operator /(TimeSpan timeSpan, double divisor)
	{
		throw null;
	}

	public static double operator /(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static bool operator ==(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static bool operator >(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static bool operator >=(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static bool operator !=(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static bool operator <(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static bool operator <=(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static TimeSpan operator *(double factor, TimeSpan timeSpan)
	{
		throw null;
	}

	public static TimeSpan operator *(TimeSpan timeSpan, double factor)
	{
		throw null;
	}

	public static TimeSpan operator -(TimeSpan t1, TimeSpan t2)
	{
		throw null;
	}

	public static TimeSpan operator -(TimeSpan t)
	{
		throw null;
	}

	public static TimeSpan operator +(TimeSpan t)
	{
		throw null;
	}

	public static TimeSpan Parse(ReadOnlySpan<char> input, IFormatProvider? formatProvider = null)
	{
		throw null;
	}

	public static TimeSpan Parse(string s)
	{
		throw null;
	}

	public static TimeSpan Parse(string input, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static TimeSpan ParseExact(ReadOnlySpan<char> input, [StringSyntax("TimeSpanFormat")] ReadOnlySpan<char> format, IFormatProvider? formatProvider, TimeSpanStyles styles = TimeSpanStyles.None)
	{
		throw null;
	}

	public static TimeSpan ParseExact(ReadOnlySpan<char> input, [StringSyntax("TimeSpanFormat")] string[] formats, IFormatProvider? formatProvider, TimeSpanStyles styles = TimeSpanStyles.None)
	{
		throw null;
	}

	public static TimeSpan ParseExact(string input, [StringSyntax("TimeSpanFormat")] string format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static TimeSpan ParseExact(string input, [StringSyntax("TimeSpanFormat")] string format, IFormatProvider? formatProvider, TimeSpanStyles styles)
	{
		throw null;
	}

	public static TimeSpan ParseExact(string input, [StringSyntax("TimeSpanFormat")] string[] formats, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static TimeSpan ParseExact(string input, [StringSyntax("TimeSpanFormat")] string[] formats, IFormatProvider? formatProvider, TimeSpanStyles styles)
	{
		throw null;
	}

	public TimeSpan Subtract(TimeSpan ts)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString([StringSyntax("TimeSpanFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("TimeSpanFormat")] string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("TimeSpanFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? formatProvider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("TimeSpanFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? formatProvider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? formatProvider, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? formatProvider, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [StringSyntax("TimeSpanFormat")] ReadOnlySpan<char> format, IFormatProvider? formatProvider, TimeSpanStyles styles, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [StringSyntax("TimeSpanFormat")] ReadOnlySpan<char> format, IFormatProvider? formatProvider, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [NotNullWhen(true)][StringSyntax("TimeSpanFormat")] string?[]? formats, IFormatProvider? formatProvider, TimeSpanStyles styles, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [NotNullWhen(true)][StringSyntax("TimeSpanFormat")] string?[]? formats, IFormatProvider? formatProvider, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("TimeSpanFormat")] string? format, IFormatProvider? formatProvider, TimeSpanStyles styles, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("TimeSpanFormat")] string? format, IFormatProvider? formatProvider, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("TimeSpanFormat")] string?[]? formats, IFormatProvider? formatProvider, TimeSpanStyles styles, out TimeSpan result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("TimeSpanFormat")] string?[]? formats, IFormatProvider? formatProvider, out TimeSpan result)
	{
		throw null;
	}
}
