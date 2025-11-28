using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct ActivityLink : IEquatable<ActivityLink>
{
	public ActivityContext Context
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

	public ActivityLink(ActivityContext context, ActivityTagsCollection? tags = null)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ActivityLink value)
	{
		throw null;
	}

	public static bool operator ==(ActivityLink left, ActivityLink right)
	{
		throw null;
	}

	public static bool operator !=(ActivityLink left, ActivityLink right)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public Activity.Enumerator<KeyValuePair<string, object?>> EnumerateTagObjects()
	{
		throw null;
	}
}
