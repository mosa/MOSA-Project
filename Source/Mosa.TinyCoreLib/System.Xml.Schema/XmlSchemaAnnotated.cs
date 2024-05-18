using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAnnotated : XmlSchemaObject
{
	[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
	public XmlSchemaAnnotation? Annotation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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
