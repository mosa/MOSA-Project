using System.Threading.Tasks;

namespace System.Threading;

public class CancellationTokenSource : IDisposable
{
	public bool IsCancellationRequested
	{
		get
		{
			throw null;
		}
	}

	public CancellationToken Token
	{
		get
		{
			throw null;
		}
	}

	public CancellationTokenSource()
	{
	}

	public CancellationTokenSource(TimeSpan delay, TimeProvider timeProvider)
	{
	}

	public CancellationTokenSource(int millisecondsDelay)
	{
	}

	public CancellationTokenSource(TimeSpan delay)
	{
	}

	public void Cancel()
	{
	}

	public void Cancel(bool throwOnFirstException)
	{
	}

	public void CancelAfter(int millisecondsDelay)
	{
	}

	public void CancelAfter(TimeSpan delay)
	{
	}

	public Task CancelAsync()
	{
		throw null;
	}

	public static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token)
	{
		throw null;
	}

	public static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2)
	{
		throw null;
	}

	public static CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public bool TryReset()
	{
		throw null;
	}
}
