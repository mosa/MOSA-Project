using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System.Threading;

public static class ThreadPool
{
	public static long CompletedWorkItemCount
	{
		get
		{
			throw null;
		}
	}

	public static long PendingWorkItemCount
	{
		get
		{
			throw null;
		}
	}

	public static int ThreadCount
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("ThreadPool.BindHandle(IntPtr) has been deprecated. Use ThreadPool.BindHandle(SafeHandle) instead.")]
	[SupportedOSPlatform("windows")]
	public static bool BindHandle(IntPtr osHandle)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static bool BindHandle(SafeHandle osHandle)
	{
		throw null;
	}

	public static void GetAvailableThreads(out int workerThreads, out int completionPortThreads)
	{
		throw null;
	}

	public static void GetMaxThreads(out int workerThreads, out int completionPortThreads)
	{
		throw null;
	}

	public static void GetMinThreads(out int workerThreads, out int completionPortThreads)
	{
		throw null;
	}

	public static bool QueueUserWorkItem(WaitCallback callBack)
	{
		throw null;
	}

	public static bool QueueUserWorkItem(WaitCallback callBack, object? state)
	{
		throw null;
	}

	public static bool QueueUserWorkItem<TState>(Action<TState> callBack, TState state, bool preferLocal)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, int millisecondsTimeOutInterval, bool executeOnlyOnce)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, long millisecondsTimeOutInterval, bool executeOnlyOnce)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, TimeSpan timeout, bool executeOnlyOnce)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, uint millisecondsTimeOutInterval, bool executeOnlyOnce)
	{
		throw null;
	}

	public static bool SetMaxThreads(int workerThreads, int completionPortThreads)
	{
		throw null;
	}

	public static bool SetMinThreads(int workerThreads, int completionPortThreads)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[SupportedOSPlatform("windows")]
	public unsafe static bool UnsafeQueueNativeOverlapped(NativeOverlapped* overlapped)
	{
		throw null;
	}

	public static bool UnsafeQueueUserWorkItem(IThreadPoolWorkItem callBack, bool preferLocal)
	{
		throw null;
	}

	public static bool UnsafeQueueUserWorkItem(WaitCallback callBack, object? state)
	{
		throw null;
	}

	public static bool UnsafeQueueUserWorkItem<TState>(Action<TState> callBack, TState state, bool preferLocal)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, int millisecondsTimeOutInterval, bool executeOnlyOnce)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, long millisecondsTimeOutInterval, bool executeOnlyOnce)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, TimeSpan timeout, bool executeOnlyOnce)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[UnsupportedOSPlatform("browser")]
	public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object? state, uint millisecondsTimeOutInterval, bool executeOnlyOnce)
	{
		throw null;
	}
}
