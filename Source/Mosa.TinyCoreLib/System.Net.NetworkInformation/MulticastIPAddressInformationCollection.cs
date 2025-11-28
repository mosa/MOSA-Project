using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation;

public class MulticastIPAddressInformationCollection : ICollection<MulticastIPAddressInformation>, IEnumerable<MulticastIPAddressInformation>, IEnumerable
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

	public virtual MulticastIPAddressInformation this[int index]
	{
		get
		{
			throw null;
		}
	}

	protected internal MulticastIPAddressInformationCollection()
	{
	}

	public virtual void Add(MulticastIPAddressInformation address)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(MulticastIPAddressInformation address)
	{
		throw null;
	}

	public virtual void CopyTo(MulticastIPAddressInformation[] array, int offset)
	{
	}

	public virtual IEnumerator<MulticastIPAddressInformation> GetEnumerator()
	{
		throw null;
	}

	public virtual bool Remove(MulticastIPAddressInformation address)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
