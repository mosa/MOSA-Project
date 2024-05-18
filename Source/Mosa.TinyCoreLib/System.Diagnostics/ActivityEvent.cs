using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct ActivityEvent
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset Timestamp
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<KeyValuePair<string, object?>> Tags
	{
		get
		{
			throw null;
		}
	}

	public ActivityEvent(string name)
	{
		throw null;
	}

	public ActivityEvent(string name, DateTimeOffset timestamp = default(DateTimeOffset), ActivityTagsCollection? tags = null)
	{
		throw null;
	}

	public Activity.Enumerator<KeyValuePair<string, object?>> EnumerateTagObjects()
	{
		throw null;
	}
}
