using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509ExtensionCollection : IEnumerable<X509Extension>, IEnumerable, ICollection
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

	public X509Extension this[int index]
	{
		get
		{
			throw null;
		}
	}

	public X509Extension? this[string oid]
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

	public int Add(X509Extension extension)
	{
		throw null;
	}

	public void CopyTo(X509Extension[] array, int index)
	{
	}

	public X509ExtensionEnumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator<X509Extension> IEnumerable<X509Extension>.GetEnumerator()
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
