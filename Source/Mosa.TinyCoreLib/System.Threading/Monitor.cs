using System.Runtime.Versioning;

namespace System.Threading;

public static class Monitor
{
	public static long LockContentionCount
	{
		get
		{
			throw null;
		}
	}

	public static void Enter(object obj)
	{
	}

	public static void Enter(object obj, ref bool lockTaken)
	{
	}

	public static void Exit(object obj)
	{
	}

	public static bool IsEntered(object obj)
	{
		throw null;
	}

	public static void Pulse(object obj)
	{
	}

	public static void PulseAll(object obj)
	{
	}

	public static bool TryEnter(object obj)
	{
		throw null;
	}

	public static void TryEnter(object obj, ref bool lockTaken)
	{
	}

	public static bool TryEnter(object obj, int millisecondsTimeout)
	{
		throw null;
	}

	public static void TryEnter(object obj, int millisecondsTimeout, ref bool lockTaken)
	{
	}

	public static bool TryEnter(object obj, TimeSpan timeout)
	{
		throw null;
	}

	public static void TryEnter(object obj, TimeSpan timeout, ref bool lockTaken)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public static bool Wait(object obj)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static bool Wait(object obj, int millisecondsTimeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static bool Wait(object obj, int millisecondsTimeout, bool exitContext)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static bool Wait(object obj, TimeSpan timeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static bool Wait(object obj, TimeSpan timeout, bool exitContext)
	{
		throw null;
	}
}
