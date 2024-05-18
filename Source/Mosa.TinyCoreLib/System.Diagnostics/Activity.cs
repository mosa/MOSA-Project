using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

public class Activity : IDisposable
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Enumerator<T>
	{
		public readonly ref T Current
		{
			get
			{
				throw null;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public readonly Enumerator<T> GetEnumerator()
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}
	}

	public ActivityTraceFlags ActivityTraceFlags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IEnumerable<KeyValuePair<string, string?>> Baggage
	{
		get
		{
			throw null;
		}
	}

	public static Activity? Current
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static ActivityIdFormat DefaultIdFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan Duration
	{
		get
		{
			throw null;
		}
	}

	public static bool ForceDefaultIdFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Id
	{
		get
		{
			throw null;
		}
	}

	public bool HasRemoteParent
	{
		get
		{
			throw null;
		}
	}

	public bool IsAllDataRequested
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public bool IsStopped
	{
		get
		{
			throw null;
		}
	}

	public ActivityIdFormat IdFormat
	{
		get
		{
			throw null;
		}
	}

	public ActivityKind Kind
	{
		get
		{
			throw null;
		}
	}

	public string OperationName
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
		set
		{
			throw null;
		}
	}

	public ActivitySource Source
	{
		get
		{
			throw null;
		}
	}

	public Activity? Parent
	{
		get
		{
			throw null;
		}
	}

	public string? ParentId
	{
		get
		{
			throw null;
		}
	}

	public ActivitySpanId ParentSpanId
	{
		get
		{
			throw null;
		}
	}

	public bool Recorded
	{
		get
		{
			throw null;
		}
	}

	public string? RootId
	{
		get
		{
			throw null;
		}
	}

	public ActivitySpanId SpanId
	{
		get
		{
			throw null;
		}
	}

	public DateTime StartTimeUtc
	{
		get
		{
			throw null;
		}
	}

	public ActivityStatusCode Status
	{
		get
		{
			throw null;
		}
	}

	public string? StatusDescription
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<KeyValuePair<string, string?>> Tags
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<KeyValuePair<string, object?>> TagObjects
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<ActivityEvent> Events
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<ActivityLink> Links
	{
		get
		{
			throw null;
		}
	}

	public ActivityTraceId TraceId
	{
		get
		{
			throw null;
		}
	}

	public string? TraceStateString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static Func<ActivityTraceId>? TraceIdGenerator
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public ActivityContext Context
	{
		get
		{
			throw null;
		}
	}

	public static event EventHandler<ActivityChangedEventArgs>? CurrentChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public Activity(string operationName)
	{
	}

	public Activity AddBaggage(string key, string? value)
	{
		throw null;
	}

	public Activity AddEvent(ActivityEvent e)
	{
		throw null;
	}

	public Activity AddTag(string key, string? value)
	{
		throw null;
	}

	public Activity AddTag(string key, object? value)
	{
		throw null;
	}

	public Activity SetTag(string key, object? value)
	{
		throw null;
	}

	public Activity SetBaggage(string key, string? value)
	{
		throw null;
	}

	public string? GetBaggageItem(string key)
	{
		throw null;
	}

	public object? GetTagItem(string key)
	{
		throw null;
	}

	public Activity SetEndTime(DateTime endTimeUtc)
	{
		throw null;
	}

	public Activity SetIdFormat(ActivityIdFormat format)
	{
		throw null;
	}

	public Activity SetParentId(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags activityTraceFlags = ActivityTraceFlags.None)
	{
		throw null;
	}

	public Activity SetParentId(string parentId)
	{
		throw null;
	}

	public Activity SetStartTime(DateTime startTimeUtc)
	{
		throw null;
	}

	public Activity SetStatus(ActivityStatusCode code, string? description = null)
	{
		throw null;
	}

	public Activity Start()
	{
		throw null;
	}

	public void Stop()
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}

	protected virtual void Dispose(bool disposing)
	{
		throw null;
	}

	public void SetCustomProperty(string propertyName, object? propertyValue)
	{
		throw null;
	}

	public object? GetCustomProperty(string propertyName)
	{
		throw null;
	}

	public Enumerator<KeyValuePair<string, object?>> EnumerateTagObjects()
	{
		throw null;
	}

	public Enumerator<ActivityEvent> EnumerateEvents()
	{
		throw null;
	}

	public Enumerator<ActivityLink> EnumerateLinks()
	{
		throw null;
	}
}
