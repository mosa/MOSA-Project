namespace System.Threading;

public class SynchronizationContext
{
	public static SynchronizationContext? Current
	{
		get
		{
			throw null;
		}
	}

	public virtual SynchronizationContext CreateCopy()
	{
		throw null;
	}

	public bool IsWaitNotificationRequired()
	{
		throw null;
	}

	public virtual void OperationCompleted()
	{
	}

	public virtual void OperationStarted()
	{
	}

	public virtual void Post(SendOrPostCallback d, object? state)
	{
	}

	public virtual void Send(SendOrPostCallback d, object? state)
	{
	}

	public static void SetSynchronizationContext(SynchronizationContext? syncContext)
	{
	}

	protected void SetWaitNotificationRequired()
	{
	}

	[CLSCompliant(false)]
	public virtual int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
	{
		throw null;
	}

	[CLSCompliant(false)]
	protected static int WaitHelper(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
	{
		throw null;
	}
}
