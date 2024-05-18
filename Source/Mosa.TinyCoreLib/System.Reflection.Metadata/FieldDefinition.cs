namespace System.Reflection.Metadata;

public readonly struct FieldDefinition
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public FieldAttributes Attributes
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

	public TType DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext)
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}

	public TypeDefinitionHandle GetDeclaringType()
	{
		throw null;
	}

	public ConstantHandle GetDefaultValue()
	{
		throw null;
	}

	public BlobHandle GetMarshallingDescriptor()
	{
		throw null;
	}

	public int GetOffset()
	{
		throw null;
	}

	public int GetRelativeVirtualAddress()
	{
		throw null;
	}
}
