using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.XPath;

namespace System.Xml;

[Obsolete("XmlDataDocument has been deprecated and is not supported.")]
public class XmlDataDocument : XmlDocument
{
	public DataSet DataSet
	{
		get
		{
			throw null;
		}
	}

	[RequiresUnreferencedCode("XmlDataDocument is used for serialization and deserialization. Members from serialized types may be trimmed if not referenced directly.")]
	public XmlDataDocument()
	{
	}

	[RequiresUnreferencedCode("XmlDataDocument is used for serialization and deserialization. Members from serialized types may be trimmed if not referenced directly.")]
	public XmlDataDocument(DataSet dataset)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw null;
	}

	public override XmlElement CreateElement(string? prefix, string localName, string? namespaceURI)
	{
		throw null;
	}

	public override XmlEntityReference CreateEntityReference(string name)
	{
		throw null;
	}

	protected internal override XPathNavigator? CreateNavigator(XmlNode node)
	{
		throw null;
	}

	public override XmlElement? GetElementById(string elemId)
	{
		throw null;
	}

	public XmlElement GetElementFromRow(DataRow r)
	{
		throw null;
	}

	public override XmlNodeList GetElementsByTagName(string name)
	{
		throw null;
	}

	public DataRow? GetRowFromElement(XmlElement? e)
	{
		throw null;
	}

	public override void Load(Stream inStream)
	{
	}

	public override void Load(TextReader txtReader)
	{
	}

	public override void Load(string filename)
	{
	}

	public override void Load(XmlReader reader)
	{
	}
}
