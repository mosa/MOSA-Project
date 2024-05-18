namespace System.Reflection.Metadata;

public readonly struct TypeSpecification
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

	public TType DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}
}
