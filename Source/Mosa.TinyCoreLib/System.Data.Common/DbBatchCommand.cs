namespace System.Data.Common;

public abstract class DbBatchCommand
{
	public abstract string CommandText { get; set; }

	public abstract CommandType CommandType { get; set; }

	public abstract int RecordsAffected { get; }

	public DbParameterCollection Parameters
	{
		get
		{
			throw null;
		}
	}

	protected abstract DbParameterCollection DbParameterCollection { get; }

	public virtual bool CanCreateParameter
	{
		get
		{
			throw null;
		}
	}

	public virtual DbParameter CreateParameter()
	{
		throw null;
	}
}
