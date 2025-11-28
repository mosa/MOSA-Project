using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

public abstract class DbBatch : IDisposable, IAsyncDisposable
{
	public DbBatchCommandCollection BatchCommands
	{
		get
		{
			throw null;
		}
	}

	protected abstract DbBatchCommandCollection DbBatchCommands { get; }

	public abstract int Timeout { get; set; }

	public DbConnection? Connection { get; set; }

	protected abstract DbConnection? DbConnection { get; set; }

	public DbTransaction? Transaction { get; set; }

	protected abstract DbTransaction? DbTransaction { get; set; }

	public DbDataReader ExecuteReader(CommandBehavior behavior = CommandBehavior.Default)
	{
		throw null;
	}

	protected abstract DbDataReader ExecuteDbDataReader(CommandBehavior behavior);

	public Task<DbDataReader> ExecuteReaderAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected abstract Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken);

	public abstract int ExecuteNonQuery();

	public abstract Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default(CancellationToken));

	public abstract object? ExecuteScalar();

	public abstract Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken = default(CancellationToken));

	public abstract void Prepare();

	public abstract Task PrepareAsync(CancellationToken cancellationToken = default(CancellationToken));

	public abstract void Cancel();

	public DbBatchCommand CreateBatchCommand()
	{
		throw null;
	}

	protected abstract DbBatchCommand CreateDbBatchCommand();

	public virtual void Dispose()
	{
		throw null;
	}

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}
}
