using System.Collections.Generic;

namespace System.Diagnostics.Metrics;

public class Meter : IDisposable
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string? Version
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<KeyValuePair<string, object?>>? Tags
	{
		get
		{
			throw null;
		}
	}

	public object? Scope
	{
		get
		{
			throw null;
		}
	}

	public Counter<T> CreateCounter<T>(string name, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public Counter<T> CreateCounter<T>(string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public UpDownCounter<T> CreateUpDownCounter<T>(string name, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public UpDownCounter<T> CreateUpDownCounter<T>(string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public Histogram<T> CreateHistogram<T>(string name, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public Histogram<T> CreateHistogram<T>(string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<T> observeValue, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<T> observeValue, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<T> observeValue, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<T> observeValue, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<Measurement<T>> observeValue, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<T> observeValue, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<T> observeValue, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<Measurement<T>> observeValue, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<Measurement<T>> observeValue, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit = null, string? description = null) where T : struct
	{
		throw null;
	}

	public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<IEnumerable<Measurement<T>>> observeValues, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>> tags) where T : struct
	{
		throw null;
	}

	protected virtual void Dispose(bool disposing)
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}

	public Meter(MeterOptions options)
	{
		throw null;
	}

	public Meter(string name)
	{
		throw null;
	}

	public Meter(string name, string? version)
	{
		throw null;
	}

	public Meter(string name, string? version, IEnumerable<KeyValuePair<string, object?>>? tags, object? scope = null)
	{
		throw null;
	}
}
