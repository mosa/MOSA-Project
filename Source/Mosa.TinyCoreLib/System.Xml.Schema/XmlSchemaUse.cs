using System.Xml.Serialization;

namespace System.Xml.Schema;

public enum XmlSchemaUse
{
	[XmlIgnore]
	None,
	[XmlEnum("optional")]
	Optional,
	[XmlEnum("prohibited")]
	Prohibited,
	[XmlEnum("required")]
	Required
}
