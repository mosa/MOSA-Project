namespace System.Reflection.Metadata.Ecma335;

public readonly struct BlobEncoder
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public BlobBuilder Builder
	{
		get
		{
			throw null;
		}
	}

	public BlobEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public void CustomAttributeSignature(Action<FixedArgumentsEncoder> fixedArguments, Action<CustomAttributeNamedArgumentsEncoder> namedArguments)
	{
	}

	public void CustomAttributeSignature(out FixedArgumentsEncoder fixedArguments, out CustomAttributeNamedArgumentsEncoder namedArguments)
	{
		throw null;
	}

	public FieldTypeEncoder Field()
	{
		throw null;
	}

	public SignatureTypeEncoder FieldSignature()
	{
		throw null;
	}

	public LocalVariablesEncoder LocalVariableSignature(int variableCount)
	{
		throw null;
	}

	public MethodSignatureEncoder MethodSignature(SignatureCallingConvention convention = SignatureCallingConvention.Default, int genericParameterCount = 0, bool isInstanceMethod = false)
	{
		throw null;
	}

	public GenericTypeArgumentsEncoder MethodSpecificationSignature(int genericArgumentCount)
	{
		throw null;
	}

	public NamedArgumentsEncoder PermissionSetArguments(int argumentCount)
	{
		throw null;
	}

	public PermissionSetEncoder PermissionSetBlob(int attributeCount)
	{
		throw null;
	}

	public MethodSignatureEncoder PropertySignature(bool isInstanceProperty = false)
	{
		throw null;
	}

	public SignatureTypeEncoder TypeSpecificationSignature()
	{
		throw null;
	}
}
