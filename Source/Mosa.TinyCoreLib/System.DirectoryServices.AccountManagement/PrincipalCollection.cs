using System.Collections;
using System.Collections.Generic;

namespace System.DirectoryServices.AccountManagement;

public class PrincipalCollection : ICollection<Principal>, IEnumerable<Principal>, IEnumerable, ICollection
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
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

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
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

	internal PrincipalCollection()
	{
	}

	public void Add(ComputerPrincipal computer)
	{
	}

	public void Add(GroupPrincipal group)
	{
	}

	public void Add(Principal principal)
	{
	}

	public void Add(PrincipalContext context, IdentityType identityType, string identityValue)
	{
	}

	public void Add(UserPrincipal user)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(ComputerPrincipal computer)
	{
		throw null;
	}

	public bool Contains(GroupPrincipal group)
	{
		throw null;
	}

	public bool Contains(Principal principal)
	{
		throw null;
	}

	public bool Contains(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public bool Contains(UserPrincipal user)
	{
		throw null;
	}

	public void CopyTo(Principal[] array, int index)
	{
	}

	public IEnumerator<Principal> GetEnumerator()
	{
		throw null;
	}

	public bool Remove(ComputerPrincipal computer)
	{
		throw null;
	}

	public bool Remove(GroupPrincipal group)
	{
		throw null;
	}

	public bool Remove(Principal principal)
	{
		throw null;
	}

	public bool Remove(PrincipalContext context, IdentityType identityType, string identityValue)
	{
		throw null;
	}

	public bool Remove(UserPrincipal user)
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
