using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public delegate void MeasurementCallback<T>(Instrument instrument, T measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state);
