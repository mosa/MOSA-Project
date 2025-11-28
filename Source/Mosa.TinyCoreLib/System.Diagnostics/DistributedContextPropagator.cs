using System.Collections.Generic;

namespace System.Diagnostics;

public abstract class DistributedContextPropagator
{
	public delegate void PropagatorGetterCallback(object? carrier, string fieldName, out string? fieldValue, out IEnumerable<string>? fieldValues);

	public delegate void PropagatorSetterCallback(object? carrier, string fieldName, string fieldValue);

	public abstract IReadOnlyCollection<string> Fields { get; }

	public static DistributedContextPropagator Current { get; set; }

	public abstract void Inject(Activity? activity, object? carrier, PropagatorSetterCallback? setter);

	public abstract void ExtractTraceIdAndState(object? carrier, PropagatorGetterCallback? getter, out string? traceId, out string? traceState);

	public abstract IEnumerable<KeyValuePair<string, string?>>? ExtractBaggage(object? carrier, PropagatorGetterCallback? getter);

	public static DistributedContextPropagator CreateDefaultPropagator()
	{
		throw null;
	}

	public static DistributedContextPropagator CreatePassThroughPropagator()
	{
		throw null;
	}

	public static DistributedContextPropagator CreateNoOutputPropagator()
	{
		throw null;
	}
}
