using System.Xml.Linq;

namespace System.Xml.Schema;

public static class Extensions
{
	public static IXmlSchemaInfo? GetSchemaInfo(this XAttribute source)
	{
		throw null;
	}

	public static IXmlSchemaInfo? GetSchemaInfo(this XElement source)
	{
		throw null;
	}

	public static void Validate(this XAttribute source, XmlSchemaObject partialValidationType, XmlSchemaSet schemas, ValidationEventHandler? validationEventHandler)
	{
	}

	public static void Validate(this XAttribute source, XmlSchemaObject partialValidationType, XmlSchemaSet schemas, ValidationEventHandler? validationEventHandler, bool addSchemaInfo)
	{
	}

	public static void Validate(this XDocument source, XmlSchemaSet schemas, ValidationEventHandler? validationEventHandler)
	{
	}

	public static void Validate(this XDocument source, XmlSchemaSet schemas, ValidationEventHandler? validationEventHandler, bool addSchemaInfo)
	{
	}

	public static void Validate(this XElement source, XmlSchemaObject partialValidationType, XmlSchemaSet schemas, ValidationEventHandler? validationEventHandler)
	{
	}

	public static void Validate(this XElement source, XmlSchemaObject partialValidationType, XmlSchemaSet schemas, ValidationEventHandler? validationEventHandler, bool addSchemaInfo)
	{
	}
}
