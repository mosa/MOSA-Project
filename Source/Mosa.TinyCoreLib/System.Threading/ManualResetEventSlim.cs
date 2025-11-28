using System.Runtime.Versioning;

namespace System.Threading;

public class ManualResetEventSlim : IDisposable
{
	public bool IsSet
	{
		get
		{
			throw null;
		}
	}

	public int SpinCount
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

	public ManualResetEventSlim()
	{
	}

	public ManualResetEventSlim(bool initialState)
	{
	}

	public ManualResetEventSlim(bool initialState, int spinCount)
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

	public void Set()
	{
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
