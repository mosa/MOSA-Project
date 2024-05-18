using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

public abstract class DbCommand : Component, IDbCommand, IDisposable, IAsyncDisposable
{
	[DefaultValue("")]
	[RefreshProperties(RefreshProperties.All)]
	public abstract string CommandText
	{
		get; [param: AllowNull]
		set;
	}

	public abstract int CommandTimeout { get; set; }

	[DefaultValue(CommandType.Text)]
	[RefreshProperties(RefreshProperties.All)]
	public abstract CommandType CommandType { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DbConnection? Connection
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected abstract DbConnection? DbConnection { get; set; }

	protected abstract DbParameterCollection DbParameterCollection { get; }

	protected abstract DbTransaction? DbTransaction { get; set; }

	[Browsable(false)]
	[DefaultValue(true)]
	[DesignOnly(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract bool DesignTimeVisible { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DbParameterCollection Parameters
	{
		get
		{
			throw null;
		}
	}

	IDbConnection? IDbCommand.Connection
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	IDataParameterCollection IDbCommand.Parameters
	{
		get
		{
			throw null;
		}
	}

	IDbTransaction? IDbCommand.Transaction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DbTransaction? Transaction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(UpdateRowSource.Both)]
	public abstract UpdateRowSource UpdatedRowSource { get; set; }

	public abstract void Cancel();

	protected abstract DbParameter CreateDbParameter();

	public DbParameter CreateParameter()
	{
		throw null;
	}

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}

	protected abstract DbDataReader ExecuteDbDataReader(CommandBehavior behavior);

	protected virtual Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		throw null;
	}

	public abstract int ExecuteNonQuery();

	public Task<int> ExecuteNonQueryAsync()
	{
		throw null;
	}

	public virtual Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public DbDataReader ExecuteReader()
	{
		throw null;
	}

	public DbDataReader ExecuteReader(CommandBehavior behavior)
	{
		throw null;
	}

	public Task<DbDataReader> ExecuteReaderAsync()
	{
		throw null;
	}

	public Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior)
	{
		throw null;
	}

	public Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<DbDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public abstract object? ExecuteScalar();

	public Task<object?> ExecuteScalarAsync()
	{
		throw null;
	}

	public virtual Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public abstract void Prepare();

	public virtual Task PrepareAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	IDbDataParameter IDbCommand.CreateParameter()
	{
		throw null;
	}

	IDataReader IDbCommand.ExecuteReader()
	{
		throw null;
	}

	IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
	{
		throw null;
	}
}
