using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml;

public class XmlElement : XmlLinkedNode
{
	public override XmlAttributeCollection Attributes
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasAttributes
	{
		get
		{
			throw null;
		}
	}

	public override string InnerText
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string InnerXml
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string LocalName
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			throw null;
		}
	}

	public override XmlNode? NextSibling
	{
		get
		{
			throw null;
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public override XmlDocument OwnerDocument
	{
		get
		{
			throw null;
		}
	}

	public override XmlNode? ParentNode
	{
		get
		{
			throw null;
		}
	}

	public override string Prefix
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override IXmlSchemaInfo SchemaInfo
	{
		get
		{
			throw null;
		}
	}

	protected internal XmlElement(string? prefix, string localName, string? namespaceURI, XmlDocument doc)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw null;
	}

	public virtual string GetAttribute(string name)
	{
		throw null;
	}

	public virtual string GetAttribute(string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlAttribute? GetAttributeNode(string name)
	{
		throw null;
	}

	public virtual XmlAttribute? GetAttributeNode(string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlNodeList GetElementsByTagName(string name)
	{
		throw null;
	}

	public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool HasAttribute(string name)
	{
		throw null;
	}

	public virtual bool HasAttribute(string localName, string? namespaceURI)
	{
		throw null;
	}

	public override void RemoveAll()
	{
	}

	public virtual void RemoveAllAttributes()
	{
	}

	public virtual void RemoveAttribute(string name)
	{
	}

	public virtual void RemoveAttribute(string localName, string? namespaceURI)
	{
	}

	public virtual XmlNode? RemoveAttributeAt(int i)
	{
		throw null;
	}

	public virtual XmlAttribute? RemoveAttributeNode(string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlAttribute? RemoveAttributeNode(XmlAttribute oldAttr)
	{
		throw null;
	}

	public virtual void SetAttribute(string name, string? value)
	{
	}

	[return: NotNullIfNotNull("value")]
	public virtual string? SetAttribute(string localName, string? namespaceURI, string? value)
	{
		throw null;
	}

	public virtual XmlAttribute SetAttributeNode(string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlAttribute? SetAttributeNode(XmlAttribute newAttr)
	{
		throw null;
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}

	public override void WriteTo(XmlWriter w)
	{
	}
}
