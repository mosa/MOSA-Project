using System.Collections;
using System.ComponentModel;

namespace System.Data.Common;

public class DbEnumerator : IEnumerator
{
	public object Current
	{
		get
		{
			throw null;
		}
	}

	public DbEnumerator(DbDataReader reader)
	{
	}

	public DbEnumerator(DbDataReader reader, bool closeReader)
	{
	}

	public DbEnumerator(IDataReader reader)
	{
	}

	public DbEnumerator(IDataReader reader, bool closeReader)
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Reset()
	{
	}
}
