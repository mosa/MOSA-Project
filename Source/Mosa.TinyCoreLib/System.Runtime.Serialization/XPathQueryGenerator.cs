using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

public static class XPathQueryGenerator
{
	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, StringBuilder? rootElementXpath, out XmlNamespaceManager namespaces)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, out XmlNamespaceManager namespaces)
	{
		throw null;
	}
}
