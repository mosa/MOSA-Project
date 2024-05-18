using System.Collections;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

public abstract class XmlNode : IEnumerable, ICloneable, IXPathNavigable
{
	public virtual XmlAttributeCollection? Attributes
	{
		get
		{
			throw null;
		}
	}

	public virtual string BaseURI
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNodeList ChildNodes
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNode? FirstChild
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasChildNodes
	{
		get
		{
			throw null;
		}
	}

	public virtual string InnerText
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string InnerXml
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlElement? this[string name]
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlElement? this[string localname, string ns]
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNode? LastChild
	{
		get
		{
			throw null;
		}
	}

	public abstract string LocalName { get; }

	public abstract string Name { get; }

	public virtual string NamespaceURI
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNode? NextSibling
	{
		get
		{
			throw null;
		}
	}

	public abstract XmlNodeType NodeType { get; }

	public virtual string OuterXml
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlDocument? OwnerDocument
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNode? ParentNode
	{
		get
		{
			throw null;
		}
	}

	public virtual string Prefix
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual XmlNode? PreviousSibling
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNode? PreviousText
	{
		get
		{
			throw null;
		}
	}

	public virtual IXmlSchemaInfo SchemaInfo
	{
		get
		{
			throw null;
		}
	}

	public virtual string? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal XmlNode()
	{
	}

	public virtual XmlNode? AppendChild(XmlNode newChild)
	{
		throw null;
	}

	public virtual XmlNode Clone()
	{
		throw null;
	}

	public abstract XmlNode CloneNode(bool deep);

	public virtual XPathNavigator? CreateNavigator()
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual string GetNamespaceOfPrefix(string prefix)
	{
		throw null;
	}

	public virtual string GetPrefixOfNamespace(string namespaceURI)
	{
		throw null;
	}

	public virtual XmlNode? InsertAfter(XmlNode newChild, XmlNode? refChild)
	{
		throw null;
	}

	public virtual XmlNode? InsertBefore(XmlNode newChild, XmlNode? refChild)
	{
		throw null;
	}

	public virtual void Normalize()
	{
	}

	public virtual XmlNode? PrependChild(XmlNode newChild)
	{
		throw null;
	}

	public virtual void RemoveAll()
	{
	}

	public virtual XmlNode RemoveChild(XmlNode oldChild)
	{
		throw null;
	}

	public virtual XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
	{
		throw null;
	}

	public XmlNodeList? SelectNodes(string xpath)
	{
		throw null;
	}

	public XmlNodeList? SelectNodes(string xpath, XmlNamespaceManager nsmgr)
	{
		throw null;
	}

	public XmlNode? SelectSingleNode(string xpath)
	{
		throw null;
	}

	public XmlNode? SelectSingleNode(string xpath, XmlNamespaceManager nsmgr)
	{
		throw null;
	}

	public virtual bool Supports(string feature, string version)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public abstract void WriteContentTo(XmlWriter w);

	public abstract void WriteTo(XmlWriter w);
}
