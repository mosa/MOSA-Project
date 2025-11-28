using System.Security.AccessControl;

namespace System.Threading;

public static class ThreadingAclExtensions
{
	public static EventWaitHandleSecurity GetAccessControl(this EventWaitHandle handle)
	{
		throw null;
	}

	public static MutexSecurity GetAccessControl(this Mutex mutex)
	{
		throw null;
	}

	public static SemaphoreSecurity GetAccessControl(this Semaphore semaphore)
	{
		throw null;
	}

	public static void SetAccessControl(this EventWaitHandle handle, EventWaitHandleSecurity eventSecurity)
	{
	}

	public static void SetAccessControl(this Mutex mutex, MutexSecurity mutexSecurity)
	{
	}

	public static void SetAccessControl(this Semaphore semaphore, SemaphoreSecurity semaphoreSecurity)
	{
	}
}
