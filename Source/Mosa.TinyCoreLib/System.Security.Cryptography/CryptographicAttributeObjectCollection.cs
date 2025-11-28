using System.Collections;

namespace System.Security.Cryptography;

public sealed class CryptographicAttributeObjectCollection : ICollection, IEnumerable
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

	public CryptographicAttributeObject this[int index]
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

	public CryptographicAttributeObjectCollection()
	{
	}

	public CryptographicAttributeObjectCollection(CryptographicAttributeObject attribute)
	{
	}

	public int Add(AsnEncodedData asnEncodedData)
	{
		throw null;
	}

	public int Add(CryptographicAttributeObject attribute)
	{
		throw null;
	}

	public void CopyTo(CryptographicAttributeObject[] array, int index)
	{
	}

	public CryptographicAttributeObjectEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(CryptographicAttributeObject attribute)
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
