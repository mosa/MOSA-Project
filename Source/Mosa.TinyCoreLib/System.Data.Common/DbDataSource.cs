using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

public abstract class DbDataSource : IDisposable, IAsyncDisposable
{
	public abstract string ConnectionString { get; }

	protected abstract DbConnection CreateDbConnection();

	protected virtual DbConnection OpenDbConnection()
	{
		throw null;
	}

	protected virtual ValueTask<DbConnection> OpenDbConnectionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected virtual DbCommand CreateDbCommand(string? commandText = null)
	{
		throw null;
	}

	protected virtual DbBatch CreateDbBatch()
	{
		throw null;
	}

	public DbConnection CreateConnection()
	{
		throw null;
	}

	public DbConnection OpenConnection()
	{
		throw null;
	}

	public ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public DbCommand CreateCommand(string? commandText = null)
	{
		throw null;
	}

	public DbBatch CreateBatch()
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	protected virtual void Dispose(bool disposing)
	{
		throw null;
	}

	protected virtual ValueTask DisposeAsyncCore()
	{
		throw null;
	}
}
