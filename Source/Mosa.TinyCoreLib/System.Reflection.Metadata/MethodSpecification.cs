using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct MethodSpecification
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public EntityHandle Method
	{
		get
		{
			throw null;
		}
	}

	public BlobHandle Signature
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<TType> DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}
}
