using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509ChainElementCollection : IEnumerable<X509ChainElement>, IEnumerable, ICollection
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

	public X509ChainElement this[int index]
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

	internal X509ChainElementCollection()
	{
	}

	public void CopyTo(X509ChainElement[] array, int index)
	{
	}

	public X509ChainElementEnumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator<X509ChainElement> IEnumerable<X509ChainElement>.GetEnumerator()
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
