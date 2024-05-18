using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32.SafeHandles;

namespace System.Threading;

public abstract class WaitHandle : MarshalByRefObject, IDisposable
{
	protected static readonly IntPtr InvalidHandle;

	public const int WaitTimeout = 258;

	[Obsolete("WaitHandle.Handle has been deprecated. Use the SafeWaitHandle property instead.")]
	public virtual IntPtr Handle
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SafeWaitHandle SafeWaitHandle
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public virtual void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool explicitDisposing)
	{
	}

	public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn)
	{
		throw null;
	}

	public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, int millisecondsTimeout, bool exitContext)
	{
		throw null;
	}

	public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, TimeSpan timeout, bool exitContext)
	{
		throw null;
	}

	public static bool WaitAll(WaitHandle[] waitHandles)
	{
		throw null;
	}

	public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout)
	{
		throw null;
	}

	public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
	{
		throw null;
	}

	public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout)
	{
		throw null;
	}

	public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext)
	{
		throw null;
	}

	public static int WaitAny(WaitHandle[] waitHandles)
	{
		throw null;
	}

	public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout)
	{
		throw null;
	}

	public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
	{
		throw null;
	}

	public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout)
	{
		throw null;
	}

	public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext)
	{
		throw null;
	}

	public virtual bool WaitOne()
	{
		throw null;
	}

	public virtual bool WaitOne(int millisecondsTimeout)
	{
		throw null;
	}

	public virtual bool WaitOne(int millisecondsTimeout, bool exitContext)
	{
		throw null;
	}

	public virtual bool WaitOne(TimeSpan timeout)
	{
		throw null;
	}

	public virtual bool WaitOne(TimeSpan timeout, bool exitContext)
	{
		throw null;
	}
}
