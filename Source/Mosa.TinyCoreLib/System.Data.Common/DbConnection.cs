using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.Common;

public abstract class DbConnection : Component, IDbConnection, IDisposable, IAsyncDisposable
{
	[DefaultValue("")]
	[RecommendedAsConfigurable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[SettingsBindable(true)]
	public abstract string ConnectionString
	{
		get; [param: AllowNull]
		set;
	}

	public virtual int ConnectionTimeout
	{
		get
		{
			throw null;
		}
	}

	public abstract string Database { get; }

	public abstract string DataSource { get; }

	protected virtual DbProviderFactory? DbProviderFactory
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public abstract string ServerVersion { get; }

	[Browsable(false)]
	public abstract ConnectionState State { get; }

	public virtual bool CanCreateBatch
	{
		get
		{
			throw null;
		}
	}

	public virtual event StateChangeEventHandler? StateChange
	{
		add
		{
		}
		remove
		{
		}
	}

	protected abstract DbTransaction BeginDbTransaction(IsolationLevel isolationLevel);

	protected virtual ValueTask<DbTransaction> BeginDbTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
	{
		throw null;
	}

	public DbTransaction BeginTransaction()
	{
		throw null;
	}

	public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
	{
		throw null;
	}

	public ValueTask<DbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public abstract void ChangeDatabase(string databaseName);

	public virtual Task ChangeDatabaseAsync(string databaseName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public abstract void Close();

	public virtual Task CloseAsync()
	{
		throw null;
	}

	public DbBatch CreateBatch()
	{
		throw null;
	}

	protected virtual DbBatch CreateDbBatch()
	{
		throw null;
	}

	public DbCommand CreateCommand()
	{
		throw null;
	}

	protected abstract DbCommand CreateDbCommand();

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}

	public virtual void EnlistTransaction(Transaction? transaction)
	{
	}

	public virtual DataTable GetSchema()
	{
		throw null;
	}

	public virtual DataTable GetSchema(string collectionName)
	{
		throw null;
	}

	public virtual DataTable GetSchema(string collectionName, string?[] restrictionValues)
	{
		throw null;
	}

	public virtual Task<DataTable> GetSchemaAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task<DataTable> GetSchemaAsync(string collectionName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task<DataTable> GetSchemaAsync(string collectionName, string?[] restrictionValues, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	protected virtual void OnStateChange(StateChangeEventArgs stateChange)
	{
	}

	public abstract void Open();

	public Task OpenAsync()
	{
		throw null;
	}

	public virtual Task OpenAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	IDbTransaction IDbConnection.BeginTransaction()
	{
		throw null;
	}

	IDbTransaction IDbConnection.BeginTransaction(IsolationLevel isolationLevel)
	{
		throw null;
	}

	IDbCommand IDbConnection.CreateCommand()
	{
		throw null;
	}
}
