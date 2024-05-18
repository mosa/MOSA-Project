using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public sealed class UpDownCounter<T> : Instrument<T> where T : struct
{
	public void Add(T delta)
	{
		throw null;
	}

	public void Add(T delta, KeyValuePair<string, object?> tag)
	{
		throw null;
	}

	public void Add(T delta, KeyValuePair<string, object?> tag1, KeyValuePair<string, object?> tag2)
	{
		throw null;
	}

	public void Add(T delta, KeyValuePair<string, object?> tag1, KeyValuePair<string, object?> tag2, KeyValuePair<string, object?> tag3)
	{
		throw null;
	}

	public void Add(T delta, ReadOnlySpan<KeyValuePair<string, object?>> tags)
	{
		throw null;
	}

	public void Add(T delta, params KeyValuePair<string, object?>[] tags)
	{
		throw null;
	}

	public void Add(T delta, in TagList tagList)
	{
		throw null;
	}

	internal UpDownCounter(Meter meter, string name, string? unit, string? description)
		: base(meter, name, unit, description)
	{
		throw null;
	}
}
