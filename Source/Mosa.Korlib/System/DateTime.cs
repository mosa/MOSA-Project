﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Auto)]
[Serializable]
public readonly partial struct DateTime : IComparable, IComparable<DateTime>, IEquatable<DateTime>
{
	#region partial class

	internal const bool s_systemSupportsLeapSeconds = false;

	internal static bool IsValidTimeWithLeapSeconds(int year, int month, int day, int hour, int minute, DateTimeKind kind) => false;

	#endregion partial class

	// Number of 100ns ticks per time unit
	private const long TicksPerMillisecond = 10000;

	private const long TicksPerSecond = TicksPerMillisecond * 1000;
	private const long TicksPerMinute = TicksPerSecond * 60;
	private const long TicksPerHour = TicksPerMinute * 60;
	private const long TicksPerDay = TicksPerHour * 24;

	// Number of milliseconds per time unit
	private const int MillisPerSecond = 1000;

	private const int MillisPerMinute = MillisPerSecond * 60;
	private const int MillisPerHour = MillisPerMinute * 60;
	private const int MillisPerDay = MillisPerHour * 24;

	// Number of days in a non-leap year
	private const int DaysPerYear = 365;

	// Number of days in 4 years
	private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461

	// Number of days in 100 years
	private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524

	// Number of days in 400 years
	private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097

	// Number of days from 1/1/0001 to 12/31/1600
	private const int DaysTo1601 = DaysPer400Years * 4;          // 584388

	// Number of days from 1/1/0001 to 12/30/1899
	private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;

	// Number of days from 1/1/0001 to 12/31/1969
	internal const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162

	// Number of days from 1/1/0001 to 12/31/9999
	private const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059

	internal const long MinTicks = 0;
	internal const long MaxTicks = DaysTo10000 * TicksPerDay - 1;
	private const long MaxMillis = (long)DaysTo10000 * MillisPerDay;

	internal const long UnixEpochTicks = DaysTo1970 * TicksPerDay;
	private const long FileTimeOffset = DaysTo1601 * TicksPerDay;
	private const long DoubleDateOffset = DaysTo1899 * TicksPerDay;

	// The minimum OA date is 0100/01/01 (Note it's year 100).
	// The maximum OA date is 9999/12/31
	private const long OADateMinAsTicks = (DaysPer100Years - DaysPerYear) * TicksPerDay;

	// All OA dates must be greater than (not >=) OADateMinAsDouble
	private const double OADateMinAsDouble = -657435.0;

	// All OA dates must be less than (not <=) OADateMaxAsDouble
	private const double OADateMaxAsDouble = 2958466.0;

	private const int DatePartYear = 0;
	private const int DatePartDayOfYear = 1;
	private const int DatePartMonth = 2;
	private const int DatePartDay = 3;

	private static readonly uint[] s_daysToMonth365 = {
		0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };

	private static readonly uint[] s_daysToMonth366 = {
		0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

	private static byte[] DaysInMonth365 => new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
	private static byte[] DaysInMonth366 => new byte[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	public static readonly DateTime MinValue;
	public static readonly DateTime MaxValue = new DateTime(MaxTicks, DateTimeKind.Unspecified);
	public static readonly DateTime UnixEpoch = new DateTime(UnixEpochTicks, DateTimeKind.Utc);

	private const ulong TicksMask = 0x3FFFFFFFFFFFFFFF;
	private const ulong FlagsMask = 0xC000000000000000;
	private const long TicksCeiling = 0x4000000000000000;
	private const ulong KindUnspecified = 0x0000000000000000;
	private const ulong KindUtc = 0x4000000000000000;
	private const ulong KindLocal = 0x8000000000000000;
	private const ulong KindLocalAmbiguousDst = 0xC000000000000000;
	private const int KindShift = 62;

	private const string TicksField = "ticks"; // Do not rename (binary serialization)
	private const string DateDataField = "dateData"; // Do not rename (binary serialization)

	// The data is stored as an unsigned 64-bit integer
	//   Bits 01-62: The value of 100-nanosecond ticks where 0 represents 1/1/0001 12:00am, up until the value
	//               12/31/9999 23:59:59.9999999
	//   Bits 63-64: A four-state value that describes the DateTimeKind value of the date time, with a 2nd
	//               value for the rare case where the date time is local, but is in an overlapped daylight
	//               savings time hour and it is in daylight savings time. This allows distinction of these
	//               otherwise ambiguous local times and prevents data loss when round tripping from Local to
	//               UTC time.
	private readonly ulong _dateData;

	// Constructs a DateTime from a tick count. The ticks
	// argument specifies the date as the number of 100-nanosecond intervals
	// that have elapsed since 1/1/0001 12:00am.
	//
	public DateTime(long ticks)
	{
		if ((ulong)ticks > MaxTicks) ThrowTicksOutOfRange();
		_dateData = (ulong)ticks;
	}

	private DateTime(ulong dateData)
	{
		this._dateData = dateData;
	}

	internal static DateTime UnsafeCreate(long ticks) => new DateTime((ulong)ticks);

	public DateTime(long ticks, DateTimeKind kind)
	{
		if ((ulong)ticks > MaxTicks) ThrowTicksOutOfRange();
		if ((uint)kind > (uint)DateTimeKind.Local) ThrowInvalidKind();
		_dateData = (ulong)ticks | ((ulong)(uint)kind << KindShift);
	}

	internal DateTime(long ticks, DateTimeKind kind, bool isAmbiguousDst)
	{
		if ((ulong)ticks > MaxTicks) ThrowTicksOutOfRange();
		Debug.Assert(kind == DateTimeKind.Local, "Internal Constructor is for local times only");
		_dateData = ((ulong)ticks | (isAmbiguousDst ? KindLocalAmbiguousDst : KindLocal));
	}

	private static void ThrowTicksOutOfRange() => throw new ArgumentOutOfRangeException("ticks", SR.ArgumentOutOfRange_DateTimeBadTicks);

	private static void ThrowInvalidKind() => throw new ArgumentException(SR.Argument_InvalidDateTimeKind, "kind");

	private static void ThrowMillisecondOutOfRange() => throw new ArgumentOutOfRangeException("millisecond", SR.Format(SR.ArgumentOutOfRange_Range, 0, MillisPerSecond - 1));

	private static void ThrowDateArithmetic(int param) => throw new ArgumentOutOfRangeException(SR.ArgumentOutOfRange_DateArithmetic);

	// Constructs a DateTime from a given year, month, and day. The
	// time-of-day of the resulting DateTime is always midnight.
	//
	public DateTime(int year, int month, int day)
	{
		_dateData = DateToTicks(year, month, day);
	}

	// Constructs a DateTime from a given year, month, day, hour,
	// minute, and second.
	//
	public DateTime(int year, int month, int day, int hour, int minute, int second)
	{
		if (second != 60 || !s_systemSupportsLeapSeconds)
		{
			_dateData = DateToTicks(year, month, day) + TimeToTicks(hour, minute, second);
		}
		else
		{
			// if we have a leap second, then we adjust it to 59 so that DateTime will consider it the last in the specified minute.
			this = new(year, month, day, hour, minute, 59);
			ValidateLeapSecond();
		}
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
	{
		if ((uint)kind > (uint)DateTimeKind.Local) ThrowInvalidKind();

		if (second != 60 || !s_systemSupportsLeapSeconds)
		{
			ulong ticks = DateToTicks(year, month, day) + TimeToTicks(hour, minute, second);
			_dateData = ticks | ((ulong)kind << KindShift);
		}
		else
		{
			// if we have a leap second, then we adjust it to 59 so that DateTime will consider it the last in the specified minute.
			this = new(year, month, day, hour, minute, 59, kind);
			ValidateLeapSecond();
		}
	}

	// Constructs a DateTime from a given year, month, day, hour,
	// minute, and second.
	//
	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		if ((uint)millisecond >= MillisPerSecond) ThrowMillisecondOutOfRange();

		if (second != 60 || !s_systemSupportsLeapSeconds)
		{
			ulong ticks = DateToTicks(year, month, day) + TimeToTicks(hour, minute, second);
			ticks += (uint)millisecond * (uint)TicksPerMillisecond;
			Debug.Assert(ticks <= MaxTicks, "Input parameters validated already");
			_dateData = ticks;
		}
		else
		{
			// if we have a leap second, then we adjust it to 59 so that DateTime will consider it the last in the specified minute.
			this = new(year, month, day, hour, minute, 59, millisecond);
			ValidateLeapSecond();
		}
	}

	public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
	{
		if ((uint)millisecond >= MillisPerSecond) ThrowMillisecondOutOfRange();
		if ((uint)kind > (uint)DateTimeKind.Local) ThrowInvalidKind();

		if (second != 60 || !s_systemSupportsLeapSeconds)
		{
			ulong ticks = DateToTicks(year, month, day) + TimeToTicks(hour, minute, second);
			ticks += (uint)millisecond * (uint)TicksPerMillisecond;
			Debug.Assert(ticks <= MaxTicks, "Input parameters validated already");
			_dateData = ticks | ((ulong)kind << KindShift);
		}
		else
		{
			// if we have a leap second, then we adjust it to 59 so that DateTime will consider it the last in the specified minute.
			this = new(year, month, day, hour, minute, 59, millisecond, kind);
			ValidateLeapSecond();
		}
	}

	private void ValidateLeapSecond()
	{
		if (!IsValidTimeWithLeapSeconds(Year, Month, Day, Hour, Minute, Kind))
		{
			ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
		}
	}

	private ulong UTicks => _dateData & TicksMask;

	private ulong InternalKind => _dateData & FlagsMask;

	// Returns the DateTime resulting from adding the given
	// TimeSpan to this DateTime.
	//
	public DateTime Add(TimeSpan value)
	{
		return AddTicks(value._ticks);
	}

	// Returns the DateTime resulting from adding a fractional number of
	// time units to this DateTime.
	private DateTime Add(double value, int scale)
	{
		double millis_double = value * scale + (value >= 0 ? 0.5 : -0.5);
		if (millis_double is <= -MaxMillis or >= MaxMillis) ThrowOutOfRange();
		return AddTicks((long)millis_double * TicksPerMillisecond);

		static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException(nameof(value), SR.ArgumentOutOfRange_AddValue);
	}

	// Returns the DateTime resulting from adding a fractional number of
	// days to this DateTime. The result is computed by rounding the
	// fractional number of days given by value to the nearest
	// millisecond, and adding that interval to this DateTime. The
	// value argument is permitted to be negative.
	//
	public DateTime AddDays(double value)
	{
		return Add(value, MillisPerDay);
	}

	// Returns the DateTime resulting from adding a fractional number of
	// hours to this DateTime. The result is computed by rounding the
	// fractional number of hours given by value to the nearest
	// millisecond, and adding that interval to this DateTime. The
	// value argument is permitted to be negative.
	//
	public DateTime AddHours(double value)
	{
		return Add(value, MillisPerHour);
	}

	// Returns the DateTime resulting from the given number of
	// milliseconds to this DateTime. The result is computed by rounding
	// the number of milliseconds given by value to the nearest integer,
	// and adding that interval to this DateTime. The value
	// argument is permitted to be negative.
	//
	public DateTime AddMilliseconds(double value)
	{
		return Add(value, 1);
	}

	// Returns the DateTime resulting from adding a fractional number of
	// minutes to this DateTime. The result is computed by rounding the
	// fractional number of minutes given by value to the nearest
	// millisecond, and adding that interval to this DateTime. The
	// value argument is permitted to be negative.
	//
	public DateTime AddMinutes(double value)
	{
		return Add(value, MillisPerMinute);
	}

	// Returns the DateTime resulting from adding the given number of
	// months to this DateTime. The result is computed by incrementing
	// (or decrementing) the year and month parts of this DateTime by
	// months months, and, if required, adjusting the day part of the
	// resulting date downwards to the last day of the resulting month in the
	// resulting year. The time-of-day part of the result is the same as the
	// time-of-day part of this DateTime.
	//
	// In more precise terms, considering this DateTime to be of the
	// form y / m / d + t, where y is the
	// year, m is the month, d is the day, and t is the
	// time-of-day, the result is y1 / m1 / d1 + t,
	// where y1 and m1 are computed by adding months months
	// to y and m, and d1 is the largest value less than
	// or equal to d that denotes a valid day in month m1 of year
	// y1.
	//
	public DateTime AddMonths(int months)
	{
		if (months is < -120000 or > 120000) throw new ArgumentOutOfRangeException(nameof(months), SR.ArgumentOutOfRange_DateTimeBadMonths);
		GetDate(out int year, out int month, out int day);
		int y = year, d = day;
		int m = month + months;
		int q = m > 0 ? (int)((uint)(m - 1) / 12) : m / 12 - 1;
		y += q;
		m -= q * 12;
		if (y is < 1 or > 9999) ThrowDateArithmetic(2);
		uint[] daysTo = IsLeapYear(y) ? s_daysToMonth366 : s_daysToMonth365;
		uint daysToMonth = daysTo[m - 1];
		int days = (int)(daysTo[m] - daysToMonth);
		if (d > days) d = days;
		uint n = DaysToYear((uint)y) + daysToMonth + (uint)d - 1;
		return new DateTime(n * (ulong)TicksPerDay + UTicks % TicksPerDay | InternalKind);
	}

	// Returns the DateTime resulting from adding a fractional number of
	// seconds to this DateTime. The result is computed by rounding the
	// fractional number of seconds given by value to the nearest
	// millisecond, and adding that interval to this DateTime. The
	// value argument is permitted to be negative.
	//
	public DateTime AddSeconds(double value)
	{
		return Add(value, MillisPerSecond);
	}

	// Returns the DateTime resulting from adding the given number of
	// 100-nanosecond ticks to this DateTime. The value argument
	// is permitted to be negative.
	//
	public DateTime AddTicks(long value)
	{
		ulong ticks = (ulong)(Ticks + value);
		if (ticks > MaxTicks) ThrowDateArithmetic(0);
		return new DateTime(ticks | InternalKind);
	}

	// TryAddTicks is exact as AddTicks except it doesn't throw
	internal bool TryAddTicks(long value, out DateTime result)
	{
		ulong ticks = (ulong)(Ticks + value);
		if (ticks > MaxTicks)
		{
			result = default;
			return false;
		}
		result = new DateTime(ticks | InternalKind);
		return true;
	}

	// Returns the DateTime resulting from adding the given number of
	// years to this DateTime. The result is computed by incrementing
	// (or decrementing) the year part of this DateTime by value
	// years. If the month and day of this DateTime is 2/29, and if the
	// resulting year is not a leap year, the month and day of the resulting
	// DateTime becomes 2/28. Otherwise, the month, day, and time-of-day
	// parts of the result are the same as those of this DateTime.
	//
	public DateTime AddYears(int value)
	{
		if (value is < -10000 or > 10000)
		{
			throw new ArgumentOutOfRangeException(nameof(value), SR.ArgumentOutOfRange_DateTimeBadYears);
		}
		GetDate(out int year, out int month, out int day);
		int y = year + value;
		if (y is < 1 or > 9999) ThrowDateArithmetic(0);
		uint n = DaysToYear((uint)y);

		int m = month - 1, d = day - 1;
		if (IsLeapYear(y))
		{
			n += s_daysToMonth366[m];
		}
		else
		{
			if (d == 28 && m == 1) d--;
			n += s_daysToMonth365[m];
		}
		n += (uint)d;
		return new DateTime(n * (ulong)TicksPerDay + UTicks % TicksPerDay | InternalKind);
	}

	// Compares two DateTime values, returning an integer that indicates
	// their relationship.
	//
	public static int Compare(DateTime t1, DateTime t2)
	{
		long ticks1 = t1.Ticks;
		long ticks2 = t2.Ticks;
		if (ticks1 > ticks2) return 1;
		if (ticks1 < ticks2) return -1;
		return 0;
	}

	// Compares this DateTime to a given object. This method provides an
	// implementation of the IComparable interface. The object
	// argument must be another DateTime, or otherwise an exception
	// occurs.  Null is considered less than any instance.
	//
	// Returns a value less than zero if this  object
	public int CompareTo(object value)
	{
		if (value == null) return 1;
		if (!(value is DateTime))
		{
			throw new ArgumentException(SR.Arg_MustBeDateTime);
		}

		return Compare(this, (DateTime)value);
	}

	public int CompareTo(DateTime value)
	{
		return Compare(this, value);
	}

	// Returns the tick count corresponding to the given year, month, and day.
	// Will check the if the parameters are valid.
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong DateToTicks(int year, int month, int day)
	{
		if (year is < 1 or > 9999 || month is < 1 or > 12 || day < 1)
		{
			ThrowHelper.ThrowArgumentOutOfRange_BadYearMonthDay();
		}

		uint[] days = IsLeapYear(year) ? s_daysToMonth366 : s_daysToMonth365;
		if ((uint)day > days[month] - days[month - 1])
		{
			ThrowHelper.ThrowArgumentOutOfRange_BadYearMonthDay();
		}

		uint n = DaysToYear((uint)year) + days[month - 1] + (uint)day - 1;
		return n * (ulong)TicksPerDay;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint DaysToYear(uint year)
	{
		uint y = year - 1;
		uint cent = y / 100;
		return y * (365 * 4 + 1) / 4 - cent + cent / 4;
	}

	// Return the tick count corresponding to the given hour, minute, second.
	// Will check the if the parameters are valid.
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong TimeToTicks(int hour, int minute, int second)
	{
		if ((uint)hour >= 24 || (uint)minute >= 60 || (uint)second >= 60)
		{
			ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
		}

		int totalSeconds = hour * 3600 + minute * 60 + second;
		return (uint)totalSeconds * (ulong)TicksPerSecond;
	}

	internal static ulong TimeToTicks(int hour, int minute, int second, int millisecond)
	{
		ulong ticks = TimeToTicks(hour, minute, second);

		if ((uint)millisecond >= MillisPerSecond) ThrowMillisecondOutOfRange();

		ticks += (uint)millisecond * (uint)TicksPerMillisecond;

		Debug.Assert(ticks <= MaxTicks, "Input parameters validated already");

		return ticks;
	}

	// Returns the number of days in the month given by the year and
	// month arguments.
	//
	public static int DaysInMonth(int year, int month)
	{
		if (month is < 1 or > 12) ThrowHelper.ThrowArgumentOutOfRange_Month(month);

		// IsLeapYear checks the year argument
		return (IsLeapYear(year) ? DaysInMonth366 : DaysInMonth365)[month - 1];
	}

	// Converts an OLE Date to a tick count.
	// This function is duplicated in COMDateTime.cpp
	internal static long DoubleDateToTicks(double value)
	{
		// The check done this way will take care of NaN
		if (!(value < OADateMaxAsDouble) || !(value > OADateMinAsDouble))
			throw new ArgumentException(SR.Arg_OleAutDateInvalid);

		// Conversion to long will not cause an overflow here, as at this point the "value" is in between OADateMinAsDouble and OADateMaxAsDouble
		long millis = (long)(value * MillisPerDay + (value >= 0 ? 0.5 : -0.5));

		// The interesting thing here is when you have a value like 12.5 it all positive 12 days and 12 hours from 01/01/1899
		// However if you a value of -12.25 it is minus 12 days but still positive 6 hours, almost as though you meant -11.75 all negative
		// This line below fixes up the millis in the negative case
		if (millis < 0)
		{
			millis -= (millis % MillisPerDay) * 2;
		}

		millis += DoubleDateOffset / TicksPerMillisecond;

		if (millis is < 0 or >= MaxMillis) throw new ArgumentException(SR.Arg_OleAutDateScale);
		return millis * TicksPerMillisecond;
	}

	// Checks if this DateTime is equal to a given object. Returns
	// true if the given object is a boxed DateTime and its value
	// is equal to the value of this DateTime. Returns false
	// otherwise.
	//
	public override bool Equals(object value)
	{
		if (value is DateTime)
		{
			return Ticks == ((DateTime)value).Ticks;
		}
		return false;
	}

	public bool Equals(DateTime value)
	{
		return this == value;
	}

	// Compares two DateTime values for equality. Returns true if
	// the two DateTime values are equal, or false if they are
	// not equal.
	//
	public static bool Equals(DateTime t1, DateTime t2)
	{
		return t1 == t2;
	}

	// Creates a DateTime from an OLE Automation Date.
	//
	public static DateTime FromOADate(double d)
	{
		return new DateTime(DoubleDateToTicks(d), DateTimeKind.Unspecified);
	}

	public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
	{
		if ((uint)kind > (uint)DateTimeKind.Local) ThrowInvalidKind();
		return new DateTime(value.UTicks | ((ulong)kind << KindShift));
	}

	// Returns the date part of this DateTime. The resulting value
	// corresponds to this DateTime with the time-of-day part set to
	// zero (midnight).
	//
	public DateTime Date
	{
		get
		{
			ulong ticks = UTicks;
			return new DateTime((ticks - ticks % TicksPerDay) | InternalKind);
		}
	}

	// Returns a given date part of this DateTime. This method is used
	// to compute the year, day-of-year, month, or day part.
	private int GetDatePart(int part)
	{
		// n = number of days since 1/1/0001
		uint n = (uint)(UTicks / TicksPerDay);

		// y400 = number of whole 400-year periods since 1/1/0001
		uint y400 = n / DaysPer400Years;

		// n = day number within 400-year period
		n -= y400 * DaysPer400Years;

		// y100 = number of whole 100-year periods within 400-year period
		uint y100 = n / DaysPer100Years;

		// Last 100-year period has an extra day, so decrement result if 4
		if (y100 == 4) y100 = 3;

		// n = day number within 100-year period
		n -= y100 * DaysPer100Years;

		// y4 = number of whole 4-year periods within 100-year period
		uint y4 = n / DaysPer4Years;

		// n = day number within 4-year period
		n -= y4 * DaysPer4Years;

		// y1 = number of whole years within 4-year period
		uint y1 = n / DaysPerYear;

		// Last year has an extra day, so decrement result if 4
		if (y1 == 4) y1 = 3;

		// If year was requested, compute and return it
		if (part == DatePartYear)
		{
			return (int)(y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1);
		}

		// n = day number within year
		n -= y1 * DaysPerYear;

		// If day-of-year was requested, return it
		if (part == DatePartDayOfYear) return (int)n + 1;

		// Leap year calculation looks different from IsLeapYear since y1, y4,
		// and y100 are relative to year 1, not year 0
		uint[] days = y1 == 3 && (y4 != 24 || y100 == 3) ? s_daysToMonth366 : s_daysToMonth365;

		// All months have less than 32 days, so n >> 5 is a good conservative
		// estimate for the month
		uint m = (n >> 5) + 1;

		// m = 1-based month number
		while (n >= days[m]) m++;

		// If month was requested, return it
		if (part == DatePartMonth) return (int)m;

		// Return 1-based day-of-month
		return (int)(n - days[m - 1] + 1);
	}

	// Exactly the same as GetDatePart, except computing all of
	// year/month/day rather than just one of them. Used when all three
	// are needed rather than redoing the computations for each.
	internal void GetDate(out int year, out int month, out int day)
	{
		// n = number of days since 1/1/0001
		uint n = (uint)(UTicks / TicksPerDay);

		// y400 = number of whole 400-year periods since 1/1/0001
		uint y400 = n / DaysPer400Years;

		// n = day number within 400-year period
		n -= y400 * DaysPer400Years;

		// y100 = number of whole 100-year periods within 400-year period
		uint y100 = n / DaysPer100Years;

		// Last 100-year period has an extra day, so decrement result if 4
		if (y100 == 4) y100 = 3;

		// n = day number within 100-year period
		n -= y100 * DaysPer100Years;

		// y4 = number of whole 4-year periods within 100-year period
		uint y4 = n / DaysPer4Years;

		// n = day number within 4-year period
		n -= y4 * DaysPer4Years;

		// y1 = number of whole years within 4-year period
		uint y1 = n / DaysPerYear;

		// Last year has an extra day, so decrement result if 4
		if (y1 == 4) y1 = 3;

		// compute year
		year = (int)(y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1);

		// n = day number within year
		n -= y1 * DaysPerYear;

		// dayOfYear = n + 1;
		// Leap year calculation looks different from IsLeapYear since y1, y4,
		// and y100 are relative to year 1, not year 0
		uint[] days = y1 == 3 && (y4 != 24 || y100 == 3) ? s_daysToMonth366 : s_daysToMonth365;

		// All months have less than 32 days, so n >> 5 is a good conservative
		// estimate for the month
		uint m = (n >> 5) + 1;

		// m = 1-based month number
		while (n >= days[m]) m++;

		// compute month and day
		month = (int)m;
		day = (int)(n - days[m - 1] + 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void GetTime(out int hour, out int minute, out int second)
	{
		ulong seconds = UTicks / TicksPerSecond;
		ulong minutes = seconds / 60;
		second = (int)(seconds - (minutes * 60));
		ulong hours = minutes / 60;
		minute = (int)(minutes - (hours * 60));
		hour = (int)((uint)hours % 24);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void GetTime(out int hour, out int minute, out int second, out int millisecond)
	{
		ulong milliseconds = UTicks / TicksPerMillisecond;
		ulong seconds = milliseconds / 1000;
		millisecond = (int)(milliseconds - (seconds * 1000));
		ulong minutes = seconds / 60;
		second = (int)(seconds - (minutes * 60));
		ulong hours = minutes / 60;
		minute = (int)(minutes - (hours * 60));
		hour = (int)((uint)hours % 24);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void GetTimePrecise(out int hour, out int minute, out int second, out int tick)
	{
		ulong ticks = UTicks;
		ulong seconds = ticks / TicksPerSecond;
		tick = (int)(ticks - (seconds * TicksPerSecond));
		ulong minutes = seconds / 60;
		second = (int)(seconds - (minutes * 60));
		ulong hours = minutes / 60;
		minute = (int)(minutes - (hours * 60));
		hour = (int)((uint)hours % 24);
	}

	// Returns the day-of-month part of this DateTime. The returned
	// value is an integer between 1 and 31.
	//
	public int Day => GetDatePart(DatePartDay);

	// Returns the day-of-week part of this DateTime. The returned value
	// is an integer between 0 and 6, where 0 indicates Sunday, 1 indicates
	// Monday, 2 indicates Tuesday, 3 indicates Wednesday, 4 indicates
	// Thursday, 5 indicates Friday, and 6 indicates Saturday.
	//
	public DayOfWeek DayOfWeek => (DayOfWeek)(((uint)(UTicks / TicksPerDay) + 1) % 7);

	// Returns the day-of-year part of this DateTime. The returned value
	// is an integer between 1 and 366.
	//
	public int DayOfYear => GetDatePart(DatePartDayOfYear);

	// Returns the hash code for this DateTime.
	//
	public override int GetHashCode()
	{
		long ticks = Ticks;
		return unchecked((int)ticks) ^ (int)(ticks >> 32);
	}

	// Returns the hour part of this DateTime. The returned value is an
	// integer between 0 and 23.
	//
	public int Hour => (int)((uint)(UTicks / TicksPerHour) % 24);

	internal bool IsAmbiguousDaylightSavingTime() =>
		InternalKind == KindLocalAmbiguousDst;

	public DateTimeKind Kind
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => InternalKind switch
		{
			KindUnspecified => DateTimeKind.Unspecified,
			KindUtc => DateTimeKind.Utc,
			_ => DateTimeKind.Local,
		};
	}

	// Returns the millisecond part of this DateTime. The returned value
	// is an integer between 0 and 999.
	//
	public int Millisecond => (int)((UTicks / TicksPerMillisecond) % 1000);

	// Returns the minute part of this DateTime. The returned value is
	// an integer between 0 and 59.
	//
	public int Minute => (int)((UTicks / TicksPerMinute) % 60);

	// Returns the month part of this DateTime. The returned value is an
	// integer between 1 and 12.
	//
	public int Month => GetDatePart(DatePartMonth);

	// Returns the second part of this DateTime. The returned value is
	// an integer between 0 and 59.
	//
	public int Second => (int)((UTicks / TicksPerSecond) % 60);

	// Returns the tick count for this DateTime. The returned value is
	// the number of 100-nanosecond intervals that have elapsed since 1/1/0001
	// 12:00am.
	//
	public long Ticks => (long)(_dateData & TicksMask);

	// Returns the time-of-day part of this DateTime. The returned value
	// is a TimeSpan that indicates the time elapsed since midnight.
	//
	public TimeSpan TimeOfDay => new TimeSpan((long)(UTicks % TicksPerDay));

	// Returns a DateTime representing the current date. The date part
	// of the returned value is the current date, and the time-of-day part of
	// the returned value is zero (midnight).
	//
	//public static DateTime Today => Now.Date;

	// Returns the year part of this DateTime. The returned value is an
	// integer between 1 and 9999.
	//
	public int Year => GetDatePart(DatePartYear);

	// Checks whether a given year is a leap year. This method returns true if
	// year is a leap year, or false if not.
	//
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeapYear(int year)
	{
		if (year is < 1 or > 9999)
		{
			ThrowHelper.ThrowArgumentOutOfRange_Year();
		}
		if ((year & 3) != 0) return false;
		if ((year & 15) == 0) return true;

		// return true/false not the test result https://github.com/dotnet/runtime/issues/4207
		return (uint)year % 25 != 0 ? true : false;
	}

	public TimeSpan Subtract(DateTime value)
	{
		return new TimeSpan(Ticks - value.Ticks);
	}

	public DateTime Subtract(TimeSpan value)
	{
		ulong ticks = (ulong)(Ticks - value._ticks);
		if (ticks > MaxTicks) ThrowDateArithmetic(0);
		return new DateTime(ticks | InternalKind);
	}

	// This function is duplicated in COMDateTime.cpp
	private static double TicksToOADate(long value)
	{
		if (value == 0)
			return 0.0;  // Returns OleAut's zero'ed date value.
		if (value < TicksPerDay) // This is a fix for VB. They want the default day to be 1/1/0001 rathar then 12/30/1899.
			value += DoubleDateOffset; // We could have moved this fix down but we would like to keep the bounds check.
		if (value < OADateMinAsTicks)
			throw new OverflowException(SR.Arg_OleAutDateInvalid);

		// Currently, our max date == OA's max date (12/31/9999), so we don't
		// need an overflow check in that direction.
		long millis = (value - DoubleDateOffset) / TicksPerMillisecond;
		if (millis < 0)
		{
			long frac = millis % MillisPerDay;
			if (frac != 0) millis -= (MillisPerDay + frac) * 2;
		}
		return (double)millis / MillisPerDay;
	}

	// Converts the DateTime instance into an OLE Automation compatible
	// double date.
	public double ToOADate()
	{
		return TicksToOADate(Ticks);
	}

	public static DateTime operator +(DateTime d, TimeSpan t)
	{
		ulong ticks = (ulong)(d.Ticks + t._ticks);
		if (ticks > MaxTicks) ThrowDateArithmetic(1);
		return new DateTime(ticks | d.InternalKind);
	}

	public static DateTime operator -(DateTime d, TimeSpan t)
	{
		ulong ticks = (ulong)(d.Ticks - t._ticks);
		if (ticks > MaxTicks) ThrowDateArithmetic(1);
		return new DateTime(ticks | d.InternalKind);
	}

	public static TimeSpan operator -(DateTime d1, DateTime d2) => new TimeSpan(d1.Ticks - d2.Ticks);

	public static bool operator ==(DateTime d1, DateTime d2) => ((d1._dateData ^ d2._dateData) << 2) == 0;

	public static bool operator !=(DateTime d1, DateTime d2) => !(d1 == d2);

	public static bool operator <(DateTime t1, DateTime t2) => t1.Ticks < t2.Ticks;

	public static bool operator <=(DateTime t1, DateTime t2) => t1.Ticks <= t2.Ticks;

	public static bool operator >(DateTime t1, DateTime t2) => t1.Ticks > t2.Ticks;

	public static bool operator >=(DateTime t1, DateTime t2) => t1.Ticks >= t2.Ticks;

	// Tries to construct a DateTime from a given year, month, day, hour,
	// minute, second and millisecond.
	//
	internal static bool TryCreate(int year, int month, int day, int hour, int minute, int second, int millisecond, out DateTime result)
	{
		result = default;
		if (year is < 1 or > 9999 || month is < 1 or > 12 || day < 1)
		{
			return false;
		}
		if ((uint)hour >= 24 || (uint)minute >= 60 || (uint)millisecond >= MillisPerSecond)
		{
			return false;
		}

		uint[] days = IsLeapYear(year) ? s_daysToMonth366 : s_daysToMonth365;
		if ((uint)day > days[month] - days[month - 1])
		{
			return false;
		}
		ulong ticks = (DaysToYear((uint)year) + days[month - 1] + (uint)day - 1) * (ulong)TicksPerDay;

		if ((uint)second < 60)
		{
			ticks += TimeToTicks(hour, minute, second) + (uint)millisecond * (uint)TicksPerMillisecond;
		}
		else if (second == 60 && s_systemSupportsLeapSeconds && IsValidTimeWithLeapSeconds(year, month, day, hour, minute, DateTimeKind.Unspecified))
		{
			// if we have leap second (second = 60) then we'll need to check if it is valid time.
			// if it is valid, then we adjust the second to 59 so DateTime will consider this second is last second
			// of this minute.
			// if it is not valid time, we'll eventually throw.
			// although this is unspecified datetime kind, we'll assume the passed time is UTC to check the leap seconds.
			ticks += TimeToTicks(hour, minute, 59) + 999 * TicksPerMillisecond;
		}
		else
		{
			return false;
		}

		Debug.Assert(ticks <= MaxTicks, "Input parameters validated already");
		result = new DateTime(ticks);
		return true;
	}
}
