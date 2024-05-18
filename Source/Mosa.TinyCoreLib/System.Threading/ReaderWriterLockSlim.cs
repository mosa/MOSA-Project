namespace System.Threading;

public class ReaderWriterLockSlim : IDisposable
{
	public int CurrentReadCount
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadLockHeld
	{
		get
		{
			throw null;
		}
	}

	public bool IsUpgradeableReadLockHeld
	{
		get
		{
			throw null;
		}
	}

	public bool IsWriteLockHeld
	{
		get
		{
			throw null;
		}
	}

	public LockRecursionPolicy RecursionPolicy
	{
		get
		{
			throw null;
		}
	}

	public int RecursiveReadCount
	{
		get
		{
			throw null;
		}
	}

	public int RecursiveUpgradeCount
	{
		get
		{
			throw null;
		}
	}

	public int RecursiveWriteCount
	{
		get
		{
			throw null;
		}
	}

	public int WaitingReadCount
	{
		get
		{
			throw null;
		}
	}

	public int WaitingUpgradeCount
	{
		get
		{
			throw null;
		}
	}

	public int WaitingWriteCount
	{
		get
		{
			throw null;
		}
	}

	public ReaderWriterLockSlim()
	{
	}

	public ReaderWriterLockSlim(LockRecursionPolicy recursionPolicy)
	{
	}

	public void Dispose()
	{
	}

	public void EnterReadLock()
	{
	}

	public void EnterUpgradeableReadLock()
	{
	}

	public void EnterWriteLock()
	{
	}

	public void ExitReadLock()
	{
	}

	public void ExitUpgradeableReadLock()
	{
	}

	public void ExitWriteLock()
	{
	}

	public bool TryEnterReadLock(int millisecondsTimeout)
	{
		throw null;
	}

	public bool TryEnterReadLock(TimeSpan timeout)
	{
		throw null;
	}

	public bool TryEnterUpgradeableReadLock(int millisecondsTimeout)
	{
		throw null;
	}

	public bool TryEnterUpgradeableReadLock(TimeSpan timeout)
	{
		throw null;
	}

	public bool TryEnterWriteLock(int millisecondsTimeout)
	{
		throw null;
	}

	public bool TryEnterWriteLock(TimeSpan timeout)
	{
		throw null;
	}
}
