using System.Runtime.Versioning;

namespace System.Threading;

public class CountdownEvent : IDisposable
{
	public int CurrentCount
	{
		get
		{
			throw null;
		}
	}

	public int InitialCount
	{
		get
		{
			throw null;
		}
	}

	public bool IsSet
	{
		get
		{
			throw null;
		}
	}

	public WaitHandle WaitHandle
	{
		get
		{
			throw null;
		}
	}

	public CountdownEvent(int initialCount)
	{
	}

	public void AddCount()
	{
	}

	public void AddCount(int signalCount)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Reset()
	{
	}

	public void Reset(int count)
	{
	}

	public bool Signal()
	{
		throw null;
	}

	public bool Signal(int signalCount)
	{
		throw null;
	}

	public bool TryAddCount()
	{
		throw null;
	}

	public bool TryAddCount(int signalCount)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public void Wait()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public bool Wait(int millisecondsTimeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public void Wait(CancellationToken cancellationToken)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public bool Wait(TimeSpan timeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}
}
