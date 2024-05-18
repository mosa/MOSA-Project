using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public static class MeterFactoryExtensions
{
	public static Meter Create(this IMeterFactory meterFactory, string name, string? version = null, IEnumerable<KeyValuePair<string, object?>>? tags = null)
	{
		return null;
	}
}
