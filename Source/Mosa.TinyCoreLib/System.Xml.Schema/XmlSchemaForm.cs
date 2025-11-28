using System.Xml.Serialization;

namespace System.Xml.Schema;

public enum XmlSchemaForm
{
	[XmlIgnore]
	None,
	[XmlEnum("qualified")]
	Qualified,
	[XmlEnum("unqualified")]
	Unqualified
}
