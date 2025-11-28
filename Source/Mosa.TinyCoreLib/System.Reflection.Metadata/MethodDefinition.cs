namespace System.Reflection.Metadata;

public readonly struct MethodDefinition
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public MethodAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public MethodImplAttributes ImplAttributes
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

	public int RelativeVirtualAddress
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

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}

	public DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes()
	{
		throw null;
	}

	public TypeDefinitionHandle GetDeclaringType()
	{
		throw null;
	}

	public GenericParameterHandleCollection GetGenericParameters()
	{
		throw null;
	}

	public MethodImport GetImport()
	{
		throw null;
	}

	public ParameterHandleCollection GetParameters()
	{
		throw null;
	}
}
