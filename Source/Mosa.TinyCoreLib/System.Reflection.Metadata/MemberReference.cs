namespace System.Reflection.Metadata;

public readonly struct MemberReference
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public StringHandle Name
	{
		get
		{
			throw null;
		}
	}

	public EntityHandle Parent
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

	public TType DecodeFieldSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
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

	public MemberReferenceKind GetKind()
	{
		throw null;
	}
}
