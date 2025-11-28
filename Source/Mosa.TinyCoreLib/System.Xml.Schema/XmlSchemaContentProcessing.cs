using System.Xml.Serialization;

namespace System.Xml.Schema;

public enum XmlSchemaContentProcessing
{
	[XmlIgnore]
	None,
	[XmlEnum("skip")]
	Skip,
	[XmlEnum("lax")]
	Lax,
	[XmlEnum("strict")]
	Strict
}
