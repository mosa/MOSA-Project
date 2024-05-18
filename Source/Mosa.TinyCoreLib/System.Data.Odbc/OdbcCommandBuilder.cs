using System.Data.Common;

namespace System.Data.Odbc;

public sealed class OdbcCommandBuilder : DbCommandBuilder
{
	public new OdbcDataAdapter? DataAdapter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OdbcCommandBuilder()
	{
	}

	public OdbcCommandBuilder(OdbcDataAdapter? adapter)
	{
	}

	protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
	{
	}

	public static void DeriveParameters(OdbcCommand command)
	{
	}

	public new OdbcCommand GetDeleteCommand()
	{
		throw null;
	}

	public new OdbcCommand GetDeleteCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	public new OdbcCommand GetInsertCommand()
	{
		throw null;
	}

	public new OdbcCommand GetInsertCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	protected override string GetParameterName(int parameterOrdinal)
	{
		throw null;
	}

	protected override string GetParameterName(string parameterName)
	{
		throw null;
	}

	protected override string GetParameterPlaceholder(int parameterOrdinal)
	{
		throw null;
	}

	public new OdbcCommand GetUpdateCommand()
	{
		throw null;
	}

	public new OdbcCommand GetUpdateCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	public override string QuoteIdentifier(string unquotedIdentifier)
	{
		throw null;
	}

	public string QuoteIdentifier(string unquotedIdentifier, OdbcConnection? connection)
	{
		throw null;
	}

	protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
	{
	}

	public override string UnquoteIdentifier(string quotedIdentifier)
	{
		throw null;
	}

	public string UnquoteIdentifier(string quotedIdentifier, OdbcConnection? connection)
	{
		throw null;
	}
}
