namespace System.Diagnostics.Metrics;

public sealed class MeterListener : IDisposable
{
	public Action<Instrument, MeterListener>? InstrumentPublished
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public Action<Instrument, object?>? MeasurementsCompleted
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public object? DisableMeasurementEvents(Instrument instrument)
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}

	public void EnableMeasurementEvents(Instrument instrument, object? state = null)
	{
		throw null;
	}

	public MeterListener()
	{
		throw null;
	}

	public void RecordObservableInstruments()
	{
		throw null;
	}

	public void SetMeasurementEventCallback<T>(MeasurementCallback<T>? measurementCallback) where T : struct
	{
		throw null;
	}

	public void Start()
	{
		throw null;
	}
}
