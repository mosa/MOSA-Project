using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaContentModel : XmlSchemaAnnotated
{
	[XmlIgnore]
	public abstract XmlSchemaContent? Content { get; set; }
}
