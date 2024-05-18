using System.Threading.Tasks;

namespace System.Threading;

public sealed class PeriodicTimer : IDisposable
{
	public TimeSpan Period
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PeriodicTimer(TimeSpan period)
	{
	}

	public PeriodicTimer(TimeSpan period, TimeProvider timeProvider)
	{
	}

	public void Dispose()
	{
	}

	~PeriodicTimer()
	{
	}

	public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
