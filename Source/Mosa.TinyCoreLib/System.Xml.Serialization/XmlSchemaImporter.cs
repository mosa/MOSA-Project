using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

public class XmlSchemaImporter : SchemaImporter
{
	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSchemaImporter(XmlSchemas schemas)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSchemaImporter(XmlSchemas schemas, CodeIdentifiers? typeIdentifiers)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping? ImportAnyType(XmlQualifiedName typeName, string elementName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportDerivedTypeMapping(XmlQualifiedName name, Type? baseType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportDerivedTypeMapping(XmlQualifiedName name, Type? baseType, bool baseTypeCanBeIndirect)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string name, string? ns, SoapSchemaMember[] members)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(XmlQualifiedName name)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(XmlQualifiedName[] names)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(XmlQualifiedName[] names, Type? baseType, bool baseTypeCanBeIndirect)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName, Type? baseType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName, Type? baseType, bool baseTypeCanBeIndirect)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportTypeMapping(XmlQualifiedName name)
	{
		throw null;
	}
}
