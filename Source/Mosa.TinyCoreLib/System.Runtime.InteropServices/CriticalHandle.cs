using System.Runtime.ConstrainedExecution;

namespace System.Runtime.InteropServices;

public abstract class CriticalHandle : CriticalFinalizerObject, IDisposable
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

	protected CriticalHandle(IntPtr invalidHandleValue)
	{
	}

	public void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~CriticalHandle()
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
