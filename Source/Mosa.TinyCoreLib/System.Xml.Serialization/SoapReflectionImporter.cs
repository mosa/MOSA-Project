using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Xml.Serialization;

public class SoapReflectionImporter
{
	public SoapReflectionImporter()
	{
	}

	public SoapReflectionImporter(string? defaultNamespace)
	{
	}

	public SoapReflectionImporter(SoapAttributeOverrides? attributeOverrides)
	{
	}

	public SoapReflectionImporter(SoapAttributeOverrides? attributeOverrides, string? defaultNamespace)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors, bool validate)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement, bool writeAccessors, bool validate, XmlMappingAccess access)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportTypeMapping(Type type)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportTypeMapping(Type type, string? defaultNamespace)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void IncludeType(Type type)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void IncludeTypes(ICustomAttributeProvider provider)
	{
	}
}
