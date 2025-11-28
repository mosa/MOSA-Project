namespace System.Reflection.Metadata;

public interface ISZArrayTypeProvider<TType>
{
	TType GetSZArrayType(TType elementType);
}
