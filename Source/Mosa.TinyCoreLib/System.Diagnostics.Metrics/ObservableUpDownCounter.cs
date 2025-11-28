using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public sealed class ObservableUpDownCounter<T> : ObservableInstrument<T> where T : struct
{
	internal ObservableUpDownCounter(Meter meter, string name, string? unit, string? description)
		: base(meter, name, unit, description)
	{
		throw null;
	}

	protected override IEnumerable<Measurement<T>> Observe()
	{
		throw null;
	}
}
