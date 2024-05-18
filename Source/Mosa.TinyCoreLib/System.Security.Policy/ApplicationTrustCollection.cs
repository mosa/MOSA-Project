using System.Collections;

namespace System.Security.Policy;

public sealed class ApplicationTrustCollection : ICollection, IEnumerable
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

	public ApplicationTrust this[int index]
	{
		get
		{
			throw null;
		}
	}

	public ApplicationTrust this[string appFullName]
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

	internal ApplicationTrustCollection()
	{
	}

	public int Add(ApplicationTrust trust)
	{
		throw null;
	}

	public void AddRange(ApplicationTrustCollection trusts)
	{
	}

	public void AddRange(ApplicationTrust[] trusts)
	{
	}

	public void Clear()
	{
	}

	public void CopyTo(ApplicationTrust[] array, int index)
	{
	}

	public ApplicationTrustCollection Find(ApplicationIdentity applicationIdentity, ApplicationVersionMatch versionMatch)
	{
		throw null;
	}

	public ApplicationTrustEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(ApplicationIdentity applicationIdentity, ApplicationVersionMatch versionMatch)
	{
	}

	public void Remove(ApplicationTrust trust)
	{
	}

	public void RemoveRange(ApplicationTrustCollection trusts)
	{
	}

	public void RemoveRange(ApplicationTrust[] trusts)
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
