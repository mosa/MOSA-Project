using System.ComponentModel;

namespace System.Timers;

[DefaultEvent("Elapsed")]
[DefaultProperty("Interval")]
public class Timer : Component, ISupportInitialize
{
	[DefaultValue(true)]
	public bool AutoReset
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(100.0)]
	public double Interval
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override ISite? Site
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public ISynchronizeInvoke? SynchronizingObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event ElapsedEventHandler Elapsed
	{
		add
		{
		}
		remove
		{
		}
	}

	public Timer()
	{
	}

	public Timer(double interval)
	{
	}

	public Timer(TimeSpan interval)
	{
	}

	public void BeginInit()
	{
	}

	public void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void EndInit()
	{
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}
}
