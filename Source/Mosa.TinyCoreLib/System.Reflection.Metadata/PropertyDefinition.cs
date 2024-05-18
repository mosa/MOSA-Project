namespace System.Reflection.Metadata;

public readonly struct PropertyDefinition
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public PropertyAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public StringHandle Name
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

	public MethodSignature<TType> DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
	{
		throw null;
	}

	public PropertyAccessors GetAccessors()
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}

	public ConstantHandle GetDefaultValue()
	{
		throw null;
	}
}
