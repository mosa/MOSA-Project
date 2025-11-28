using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Xml.Serialization;

public class XmlReflectionImporter
{
	public XmlReflectionImporter()
	{
	}

	public XmlReflectionImporter(string? defaultNamespace)
	{
	}

	public XmlReflectionImporter(XmlAttributeOverrides? attributeOverrides)
	{
	}

	public XmlReflectionImporter(XmlAttributeOverrides? attributeOverrides, string? defaultNamespace)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement, bool rpc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement, bool rpc, bool openModel)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlMembersMapping ImportMembersMapping(string? elementName, string? ns, XmlReflectionMember[] members, bool hasWrapperElement, bool rpc, bool openModel, XmlMappingAccess access)
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
	public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute? root)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute? root, string? defaultNamespace)
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
