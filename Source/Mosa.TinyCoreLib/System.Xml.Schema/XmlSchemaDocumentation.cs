using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaDocumentation : XmlSchemaObject
{
	[XmlAttribute("xml:lang")]
	public string? Language
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	[XmlAnyElement]
	[XmlText]
	public XmlNode?[]? Markup
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlAttribute("source", DataType = "anyURI")]
	public string? Source
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
