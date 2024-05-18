using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Xml;

public sealed class XmlAttributeCollection : XmlNamedNodeMap, ICollection, IEnumerable
{
	[IndexerName("ItemOf")]
	public XmlAttribute this[int i]
	{
		get
		{
			throw null;
		}
	}

	[IndexerName("ItemOf")]
	public XmlAttribute? this[string name]
	{
		get
		{
			throw null;
		}
	}

	[IndexerName("ItemOf")]
	public XmlAttribute? this[string localName, string? namespaceURI]
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

	internal XmlAttributeCollection()
	{
	}

	public XmlAttribute Append(XmlAttribute node)
	{
		throw null;
	}

	public void CopyTo(XmlAttribute[] array, int index)
	{
	}

	public XmlAttribute InsertAfter(XmlAttribute newNode, XmlAttribute? refNode)
	{
		throw null;
	}

	public XmlAttribute InsertBefore(XmlAttribute newNode, XmlAttribute? refNode)
	{
		throw null;
	}

	public XmlAttribute Prepend(XmlAttribute node)
	{
		throw null;
	}

	public XmlAttribute? Remove(XmlAttribute? node)
	{
		throw null;
	}

	public void RemoveAll()
	{
	}

	public XmlAttribute? RemoveAt(int i)
	{
		throw null;
	}

	[return: NotNullIfNotNull("node")]
	public override XmlNode? SetNamedItem(XmlNode? node)
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
