using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class CmsRecipientCollection : ICollection, IEnumerable
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

	public CmsRecipient this[int index]
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

	public CmsRecipientCollection()
	{
	}

	public CmsRecipientCollection(CmsRecipient recipient)
	{
	}

	public CmsRecipientCollection(SubjectIdentifierType recipientIdentifierType, X509Certificate2Collection certificates)
	{
	}

	public int Add(CmsRecipient recipient)
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(CmsRecipient[] array, int index)
	{
	}

	public CmsRecipientEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(CmsRecipient recipient)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
