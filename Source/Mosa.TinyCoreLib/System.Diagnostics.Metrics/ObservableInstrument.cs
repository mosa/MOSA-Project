using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public abstract class ObservableInstrument<T> : Instrument where T : struct
{
	public override bool IsObservable
	{
		get
		{
			throw null;
		}
	}

	protected ObservableInstrument(Meter meter, string name, string? unit, string? description)
		: this(meter, name, unit, description, (IEnumerable<KeyValuePair<string, object?>>)null)
	{
		throw null;
	}

	protected ObservableInstrument(Meter meter, string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags)
		: base(meter, name, unit, description)
	{
		throw null;
	}

	protected abstract IEnumerable<Measurement<T>> Observe();
}
