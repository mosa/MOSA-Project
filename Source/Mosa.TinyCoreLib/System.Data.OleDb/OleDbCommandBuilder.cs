using System.ComponentModel;
using System.Data.Common;

namespace System.Data.OleDb;

public sealed class OleDbCommandBuilder : DbCommandBuilder
{
	[DefaultValue(null)]
	public new OleDbDataAdapter? DataAdapter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OleDbCommandBuilder()
	{
	}

	public OleDbCommandBuilder(OleDbDataAdapter? adapter)
	{
	}

	protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
	{
	}

	public static void DeriveParameters(OleDbCommand command)
	{
	}

	public new OleDbCommand GetDeleteCommand()
	{
		throw null;
	}

	public new OleDbCommand GetDeleteCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	public new OleDbCommand GetInsertCommand()
	{
		throw null;
	}

	public new OleDbCommand GetInsertCommand(bool useColumnsForParameterNames)
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

	public new OleDbCommand GetUpdateCommand()
	{
		throw null;
	}

	public new OleDbCommand GetUpdateCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	public override string QuoteIdentifier(string unquotedIdentifier)
	{
		throw null;
	}

	public string QuoteIdentifier(string unquotedIdentifier, OleDbConnection? connection)
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

	public string UnquoteIdentifier(string quotedIdentifier, OleDbConnection? connection)
	{
		throw null;
	}
}
