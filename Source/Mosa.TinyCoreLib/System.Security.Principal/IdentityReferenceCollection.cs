using System.Collections;
using System.Collections.Generic;

namespace System.Security.Principal;

public class IdentityReferenceCollection : ICollection<IdentityReference>, IEnumerable<IdentityReference>, IEnumerable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public IdentityReference this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	bool ICollection<IdentityReference>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public IdentityReferenceCollection()
	{
	}

	public IdentityReferenceCollection(int capacity)
	{
	}

	public void Add(IdentityReference identity)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(IdentityReference identity)
	{
		throw null;
	}

	public void CopyTo(IdentityReference[] array, int offset)
	{
	}

	public IEnumerator<IdentityReference> GetEnumerator()
	{
		throw null;
	}

	public bool Remove(IdentityReference identity)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public IdentityReferenceCollection Translate(Type targetType)
	{
		throw null;
	}

	public IdentityReferenceCollection Translate(Type targetType, bool forceSuccess)
	{
		throw null;
	}
}
