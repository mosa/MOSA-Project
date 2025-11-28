using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Xml;

public abstract class XmlNodeList : IEnumerable, IDisposable
{
	public abstract int Count { get; }

	[IndexerName("ItemOf")]
	public virtual XmlNode? this[int i]
	{
		get
		{
			throw null;
		}
	}

	public abstract IEnumerator GetEnumerator();

	public abstract XmlNode? Item(int index);

	protected virtual void PrivateDisposeNodeList()
	{
	}

	void IDisposable.Dispose()
	{
	}
}
