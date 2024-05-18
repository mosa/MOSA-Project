using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public sealed class Histogram<T> : Instrument<T> where T : struct
{
	internal Histogram(Meter meter, string name, string? unit, string? description)
		: base(meter, name, unit, description)
	{
		throw null;
	}

	public void Record(T value)
	{
		throw null;
	}

	public void Record(T value, KeyValuePair<string, object?> tag)
	{
		throw null;
	}

	public void Record(T value, KeyValuePair<string, object?> tag1, KeyValuePair<string, object?> tag2)
	{
		throw null;
	}

	public void Record(T value, KeyValuePair<string, object?> tag1, KeyValuePair<string, object?> tag2, KeyValuePair<string, object?> tag3)
	{
		throw null;
	}

	public void Record(T value, in TagList tagList)
	{
		throw null;
	}

	public void Record(T value, ReadOnlySpan<KeyValuePair<string, object?>> tags)
	{
		throw null;
	}

	public void Record(T value, params KeyValuePair<string, object?>[] tags)
	{
		throw null;
	}
}
