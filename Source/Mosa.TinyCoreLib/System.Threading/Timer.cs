using System.Threading.Tasks;

namespace System.Threading;

public sealed class Timer : MarshalByRefObject, IAsyncDisposable, IDisposable, ITimer
{
	public static long ActiveCount
	{
		get
		{
			throw null;
		}
	}

	public Timer(TimerCallback callback)
	{
	}

	public Timer(TimerCallback callback, object? state, int dueTime, int period)
	{
	}

	public Timer(TimerCallback callback, object? state, long dueTime, long period)
	{
	}

	public Timer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
	{
	}

	[CLSCompliant(false)]
	public Timer(TimerCallback callback, object? state, uint dueTime, uint period)
	{
	}

	public bool Change(int dueTime, int period)
	{
		throw null;
	}

	public bool Change(long dueTime, long period)
	{
		throw null;
	}

	public bool Change(TimeSpan dueTime, TimeSpan period)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public bool Change(uint dueTime, uint period)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public bool Dispose(WaitHandle notifyObject)
	{
		throw null;
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}
}
