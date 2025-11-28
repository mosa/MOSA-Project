namespace System.Xml.Schema;

public interface IXmlSchemaInfo
{
	bool IsDefault { get; }

	bool IsNil { get; }

	XmlSchemaSimpleType? MemberType { get; }

	XmlSchemaAttribute? SchemaAttribute { get; }

	XmlSchemaElement? SchemaElement { get; }

	XmlSchemaType? SchemaType { get; }

	XmlSchemaValidity Validity { get; }
}
