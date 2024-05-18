using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Runtime.Serialization;

public abstract class DataContractResolver
{
	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public abstract Type? ResolveName(string typeName, string? typeNamespace, Type? declaredType, DataContractResolver knownTypeResolver);

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public abstract bool TryResolveType(Type type, Type? declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString? typeName, out XmlDictionaryString? typeNamespace);
}
