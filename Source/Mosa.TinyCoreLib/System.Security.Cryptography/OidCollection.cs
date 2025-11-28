using System.Collections;

namespace System.Security.Cryptography;

public sealed class OidCollection : ICollection, IEnumerable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public Oid this[int index]
	{
		get
		{
			throw null;
		}
	}

	public Oid? this[string oid]
	{
		get
		{
			throw null;
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public int Add(Oid oid)
	{
		throw null;
	}

	public void CopyTo(Oid[] array, int index)
	{
	}

	public OidEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
