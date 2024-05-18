using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaInclude : XmlSchemaExternal
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
}
