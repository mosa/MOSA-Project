namespace System.Reflection.Metadata;

public interface ICustomAttributeTypeProvider<TType> : ISimpleTypeProvider<TType>, ISZArrayTypeProvider<TType>
{
	TType GetSystemType();

	TType GetTypeFromSerializedName(string name);

	PrimitiveTypeCode GetUnderlyingEnumType(TType type);

	bool IsSystemType(TType type);
}
