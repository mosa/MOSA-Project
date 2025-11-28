using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Diagnostics;

public sealed class ActivitySource : IDisposable
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string? Version
	{
		get
		{
			throw null;
		}
	}

	public ActivitySource(string name, string? version = "")
	{
		throw null;
	}

	public bool HasListeners()
	{
		throw null;
	}

	public Activity? CreateActivity(string name, ActivityKind kind)
	{
		throw null;
	}

	public Activity? CreateActivity(string name, ActivityKind kind, ActivityContext parentContext, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, ActivityIdFormat idFormat = ActivityIdFormat.Unknown)
	{
		throw null;
	}

	public Activity? CreateActivity(string name, ActivityKind kind, string? parentId, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, ActivityIdFormat idFormat = ActivityIdFormat.Unknown)
	{
		throw null;
	}

	public Activity? StartActivity([CallerMemberName] string name = "", ActivityKind kind = ActivityKind.Internal)
	{
		throw null;
	}

	public Activity? StartActivity(string name, ActivityKind kind, ActivityContext parentContext, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default(DateTimeOffset))
	{
		throw null;
	}

	public Activity? StartActivity(string name, ActivityKind kind, string? parentId, IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default(DateTimeOffset))
	{
		throw null;
	}

	public Activity? StartActivity(ActivityKind kind, ActivityContext parentContext = default(ActivityContext), IEnumerable<KeyValuePair<string, object?>>? tags = null, IEnumerable<ActivityLink>? links = null, DateTimeOffset startTime = default(DateTimeOffset), [CallerMemberName] string name = "")
	{
		throw null;
	}

	public static void AddActivityListener(ActivityListener listener)
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}
}
