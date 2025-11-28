namespace System.Xml.Serialization;

[Flags]
public enum CodeGenerationOptions
{
	[XmlIgnore]
	None = 0,
	[XmlEnum("properties")]
	GenerateProperties = 1,
	[XmlEnum("newAsync")]
	GenerateNewAsync = 2,
	[XmlEnum("oldAsync")]
	GenerateOldAsync = 4,
	[XmlEnum("order")]
	GenerateOrder = 8,
	[XmlEnum("enableDataBinding")]
	EnableDataBinding = 0x10
}
