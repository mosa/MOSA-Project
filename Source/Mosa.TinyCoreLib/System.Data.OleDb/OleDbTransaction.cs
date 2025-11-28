using System.Data.Common;

namespace System.Data.OleDb;

public sealed class OleDbTransaction : DbTransaction
{
	public new OleDbConnection? Connection
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

	internal OleDbTransaction()
	{
	}

	public OleDbTransaction Begin()
	{
		throw null;
	}

	public OleDbTransaction Begin(IsolationLevel isolevel)
	{
		throw null;
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
