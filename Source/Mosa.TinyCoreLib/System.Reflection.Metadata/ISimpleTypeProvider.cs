namespace System.Reflection.Metadata;

public interface ISimpleTypeProvider<TType>
{
	TType GetPrimitiveType(PrimitiveTypeCode typeCode);

	TType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind);

	TType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind);
}
