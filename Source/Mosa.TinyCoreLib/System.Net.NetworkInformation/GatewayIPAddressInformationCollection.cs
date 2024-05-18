using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation;

public class GatewayIPAddressInformationCollection : ICollection<GatewayIPAddressInformation>, IEnumerable<GatewayIPAddressInformation>, IEnumerable
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

	public virtual GatewayIPAddressInformation this[int index]
	{
		get
		{
			throw null;
		}
	}

	protected internal GatewayIPAddressInformationCollection()
	{
	}

	public virtual void Add(GatewayIPAddressInformation address)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(GatewayIPAddressInformation address)
	{
		throw null;
	}

	public virtual void CopyTo(GatewayIPAddressInformation[] array, int offset)
	{
	}

	public virtual IEnumerator<GatewayIPAddressInformation> GetEnumerator()
	{
		throw null;
	}

	public virtual bool Remove(GatewayIPAddressInformation address)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
