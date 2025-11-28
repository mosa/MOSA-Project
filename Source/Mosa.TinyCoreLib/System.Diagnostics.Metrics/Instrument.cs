using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public abstract class Instrument
{
	public string? Description
	{
		get
		{
			throw null;
		}
	}

	public bool Enabled
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsObservable
	{
		get
		{
			throw null;
		}
	}

	public Meter Meter
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

	public string? Unit
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<KeyValuePair<string, object?>>? Tags { get; }

	protected Instrument(Meter meter, string name, string? unit, string? description)
	{
		throw null;
	}

	protected Instrument(Meter meter, string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>>? tags)
	{
		throw null;
	}

	protected void Publish()
	{
		throw null;
	}
}
public abstract class Instrument<T> : Instrument where T : struct
{
	protected Instrument(Meter meter, string name, string? unit, string? description)
		: base(meter, name, unit, description)
	{
		throw null;
	}

	protected Instrument(Meter meter, string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>>? tags)
		: base(meter, name, unit, description, tags)
	{
		throw null;
	}

	protected void RecordMeasurement(T measurement)
	{
		throw null;
	}

	protected void RecordMeasurement(T measurement, KeyValuePair<string, object?> tag)
	{
		throw null;
	}

	protected void RecordMeasurement(T measurement, KeyValuePair<string, object?> tag1, KeyValuePair<string, object?> tag2)
	{
		throw null;
	}

	protected void RecordMeasurement(T measurement, KeyValuePair<string, object?> tag1, KeyValuePair<string, object?> tag2, KeyValuePair<string, object?> tag3)
	{
		throw null;
	}

	protected void RecordMeasurement(T measurement, in TagList tagList)
	{
		throw null;
	}

	protected void RecordMeasurement(T measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags)
	{
		throw null;
	}
}
