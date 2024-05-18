using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct StandaloneSignature
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public BlobHandle Signature
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<TType> DecodeLocalSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
	{
		throw null;
	}

	public MethodSignature<TType> DecodeMethodSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}

	public StandaloneSignatureKind GetKind()
	{
		throw null;
	}
}
