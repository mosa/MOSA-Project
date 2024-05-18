using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation;

public class IPAddressCollection : ICollection<IPAddress>, IEnumerable<IPAddress>, IEnumerable
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

	public virtual IPAddress this[int index]
	{
		get
		{
			throw null;
		}
	}

	protected internal IPAddressCollection()
	{
	}

	public virtual void Add(IPAddress address)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(IPAddress address)
	{
		throw null;
	}

	public virtual void CopyTo(IPAddress[] array, int offset)
	{
	}

	public virtual IEnumerator<IPAddress> GetEnumerator()
	{
		throw null;
	}

	public virtual bool Remove(IPAddress address)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
