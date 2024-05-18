using System.Collections;

namespace System.Xml.XPath;

public abstract class XPathNodeIterator : IEnumerable, ICloneable
{
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public abstract XPathNavigator? Current { get; }

	public abstract int CurrentPosition { get; }

	public abstract XPathNodeIterator Clone();

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public abstract bool MoveNext();

	object ICloneable.Clone()
	{
		throw null;
	}
}
