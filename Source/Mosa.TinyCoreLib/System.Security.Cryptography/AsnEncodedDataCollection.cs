using System.Collections;

namespace System.Security.Cryptography;

public sealed class AsnEncodedDataCollection : ICollection, IEnumerable
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

	public AsnEncodedData this[int index]
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

	public AsnEncodedDataCollection()
	{
	}

	public AsnEncodedDataCollection(AsnEncodedData asnEncodedData)
	{
	}

	public int Add(AsnEncodedData asnEncodedData)
	{
		throw null;
	}

	public void CopyTo(AsnEncodedData[] array, int index)
	{
	}

	public AsnEncodedDataEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(AsnEncodedData asnEncodedData)
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
