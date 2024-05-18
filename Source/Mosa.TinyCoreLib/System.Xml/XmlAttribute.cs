using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml;

public class XmlAttribute : XmlNode
{
	public override string BaseURI
	{
		get
		{
			throw null;
		}
	}

	public override string InnerText
	{
		set
		{
		}
	}

	public override string InnerXml
	{
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

	public virtual XmlElement? OwnerElement
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

	public virtual bool Specified
	{
		get
		{
			throw null;
		}
	}

	public override string Value
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	protected internal XmlAttribute(string? prefix, string localName, string? namespaceURI, XmlDocument doc)
	{
	}

	public override XmlNode? AppendChild(XmlNode newChild)
	{
		throw null;
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw null;
	}

	public override XmlNode? InsertAfter(XmlNode newChild, XmlNode? refChild)
	{
		throw null;
	}

	public override XmlNode? InsertBefore(XmlNode newChild, XmlNode? refChild)
	{
		throw null;
	}

	public override XmlNode? PrependChild(XmlNode newChild)
	{
		throw null;
	}

	public override XmlNode RemoveChild(XmlNode oldChild)
	{
		throw null;
	}

	public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
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
