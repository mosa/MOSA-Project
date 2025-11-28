using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAppInfo : XmlSchemaObject
{
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
