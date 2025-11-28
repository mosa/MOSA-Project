using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System;

public sealed class TimeZoneInfo : IEquatable<TimeZoneInfo?>, IDeserializationCallback, ISerializable
{
	public sealed class AdjustmentRule : IEquatable<AdjustmentRule?>, IDeserializationCallback, ISerializable
	{
		public TimeSpan BaseUtcOffsetDelta
		{
			get
			{
				throw null;
			}
		}

		public DateTime DateEnd
		{
			get
			{
				throw null;
			}
		}

		public DateTime DateStart
		{
			get
			{
				throw null;
			}
		}

		public TimeSpan DaylightDelta
		{
			get
			{
				throw null;
			}
		}

		public TransitionTime DaylightTransitionEnd
		{
			get
			{
				throw null;
			}
		}

		public TransitionTime DaylightTransitionStart
		{
			get
			{
				throw null;
			}
		}

		internal AdjustmentRule()
		{
		}

		public static AdjustmentRule CreateAdjustmentRule(DateTime dateStart, DateTime dateEnd, TimeSpan daylightDelta, TransitionTime daylightTransitionStart, TransitionTime daylightTransitionEnd)
		{
			throw null;
		}

		public static AdjustmentRule CreateAdjustmentRule(DateTime dateStart, DateTime dateEnd, TimeSpan daylightDelta, TransitionTime daylightTransitionStart, TransitionTime daylightTransitionEnd, TimeSpan baseUtcOffsetDelta)
		{
			throw null;
		}

		public bool Equals([NotNullWhen(true)] AdjustmentRule? other)
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

		void IDeserializationCallback.OnDeserialization(object? sender)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}
	}

	public readonly struct TransitionTime : IEquatable<TransitionTime>, IDeserializationCallback, ISerializable
	{
		private readonly int _dummyPrimitive;

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

		public bool IsFixedDateRule
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

		public DateTime TimeOfDay
		{
			get
			{
				throw null;
			}
		}

		public int Week
		{
			get
			{
				throw null;
			}
		}

		public static TransitionTime CreateFixedDateRule(DateTime timeOfDay, int month, int day)
		{
			throw null;
		}

		public static TransitionTime CreateFloatingDateRule(DateTime timeOfDay, int month, int week, DayOfWeek dayOfWeek)
		{
			throw null;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			throw null;
		}

		public bool Equals(TransitionTime other)
		{
			throw null;
		}

		public override int GetHashCode()
		{
			throw null;
		}

		public static bool operator ==(TransitionTime t1, TransitionTime t2)
		{
			throw null;
		}

		public static bool operator !=(TransitionTime t1, TransitionTime t2)
		{
			throw null;
		}

		void IDeserializationCallback.OnDeserialization(object? sender)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}
	}

	public TimeSpan BaseUtcOffset
	{
		get
		{
			throw null;
		}
	}

	public string DaylightName
	{
		get
		{
			throw null;
		}
	}

	public string DisplayName
	{
		get
		{
			throw null;
		}
	}

	public bool HasIanaId
	{
		get
		{
			throw null;
		}
	}

	public string Id
	{
		get
		{
			throw null;
		}
	}

	public static TimeZoneInfo Local
	{
		get
		{
			throw null;
		}
	}

	public string StandardName
	{
		get
		{
			throw null;
		}
	}

	public bool SupportsDaylightSavingTime
	{
		get
		{
			throw null;
		}
	}

	public static TimeZoneInfo Utc
	{
		get
		{
			throw null;
		}
	}

	internal TimeZoneInfo()
	{
	}

	public static void ClearCachedData()
	{
	}

	public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo destinationTimeZone)
	{
		throw null;
	}

	public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
	{
		throw null;
	}

	public static DateTimeOffset ConvertTime(DateTimeOffset dateTimeOffset, TimeZoneInfo destinationTimeZone)
	{
		throw null;
	}

	public static DateTime ConvertTimeBySystemTimeZoneId(DateTime dateTime, string destinationTimeZoneId)
	{
		throw null;
	}

	public static DateTime ConvertTimeBySystemTimeZoneId(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
	{
		throw null;
	}

	public static DateTimeOffset ConvertTimeBySystemTimeZoneId(DateTimeOffset dateTimeOffset, string destinationTimeZoneId)
	{
		throw null;
	}

	public static DateTime ConvertTimeFromUtc(DateTime dateTime, TimeZoneInfo destinationTimeZone)
	{
		throw null;
	}

	public static DateTime ConvertTimeToUtc(DateTime dateTime)
	{
		throw null;
	}

	public static DateTime ConvertTimeToUtc(DateTime dateTime, TimeZoneInfo sourceTimeZone)
	{
		throw null;
	}

	public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string? displayName, string? standardDisplayName)
	{
		throw null;
	}

	public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string? displayName, string? standardDisplayName, string? daylightDisplayName, AdjustmentRule[]? adjustmentRules)
	{
		throw null;
	}

	public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string? displayName, string? standardDisplayName, string? daylightDisplayName, AdjustmentRule[]? adjustmentRules, bool disableDaylightSavingTime)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] TimeZoneInfo? other)
	{
		throw null;
	}

	public static TimeZoneInfo FindSystemTimeZoneById(string id)
	{
		throw null;
	}

	public static bool TryFindSystemTimeZoneById(string id, [NotNullWhen(true)] out TimeZoneInfo? timeZoneInfo)
	{
		throw null;
	}

	public static TimeZoneInfo FromSerializedString(string source)
	{
		throw null;
	}

	public AdjustmentRule[] GetAdjustmentRules()
	{
		throw null;
	}

	public TimeSpan[] GetAmbiguousTimeOffsets(DateTime dateTime)
	{
		throw null;
	}

	public TimeSpan[] GetAmbiguousTimeOffsets(DateTimeOffset dateTimeOffset)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
	{
		throw null;
	}

	public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones(bool skipSorting)
	{
		throw null;
	}

	public TimeSpan GetUtcOffset(DateTime dateTime)
	{
		throw null;
	}

	public TimeSpan GetUtcOffset(DateTimeOffset dateTimeOffset)
	{
		throw null;
	}

	public bool HasSameRules(TimeZoneInfo other)
	{
		throw null;
	}

	public bool IsAmbiguousTime(DateTime dateTime)
	{
		throw null;
	}

	public bool IsAmbiguousTime(DateTimeOffset dateTimeOffset)
	{
		throw null;
	}

	public bool IsDaylightSavingTime(DateTime dateTime)
	{
		throw null;
	}

	public bool IsDaylightSavingTime(DateTimeOffset dateTimeOffset)
	{
		throw null;
	}

	public bool IsInvalidTime(DateTime dateTime)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public string ToSerializedString()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryConvertIanaIdToWindowsId(string ianaId, [NotNullWhen(true)] out string? windowsId)
	{
		throw null;
	}

	public static bool TryConvertWindowsIdToIanaId(string windowsId, string? region, [NotNullWhen(true)] out string? ianaId)
	{
		throw null;
	}

	public static bool TryConvertWindowsIdToIanaId(string windowsId, [NotNullWhen(true)] out string? ianaId)
	{
		throw null;
	}
}
