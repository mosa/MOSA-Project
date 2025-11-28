using System.Data.Common;

namespace System.Data.Odbc;

public sealed class OdbcRowUpdatingEventArgs : RowUpdatingEventArgs
{
	protected override IDbCommand? BaseCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public new OdbcCommand? Command
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OdbcRowUpdatingEventArgs(DataRow row, IDbCommand? command, StatementType statementType, DataTableMapping tableMapping)
		: base(null, null, StatementType.Select, null)
	{
	}
}
