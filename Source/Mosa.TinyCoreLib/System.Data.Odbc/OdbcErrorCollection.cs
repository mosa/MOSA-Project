using System.Collections;

namespace System.Data.Odbc;

public sealed class OdbcErrorCollection : ICollection, IEnumerable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public OdbcError this[int i]
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	internal OdbcErrorCollection()
	{
	}

	public void CopyTo(Array array, int i)
	{
	}

	public void CopyTo(OdbcError[] array, int i)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}
}
