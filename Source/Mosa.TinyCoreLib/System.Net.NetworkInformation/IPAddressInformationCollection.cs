using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation;

public class IPAddressInformationCollection : ICollection<IPAddressInformation>, IEnumerable<IPAddressInformation>, IEnumerable
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

	public virtual IPAddressInformation this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal IPAddressInformationCollection()
	{
	}

	public virtual void Add(IPAddressInformation address)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(IPAddressInformation address)
	{
		throw null;
	}

	public virtual void CopyTo(IPAddressInformation[] array, int offset)
	{
	}

	public virtual IEnumerator<IPAddressInformation> GetEnumerator()
	{
		throw null;
	}

	public virtual bool Remove(IPAddressInformation address)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
