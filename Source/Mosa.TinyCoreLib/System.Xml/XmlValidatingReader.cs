using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml;

[Obsolete("XmlValidatingReader has been deprecated. Use XmlReader created by XmlReader.Create() method using appropriate XmlReaderSettings instead.")]
public class XmlValidatingReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
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

	public Encoding? Encoding
	{
		get
		{
			throw null;
		}
	}

	public EntityHandling EntityHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool EOF
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

	public int LineNumber
	{
		get
		{
			throw null;
		}
	}

	public int LinePosition
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

	public bool Namespaces
	{
		get
		{
			throw null;
		}
		set
		{
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

	public override char QuoteChar
	{
		get
		{
			throw null;
		}
	}

	public XmlReader Reader
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

	public XmlSchemaCollection Schemas
	{
		get
		{
			throw null;
		}
	}

	public object? SchemaType
	{
		get
		{
			throw null;
		}
	}

	public ValidationType ValidationType
	{
		get
		{
			throw null;
		}
		set
		{
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

	public XmlResolver XmlResolver
	{
		set
		{
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			throw null;
		}
	}

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
		}
		remove
		{
		}
	}

	public XmlValidatingReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
	{
	}

	public XmlValidatingReader([StringSyntax("Xml")] string xmlFragment, XmlNodeType fragType, XmlParserContext context)
	{
	}

	public XmlValidatingReader(XmlReader reader)
	{
	}

	public override void Close()
	{
	}

	public override string GetAttribute(int i)
	{
		throw null;
	}

	public override string? GetAttribute(string name)
	{
		throw null;
	}

	public override string? GetAttribute(string localName, string? namespaceURI)
	{
		throw null;
	}

	public bool HasLineInfo()
	{
		throw null;
	}

	public override string? LookupNamespace(string prefix)
	{
		throw null;
	}

	public override void MoveToAttribute(int i)
	{
	}

	public override bool MoveToAttribute(string name)
	{
		throw null;
	}

	public override bool MoveToAttribute(string localName, string? namespaceURI)
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

	public object? ReadTypedValue()
	{
		throw null;
	}

	public override void ResolveEntity()
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
