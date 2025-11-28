using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public interface IConstructedTypeProvider<TType> : ISZArrayTypeProvider<TType>
{
	TType GetArrayType(TType elementType, ArrayShape shape);

	TType GetByReferenceType(TType elementType);

	TType GetGenericInstantiation(TType genericType, ImmutableArray<TType> typeArguments);

	TType GetPointerType(TType elementType);
}
