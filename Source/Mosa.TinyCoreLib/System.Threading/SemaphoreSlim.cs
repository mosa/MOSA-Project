using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace System.Threading;

public class SemaphoreSlim : IDisposable
{
	public WaitHandle AvailableWaitHandle
	{
		get
		{
			throw null;
		}
	}

	public int CurrentCount
	{
		get
		{
			throw null;
		}
	}

	public SemaphoreSlim(int initialCount)
	{
	}

	public SemaphoreSlim(int initialCount, int maxCount)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public int Release()
	{
		throw null;
	}

	public int Release(int releaseCount)
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

	public Task WaitAsync()
	{
		throw null;
	}

	public Task<bool> WaitAsync(int millisecondsTimeout)
	{
		throw null;
	}

	public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task WaitAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<bool> WaitAsync(TimeSpan timeout)
	{
		throw null;
	}

	public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}
}
