using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

public abstract class DbTransaction : MarshalByRefObject, IDbTransaction, IDisposable, IAsyncDisposable
{
	public DbConnection? Connection
	{
		get
		{
			throw null;
		}
	}

	protected abstract DbConnection? DbConnection { get; }

	public abstract IsolationLevel IsolationLevel { get; }

	IDbConnection? IDbTransaction.Connection
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsSavepoints
	{
		get
		{
			throw null;
		}
	}

	public abstract void Commit();

	public virtual Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}

	public abstract void Rollback();

	public virtual Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task SaveAsync(string savepointName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task RollbackAsync(string savepointName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task ReleaseAsync(string savepointName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual void Save(string savepointName)
	{
		throw null;
	}

	public virtual void Rollback(string savepointName)
	{
		throw null;
	}

	public virtual void Release(string savepointName)
	{
		throw null;
	}
}
