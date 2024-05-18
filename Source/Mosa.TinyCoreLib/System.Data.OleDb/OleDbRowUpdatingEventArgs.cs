using System.Data.Common;

namespace System.Data.OleDb;

public sealed class OleDbRowUpdatingEventArgs : RowUpdatingEventArgs
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

	public new OleDbCommand? Command
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OleDbRowUpdatingEventArgs(DataRow dataRow, IDbCommand? command, StatementType statementType, DataTableMapping tableMapping)
		: base(null, null, StatementType.Select, null)
	{
	}
}
