using Microsoft.Win32.SafeHandles;

namespace System.Threading;

public static class WaitHandleExtensions
{
	public static SafeWaitHandle GetSafeWaitHandle(this WaitHandle waitHandle)
	{
		throw null;
	}

	public static void SetSafeWaitHandle(this WaitHandle waitHandle, SafeWaitHandle? value)
	{
	}
}
