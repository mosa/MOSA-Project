namespace System.Xml.Schema;

[Flags]
public enum XmlSchemaValidationFlags
{
	None = 0,
	ProcessInlineSchema = 1,
	ProcessSchemaLocation = 2,
	ReportValidationWarnings = 4,
	ProcessIdentityConstraints = 8,
	AllowXmlAttributes = 0x10
}
