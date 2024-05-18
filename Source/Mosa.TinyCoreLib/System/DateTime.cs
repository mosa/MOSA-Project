using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace System;

public readonly struct DateTime : IComparable, IComparable<DateTime>, IConvertible, IEquatable<DateTime>, IFormattable, IParsable<DateTime>, ISpanFormattable, ISpanParsable<DateTime>, ISerializable, IUtf8SpanFormattable
{
	private readonly int _dummyPrimitive;

	public static readonly DateTime MaxValue;

	public static readonly DateTime MinValue;

	public static readonly DateTime UnixEpoch;

	public DateTime Date
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

	public DateTimeKind Kind
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

	public static DateTime Now
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

	public static DateTime Today
	{
		get
		{
			throw null;
		}
	}

	public static DateTime UtcNow
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

	public DateTime(int year, int month, int day)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, Calendar calendar)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond, Calendar calendar)
	{
		throw null;
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int microsecond, Calendar calendar, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime(long ticks)
	{
		throw null;
	}

	public DateTime(long ticks, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime(DateOnly date, TimeOnly time)
	{
		throw null;
	}

	public DateTime(DateOnly date, TimeOnly time, DateTimeKind kind)
	{
		throw null;
	}

	public DateTime Add(TimeSpan value)
	{
		throw null;
	}

	public DateTime AddDays(double value)
	{
		throw null;
	}

	public DateTime AddHours(double value)
	{
		throw null;
	}

	public DateTime AddMicroseconds(double value)
	{
		throw null;
	}

	public DateTime AddMilliseconds(double value)
	{
		throw null;
	}

	public DateTime AddMinutes(double value)
	{
		throw null;
	}

	public DateTime AddMonths(int months)
	{
		throw null;
	}

	public DateTime AddSeconds(double value)
	{
		throw null;
	}

	public DateTime AddTicks(long value)
	{
		throw null;
	}

	public DateTime AddYears(int value)
	{
		throw null;
	}

	public static int Compare(DateTime t1, DateTime t2)
	{
		throw null;
	}

	public int CompareTo(DateTime value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static int DaysInMonth(int year, int month)
	{
		throw null;
	}

	public bool Equals(DateTime value)
	{
		throw null;
	}

	public static bool Equals(DateTime t1, DateTime t2)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static DateTime FromBinary(long dateData)
	{
		throw null;
	}

	public static DateTime FromFileTime(long fileTime)
	{
		throw null;
	}

	public static DateTime FromFileTimeUtc(long fileTime)
	{
		throw null;
	}

	public static DateTime FromOADate(double d)
	{
		throw null;
	}

	public string[] GetDateTimeFormats()
	{
		throw null;
	}

	public string[] GetDateTimeFormats(char format)
	{
		throw null;
	}

	public string[] GetDateTimeFormats(char format, IFormatProvider? provider)
	{
		throw null;
	}

	public string[] GetDateTimeFormats(IFormatProvider? provider)
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

	public bool IsDaylightSavingTime()
	{
		throw null;
	}

	public static bool IsLeapYear(int year)
	{
		throw null;
	}

	public static DateTime operator +(DateTime d, TimeSpan t)
	{
		throw null;
	}

	public static bool operator ==(DateTime d1, DateTime d2)
	{
		throw null;
	}

	public static bool operator >(DateTime t1, DateTime t2)
	{
		throw null;
	}

	public static bool operator >=(DateTime t1, DateTime t2)
	{
		throw null;
	}

	public static bool operator !=(DateTime d1, DateTime d2)
	{
		throw null;
	}

	public static bool operator <(DateTime t1, DateTime t2)
	{
		throw null;
	}

	public static bool operator <=(DateTime t1, DateTime t2)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out DateOnly date, out TimeOnly time)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out int year, out int month, out int day)
	{
		throw null;
	}

	public static TimeSpan operator -(DateTime d1, DateTime d2)
	{
		throw null;
	}

	public static DateTime operator -(DateTime d, TimeSpan t)
	{
		throw null;
	}

	public static DateTime Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw null;
	}

	public static DateTime Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateTime Parse(string s)
	{
		throw null;
	}

	public static DateTime Parse(string s, IFormatProvider? provider)
	{
		throw null;
	}

	public static DateTime Parse(string s, IFormatProvider? provider, DateTimeStyles styles)
	{
		throw null;
	}

	public static DateTime ParseExact(ReadOnlySpan<char> s, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateTime ParseExact(ReadOnlySpan<char> s, [StringSyntax("DateTimeFormat")] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
	{
		throw null;
	}

	public static DateTime ParseExact(string s, [StringSyntax("DateTimeFormat")] string format, IFormatProvider? provider)
	{
		throw null;
	}

	public static DateTime ParseExact(string s, [StringSyntax("DateTimeFormat")] string format, IFormatProvider? provider, DateTimeStyles style)
	{
		throw null;
	}

	public static DateTime ParseExact(string s, [StringSyntax("DateTimeFormat")] string[] formats, IFormatProvider? provider, DateTimeStyles style)
	{
		throw null;
	}

	public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
	{
		throw null;
	}

	public TimeSpan Subtract(DateTime value)
	{
		throw null;
	}

	public DateTime Subtract(TimeSpan value)
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

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public long ToBinary()
	{
		throw null;
	}

	public long ToFileTime()
	{
		throw null;
	}

	public long ToFileTimeUtc()
	{
		throw null;
	}

	public DateTime ToLocalTime()
	{
		throw null;
	}

	public string ToLongDateString()
	{
		throw null;
	}

	public string ToLongTimeString()
	{
		throw null;
	}

	public double ToOADate()
	{
		throw null;
	}

	public string ToShortDateString()
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

	public string ToString([StringSyntax("DateTimeFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("DateTimeFormat")] string? format, IFormatProvider? provider)
	{
		throw null;
	}

	public DateTime ToUniversalTime()
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider? provider = null)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, out DateTime result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DateTime result)
	{
		throw null;
	}

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles styles, out DateTime result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, out DateTime result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DateTime result)
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, DateTimeStyles styles, out DateTime result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax("DateTimeFormat")] ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out DateTime result)
	{
		throw null;
	}

	public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)][StringSyntax("DateTimeFormat")] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateTime result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("DateTimeFormat")] string? format, IFormatProvider? provider, DateTimeStyles style, out DateTime result)
	{
		throw null;
	}

	public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)][StringSyntax("DateTimeFormat")] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateTime result)
	{
		throw null;
	}
}
