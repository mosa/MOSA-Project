using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335;

public readonly struct SignatureDecoder<TType, TGenericContext>
{
	private readonly TGenericContext _genericContext;

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public SignatureDecoder(ISignatureTypeProvider<TType, TGenericContext> provider, MetadataReader metadataReader, TGenericContext genericContext)
	{
		throw null;
	}

	public TType DecodeFieldSignature(ref BlobReader blobReader)
	{
		throw null;
	}

	public ImmutableArray<TType> DecodeLocalSignature(ref BlobReader blobReader)
	{
		throw null;
	}

	public MethodSignature<TType> DecodeMethodSignature(ref BlobReader blobReader)
	{
		throw null;
	}

	public ImmutableArray<TType> DecodeMethodSpecificationSignature(ref BlobReader blobReader)
	{
		throw null;
	}

	public TType DecodeType(ref BlobReader blobReader, bool allowTypeSpecifications = false)
	{
		throw null;
	}
}
