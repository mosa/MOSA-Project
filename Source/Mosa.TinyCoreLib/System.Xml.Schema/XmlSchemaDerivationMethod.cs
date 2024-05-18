using System.Xml.Serialization;

namespace System.Xml.Schema;

[Flags]
public enum XmlSchemaDerivationMethod
{
	[XmlEnum("")]
	Empty = 0,
	[XmlEnum("substitution")]
	Substitution = 1,
	[XmlEnum("extension")]
	Extension = 2,
	[XmlEnum("restriction")]
	Restriction = 4,
	[XmlEnum("list")]
	List = 8,
	[XmlEnum("union")]
	Union = 0x10,
	[XmlEnum("#all")]
	All = 0xFF,
	[XmlIgnore]
	None = 0x100
}
