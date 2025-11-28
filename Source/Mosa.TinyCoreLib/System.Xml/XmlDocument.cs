using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml;

public class XmlDocument : XmlNode
{
	public override string BaseURI
	{
		get
		{
			throw null;
		}
	}

	public XmlElement? DocumentElement
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlDocumentType? DocumentType
	{
		get
		{
			throw null;
		}
	}

	public XmlImplementation Implementation
	{
		get
		{
			throw null;
		}
	}

	public override string InnerText
	{
		[param: AllowNull]
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

	public override bool IsReadOnly
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

	public XmlNameTable NameTable
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

	public override XmlDocument? OwnerDocument
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

	public bool PreserveWhitespace
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

	public XmlSchemaSet Schemas
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual XmlResolver? XmlResolver
	{
		set
		{
		}
	}

	public event XmlNodeChangedEventHandler NodeChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlNodeChangedEventHandler NodeChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlNodeChangedEventHandler NodeInserted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlNodeChangedEventHandler NodeInserting
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlNodeChangedEventHandler NodeRemoved
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlNodeChangedEventHandler NodeRemoving
	{
		add
		{
		}
		remove
		{
		}
	}

	public XmlDocument()
	{
	}

	protected internal XmlDocument(XmlImplementation imp)
	{
	}

	public XmlDocument(XmlNameTable nt)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw null;
	}

	public XmlAttribute CreateAttribute(string name)
	{
		throw null;
	}

	public XmlAttribute CreateAttribute(string qualifiedName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlAttribute CreateAttribute(string? prefix, string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlCDataSection CreateCDataSection(string? data)
	{
		throw null;
	}

	public virtual XmlComment CreateComment(string? data)
	{
		throw null;
	}

	protected internal virtual XmlAttribute CreateDefaultAttribute(string? prefix, string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlDocumentFragment CreateDocumentFragment()
	{
		throw null;
	}

	public virtual XmlDocumentType CreateDocumentType(string name, string? publicId, string? systemId, string? internalSubset)
	{
		throw null;
	}

	public XmlElement CreateElement(string name)
	{
		throw null;
	}

	public XmlElement CreateElement(string qualifiedName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlElement CreateElement(string? prefix, string localName, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlEntityReference CreateEntityReference(string name)
	{
		throw null;
	}

	public override XPathNavigator? CreateNavigator()
	{
		throw null;
	}

	protected internal virtual XPathNavigator? CreateNavigator(XmlNode node)
	{
		throw null;
	}

	public virtual XmlNode CreateNode(string nodeTypeString, string name, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlNode CreateNode(XmlNodeType type, string name, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlNode CreateNode(XmlNodeType type, string? prefix, string name, string? namespaceURI)
	{
		throw null;
	}

	public virtual XmlProcessingInstruction CreateProcessingInstruction(string target, string? data)
	{
		throw null;
	}

	public virtual XmlSignificantWhitespace CreateSignificantWhitespace(string? text)
	{
		throw null;
	}

	public virtual XmlText CreateTextNode(string? text)
	{
		throw null;
	}

	public virtual XmlWhitespace CreateWhitespace(string? text)
	{
		throw null;
	}

	public virtual XmlDeclaration CreateXmlDeclaration(string version, string? encoding, string? standalone)
	{
		throw null;
	}

	public virtual XmlElement? GetElementById(string elementId)
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

	public virtual XmlNode ImportNode(XmlNode node, bool deep)
	{
		throw null;
	}

	public virtual void Load(Stream inStream)
	{
	}

	public virtual void Load(TextReader txtReader)
	{
	}

	public virtual void Load(string filename)
	{
	}

	public virtual void Load(XmlReader reader)
	{
	}

	public virtual void LoadXml([StringSyntax("Xml")] string xml)
	{
	}

	public virtual XmlNode? ReadNode(XmlReader reader)
	{
		throw null;
	}

	public virtual void Save(Stream outStream)
	{
	}

	public virtual void Save(TextWriter writer)
	{
	}

	public virtual void Save(string filename)
	{
	}

	public virtual void Save(XmlWriter w)
	{
	}

	public void Validate(ValidationEventHandler? validationEventHandler)
	{
	}

	public void Validate(ValidationEventHandler? validationEventHandler, XmlNode nodeToValidate)
	{
	}

	public override void WriteContentTo(XmlWriter xw)
	{
	}

	public override void WriteTo(XmlWriter w)
	{
	}
}
