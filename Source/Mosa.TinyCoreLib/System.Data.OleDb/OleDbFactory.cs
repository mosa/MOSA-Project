using System.Data.Common;

namespace System.Data.OleDb;

public sealed class OleDbFactory : DbProviderFactory
{
	public static readonly OleDbFactory Instance;

	internal OleDbFactory()
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
