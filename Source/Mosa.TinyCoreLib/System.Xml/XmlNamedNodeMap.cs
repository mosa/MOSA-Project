using System.Collections;

namespace System.Xml;

public class XmlNamedNodeMap : IEnumerable
{
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	internal XmlNamedNodeMap()
	{
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual XmlNode? GetNamedItem(string name)
	{
		throw null;
	}

	public virtual XmlNode? GetNamedItem(string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlNode? Item(int index)
	{
		throw null;
	}

	public virtual XmlNode? RemoveNamedItem(string name)
	{
		throw null;
	}

	public virtual XmlNode? RemoveNamedItem(string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlNode? SetNamedItem(XmlNode? node)
	{
		throw null;
	}
}
