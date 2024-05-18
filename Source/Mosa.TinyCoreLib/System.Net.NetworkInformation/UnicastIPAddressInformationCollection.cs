using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation;

public class UnicastIPAddressInformationCollection : ICollection<UnicastIPAddressInformation>, IEnumerable<UnicastIPAddressInformation>, IEnumerable
{
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual UnicastIPAddressInformation this[int index]
	{
		get
		{
			throw null;
		}
	}

	protected internal UnicastIPAddressInformationCollection()
	{
	}

	public virtual void Add(UnicastIPAddressInformation address)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(UnicastIPAddressInformation address)
	{
		throw null;
	}

	public virtual void CopyTo(UnicastIPAddressInformation[] array, int offset)
	{
	}

	public virtual IEnumerator<UnicastIPAddressInformation> GetEnumerator()
	{
		throw null;
	}

	public virtual bool Remove(UnicastIPAddressInformation address)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
