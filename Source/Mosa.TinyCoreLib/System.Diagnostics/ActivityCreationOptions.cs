using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct ActivityCreationOptions<T>
{
	public ActivitySource Source
	{
		get
		{
			throw null;
		}
	}

	public string Name
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

	public T Parent
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<KeyValuePair<string, object?>>? Tags
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<ActivityLink>? Links
	{
		get
		{
			throw null;
		}
	}

	public ActivityTagsCollection SamplingTags
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

	public string? TraceState
	{
		get
		{
			throw null;
		}
		init
		{
			throw null;
		}
	}
}
