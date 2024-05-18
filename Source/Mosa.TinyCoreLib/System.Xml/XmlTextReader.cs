using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace System.Xml;

[EditorBrowsable(EditorBrowsableState.Never)]
public class XmlTextReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
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

	public override bool CanReadValueChunk
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

	public DtdProcessing DtdProcessing
	{
		get
		{
			throw null;
		}
		set
		{
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

	public bool Normalization
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string Prefix
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("XmlTextReader.ProhibitDtd has been deprecated. Use DtdProcessing instead.")]
	public bool ProhibitDtd
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override char QuoteChar
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

	public override string Value
	{
		get
		{
			throw null;
		}
	}

	public WhitespaceHandling WhitespaceHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string XmlLang
	{
		get
		{
			throw null;
		}
	}

	public XmlResolver? XmlResolver
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

	protected XmlTextReader()
	{
	}

	public XmlTextReader(Stream input)
	{
	}

	public XmlTextReader(Stream input, XmlNameTable nt)
	{
	}

	public XmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext? context)
	{
	}

	public XmlTextReader(TextReader input)
	{
	}

	public XmlTextReader(TextReader input, XmlNameTable nt)
	{
	}

	public XmlTextReader([StringSyntax("Uri")] string url)
	{
	}

	public XmlTextReader([StringSyntax("Uri")] string url, Stream input)
	{
	}

	public XmlTextReader([StringSyntax("Uri")] string url, Stream input, XmlNameTable nt)
	{
	}

	public XmlTextReader([StringSyntax("Uri")] string url, TextReader input)
	{
	}

	public XmlTextReader([StringSyntax("Uri")] string url, TextReader input, XmlNameTable nt)
	{
	}

	public XmlTextReader([StringSyntax("Uri")] string url, XmlNameTable nt)
	{
	}

	public XmlTextReader([StringSyntax("Xml")] string xmlFragment, XmlNodeType fragType, XmlParserContext? context)
	{
	}

	protected XmlTextReader(XmlNameTable nt)
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

	public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		throw null;
	}

	public TextReader GetRemainder()
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

	public int ReadBase64(byte[] array, int offset, int len)
	{
		throw null;
	}

	public int ReadBinHex(byte[] array, int offset, int len)
	{
		throw null;
	}

	public int ReadChars(char[] buffer, int index, int count)
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

	public void ResetState()
	{
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

	string? IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		throw null;
	}

	string? IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		throw null;
	}
}
