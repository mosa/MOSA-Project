namespace System.Threading;

public struct SpinWait
{
	private int _dummyPrimitive;

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool NextSpinWillYield
	{
		get
		{
			throw null;
		}
	}

	public void Reset()
	{
	}

	public void SpinOnce()
	{
	}

	public void SpinOnce(int sleep1Threshold)
	{
	}

	public static void SpinUntil(Func<bool> condition)
	{
	}

	public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
	{
		throw null;
	}

	public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
	{
		throw null;
	}
}
