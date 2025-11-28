using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaImport : XmlSchemaExternal
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

	[XmlAttribute("namespace", DataType = "anyURI")]
	public string? Namespace
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
