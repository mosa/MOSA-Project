using System.Data.Common;

namespace System.Data.Odbc;

public sealed class OdbcTransaction : DbTransaction
{
	public new OdbcConnection? Connection
	{
		get
		{
			throw null;
		}
	}

	protected override DbConnection? DbConnection
	{
		get
		{
			throw null;
		}
	}

	public override IsolationLevel IsolationLevel
	{
		get
		{
			throw null;
		}
	}

	internal OdbcTransaction()
	{
	}

	public override void Commit()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void Rollback()
	{
	}
}
