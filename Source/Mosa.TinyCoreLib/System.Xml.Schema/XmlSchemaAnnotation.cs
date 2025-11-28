using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAnnotation : XmlSchemaObject
{
	[XmlAttribute("id", DataType = "ID")]
	public string? Id
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[XmlElement("appinfo", typeof(XmlSchemaAppInfo))]
	[XmlElement("documentation", typeof(XmlSchemaDocumentation))]
	public XmlSchemaObjectCollection Items
	{
		get
		{
			throw null;
		}
	}

	[XmlAnyAttribute]
	public XmlAttribute[]? UnhandledAttributes
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
