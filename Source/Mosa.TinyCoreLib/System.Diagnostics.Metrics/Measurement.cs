using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Diagnostics.Metrics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct Measurement<T> where T : struct
{
	public ReadOnlySpan<KeyValuePair<string, object?>> Tags
	{
		get
		{
			throw null;
		}
	}

	public T Value
	{
		get
		{
			throw null;
		}
	}

	public Measurement(T value)
	{
		throw null;
	}

	public Measurement(T value, IEnumerable<KeyValuePair<string, object?>>? tags)
	{
		throw null;
	}

	public Measurement(T value, params KeyValuePair<string, object?>[]? tags)
	{
		throw null;
	}

	public Measurement(T value, ReadOnlySpan<KeyValuePair<string, object?>> tags)
	{
		throw null;
	}
}
