using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace System;

public readonly struct DateTimeOffset : IComparable, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>, IFormattable, IParsable<DateTimeOffset>, ISpanFormattable, ISpanParsable<DateTimeOffset>, IDeserializationCallback, ISerializable, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public static readonly DateTimeOffset MaxValue;

	public static readonly DateTimeOffset MinValue;

	public static readonly DateTimeOffset UnixEpoch;

	public DateTime Date
	{
		get
		{
			throw null;
		}
	}

	public DateTime DateTime
	{
		get
		{
			throw null;
		}
	}

	public int Day
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

	public int Hour
	{
		get
		{
			throw null;
		}
	}

	public DateTime LocalDateTime
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

	public int Month
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

	public static DateTimeOffset Now
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan Offset
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

	public TimeSpan TimeOfDay
	{
		get
		{
			throw null;
		}
	}

	public int TotalOffsetMinutes
	{
		get
		{
			throw null;
		}
	}

	public DateTime UtcDateTime
	{
		get
		{
			throw null;
		}
	}

	public static DateTimeOffset UtcNow
	{
		get
		{
			throw null;
		}
	}

	public long UtcTicks
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

	public DateTimeOffset(DateTime dateTime)
	{
		throw null;
	}

	public DateTimeOffset(DateTime dateTime, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond, Calendar calendar, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(DateOnly date, TimeOnly time, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset(long ticks, TimeSpan offset)
	{
		throw null;
	}

	public DateTimeOffset Add(TimeSpan timeSpan)
	{
		throw null;
	}

	public DateTimeOffset AddDays(double days)
	{
		throw null;
	}

	public DateTimeOffset AddHours(double hours)
	{
		throw null;
	}

	public DateTimeOffset AddMicroseconds(double microseconds)
	{
		throw null;
	}

	public DateTimeOffset AddMilliseconds(double milliseconds)
	{
		throw null;
	}

	public DateTimeOffset AddMinutes(double minutes)
	{
		throw null;
	}

	public DateTimeOffset AddMonths(int months)
	{
		throw null;
	}

	public DateTimeOffset AddSeconds(double seconds)
	{
		throw null;
	}

	public DateTimeOffset AddTicks(long ticks)
	{
		throw null;
	}

	public DateTimeOffset AddYears(int years)
	{
		throw null;
	}

	public static int Compare(DateTimeOffset first, DateTimeOffset second)
	{
		throw null;
	}

	public int CompareTo(DateTimeOffset other)
	{
		throw null;
	}

	public bool Equals(DateTimeOffset other)
	{
		throw null;
	}

	public static bool Equals(DateTimeOffset first, DateTimeOffset second)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool EqualsExact(DateTimeOffset other)
	{
		throw null;
	}

	public static DateTimeOffset FromFileTime(long fileTime)
	{
		throw null;
	}

	public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds)
	{
		throw null;
	}

	public static DateTimeOffset FromUnixTimeSeconds(long seconds)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static DateTimeOffset operator +(DateTimeOffset dateTimeOffset, TimeSpan timeSpan)
	{
		throw null;
	}

	public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	public static bool operator >(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	public static implicit operator DateTimeOffset(DateTime dateTime)
	{
		throw null;
	}

	public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	public static bool operator <(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out DateOnly date, out TimeOnly time, out TimeSpan offset)
	{
		throw null;
	}

	public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
	{
		throw null;
	}

	public static DateTimeOffset operator -(DateTimeOffset dateTimeOffset, TimeSpan timeSpan)
	{
		throw null;
	}

	public static DateTimeOffset Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static DateTimeOffset Parse(ReadOnlySpan<char> input, IFormatProvider? formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateTimeOffset Parse(string input)
	{
		throw null;
	}

	public static DateTimeOffset Parse(string input, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static DateTimeOffset Parse(string input, IFormatProvider? formatProvider, DateTimeStyles styles)
	{
		throw null;
	}

	public static DateTimeOffset ParseExact(ReadOnlySpan<char> input, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format, IFormatProvider? formatProvider, DateTimeStyles styles = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateTimeOffset ParseExact(ReadOnlySpan<char> input, [StringSyntax("DateTimeFormat")] string[] formats, IFormatProvider? formatProvider, DateTimeStyles styles = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateTimeOffset ParseExact(string input, [StringSyntax("DateTimeFormat")] string format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static DateTimeOffset ParseExact(string input, [StringSyntax("DateTimeFormat")] string format, IFormatProvider? formatProvider, DateTimeStyles styles)
	{
		throw null;
	}

	public static DateTimeOffset ParseExact(string input, [StringSyntax("DateTimeFormat")] string[] formats, IFormatProvider? formatProvider, DateTimeStyles styles)
	{
		throw null;
	}

	public TimeSpan Subtract(DateTimeOffset value)
	{
		throw null;
	}

	public DateTimeOffset Subtract(TimeSpan value)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public long ToFileTime()
	{
		throw null;
	}

	public DateTimeOffset ToLocalTime()
	{
		throw null;
	}

	public DateTimeOffset ToOffset(TimeSpan offset)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString(IFormatProvider? formatProvider)
	{
		throw null;
	}

	public string ToString([StringSyntax("DateTimeFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("DateTimeFormat")] string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public DateTimeOffset ToUniversalTime()
	{
		throw null;
	}

	public long ToUnixTimeMilliseconds()
	{
		throw null;
	}

	public long ToUnixTimeSeconds()
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? formatProvider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? formatProvider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> input, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? formatProvider, DateTimeStyles styles, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? formatProvider, DateTimeStyles styles, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format, IFormatProvider? formatProvider, DateTimeStyles styles, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> input, [NotNullWhen(true)][StringSyntax("DateTimeFormat")] string?[]? formats, IFormatProvider? formatProvider, DateTimeStyles styles, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("DateTimeFormat")] string? format, IFormatProvider? formatProvider, DateTimeStyles styles, out DateTimeOffset result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true)][StringSyntax("DateTimeFormat")] string?[]? formats, IFormatProvider? formatProvider, DateTimeStyles styles, out DateTimeOffset result)
	{
		throw null;
	}
}
