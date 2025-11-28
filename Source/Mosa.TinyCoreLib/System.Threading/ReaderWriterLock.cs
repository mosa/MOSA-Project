using System.Runtime.ConstrainedExecution;
using System.Runtime.Versioning;

namespace System.Threading;

public sealed class ReaderWriterLock : CriticalFinalizerObject
{
	public bool IsReaderLockHeld
	{
		get
		{
			throw null;
		}
	}

	public bool IsWriterLockHeld
	{
		get
		{
			throw null;
		}
	}

	public int WriterSeqNum
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public void AcquireReaderLock(int millisecondsTimeout)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void AcquireReaderLock(TimeSpan timeout)
	{
	}

	public void AcquireWriterLock(int millisecondsTimeout)
	{
	}

	public void AcquireWriterLock(TimeSpan timeout)
	{
	}

	public bool AnyWritersSince(int seqNum)
	{
		throw null;
	}

	public void DowngradeFromWriterLock(ref LockCookie lockCookie)
	{
	}

	public LockCookie ReleaseLock()
	{
		throw null;
	}

	public void ReleaseReaderLock()
	{
	}

	public void ReleaseWriterLock()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void RestoreLock(ref LockCookie lockCookie)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public LockCookie UpgradeToWriterLock(TimeSpan timeout)
	{
		throw null;
	}
}
