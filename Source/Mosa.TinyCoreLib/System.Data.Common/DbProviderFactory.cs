using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
public abstract class DbProviderFactory
{
	public virtual bool CanCreateBatch
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanCreateCommandBuilder
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanCreateDataAdapter
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanCreateDataSourceEnumerator
	{
		get
		{
			throw null;
		}
	}

	public virtual DbBatch CreateBatch()
	{
		throw null;
	}

	public virtual DbBatchCommand CreateBatchCommand()
	{
		throw null;
	}

	public virtual DbCommand? CreateCommand()
	{
		throw null;
	}

	public virtual DbCommandBuilder? CreateCommandBuilder()
	{
		throw null;
	}

	public virtual DbConnection? CreateConnection()
	{
		throw null;
	}

	public virtual DbConnectionStringBuilder? CreateConnectionStringBuilder()
	{
		throw null;
	}

	public virtual DbDataAdapter? CreateDataAdapter()
	{
		throw null;
	}

	public virtual DbDataSourceEnumerator? CreateDataSourceEnumerator()
	{
		throw null;
	}

	public virtual DbParameter? CreateParameter()
	{
		throw null;
	}

	public virtual DbDataSource CreateDataSource(string connectionString)
	{
		throw null;
	}
}
