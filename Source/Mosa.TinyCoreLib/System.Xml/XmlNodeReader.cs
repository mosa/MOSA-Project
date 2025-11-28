using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml;

public class XmlNodeReader : XmlReader, IXmlNamespaceResolver
{
	public override int AttributeCount
	{
		get
		{
			throw null;
		}
	}

	public override string BaseURI
	{
		get
		{
			throw null;
		}
	}

	public override bool CanReadBinaryContent
	{
		get
		{
			throw null;
		}
	}

	public override bool CanResolveEntity
	{
		get
		{
			throw null;
		}
	}

	public override int Depth
	{
		get
		{
			throw null;
		}
	}

	public override bool EOF
	{
		get
		{
			throw null;
		}
	}

	public override bool HasAttributes
	{
		get
		{
			throw null;
		}
	}

	public override bool HasValue
	{
		get
		{
			throw null;
		}
	}

	public override bool IsDefault
	{
		get
		{
			throw null;
		}
	}

	public override bool IsEmptyElement
	{
		get
		{
			throw null;
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

	public override XmlNameTable NameTable
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

	public override string Prefix
	{
		get
		{
			throw null;
		}
	}

	public override ReadState ReadState
	{
		get
		{
			throw null;
		}
	}

	public override IXmlSchemaInfo? SchemaInfo
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
	}

	public override string XmlLang
	{
		get
		{
			throw null;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			throw null;
		}
	}

	public XmlNodeReader(XmlNode node)
	{
	}

	public override void Close()
	{
	}

	public override string GetAttribute(int attributeIndex)
	{
		throw null;
	}

	public override string? GetAttribute(string name)
	{
		throw null;
	}

	public override string? GetAttribute(string name, string? namespaceURI)
	{
		throw null;
	}

	public override string? LookupNamespace(string prefix)
	{
		throw null;
	}

	public override void MoveToAttribute(int attributeIndex)
	{
	}

	public override bool MoveToAttribute(string name)
	{
		throw null;
	}

	public override bool MoveToAttribute(string name, string? namespaceURI)
	{
		throw null;
	}

	public override bool MoveToElement()
	{
		throw null;
	}

	public override bool MoveToFirstAttribute()
	{
		throw null;
	}

	public override bool MoveToNextAttribute()
	{
		throw null;
	}

	public override bool Read()
	{
		throw null;
	}

	public override bool ReadAttributeValue()
	{
		throw null;
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public override string ReadString()
	{
		throw null;
	}

	public override void ResolveEntity()
	{
	}

	public override void Skip()
	{
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		throw null;
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		throw null;
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		throw null;
	}
}
