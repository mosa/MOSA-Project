using System.Runtime.ConstrainedExecution;

namespace System.Runtime.InteropServices;

public abstract class SafeHandle : CriticalFinalizerObject, IDisposable
{
	protected IntPtr handle;

	public bool IsClosed
	{
		get
		{
			throw null;
		}
	}

	public abstract bool IsInvalid { get; }

	protected SafeHandle(IntPtr invalidHandleValue, bool ownsHandle)
	{
	}

	public void Close()
	{
	}

	public void DangerousAddRef(ref bool success)
	{
	}

	public IntPtr DangerousGetHandle()
	{
		throw null;
	}

	public void DangerousRelease()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~SafeHandle()
	{
	}

	protected abstract bool ReleaseHandle();

	protected void SetHandle(IntPtr handle)
	{
	}

	public void SetHandleAsInvalid()
	{
	}
}
