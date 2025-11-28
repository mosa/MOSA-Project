using System.Data.Common;

namespace System.Data.Odbc;

public sealed class OdbcFactory : DbProviderFactory
{
	public static readonly OdbcFactory Instance;

	internal OdbcFactory()
	{
	}

	public override DbCommand CreateCommand()
	{
		throw null;
	}

	public override DbCommandBuilder CreateCommandBuilder()
	{
		throw null;
	}

	public override DbConnection CreateConnection()
	{
		throw null;
	}

	public override DbConnectionStringBuilder CreateConnectionStringBuilder()
	{
		throw null;
	}

	public override DbDataAdapter CreateDataAdapter()
	{
		throw null;
	}

	public override DbParameter CreateParameter()
	{
		throw null;
	}
}
