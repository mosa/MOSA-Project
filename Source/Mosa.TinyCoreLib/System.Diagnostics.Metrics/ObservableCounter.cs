using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public sealed class ObservableCounter<T> : ObservableInstrument<T> where T : struct
{
	internal ObservableCounter(Meter meter, string name, string? unit, string? description)
		: base(meter, name, unit, description)
	{
		throw null;
	}

	protected override IEnumerable<Measurement<T>> Observe()
	{
		throw null;
	}
}
