namespace System.Threading;

public struct SpinLock
{
	private int _dummyPrimitive;

	public bool IsHeld
	{
		get
		{
			throw null;
		}
	}

	public bool IsHeldByCurrentThread
	{
		get
		{
			throw null;
		}
	}

	public bool IsThreadOwnerTrackingEnabled
	{
		get
		{
			throw null;
		}
	}

	public SpinLock(bool enableThreadOwnerTracking)
	{
		throw null;
	}

	public void Enter(ref bool lockTaken)
	{
	}

	public void Exit()
	{
	}

	public void Exit(bool useMemoryBarrier)
	{
	}

	public void TryEnter(ref bool lockTaken)
	{
	}

	public void TryEnter(int millisecondsTimeout, ref bool lockTaken)
	{
	}

	public void TryEnter(TimeSpan timeout, ref bool lockTaken)
	{
	}
}
