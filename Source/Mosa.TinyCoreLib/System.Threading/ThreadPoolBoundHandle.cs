using System.Runtime.InteropServices;

namespace System.Threading;

public sealed class ThreadPoolBoundHandle : IDisposable
{
	public SafeHandle Handle
	{
		get
		{
			throw null;
		}
	}

	internal ThreadPoolBoundHandle()
	{
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* AllocateNativeOverlapped(IOCompletionCallback callback, object? state, object? pinData)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* AllocateNativeOverlapped(PreAllocatedOverlapped preAllocated)
	{
		throw null;
	}

	public static ThreadPoolBoundHandle BindHandle(SafeHandle handle)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	[CLSCompliant(false)]
	public unsafe void FreeNativeOverlapped(NativeOverlapped* overlapped)
	{
	}

	[CLSCompliant(false)]
	public unsafe static object? GetNativeOverlappedState(NativeOverlapped* overlapped)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* UnsafeAllocateNativeOverlapped(IOCompletionCallback callback, object? state, object? pinData)
	{
		throw null;
	}
}
