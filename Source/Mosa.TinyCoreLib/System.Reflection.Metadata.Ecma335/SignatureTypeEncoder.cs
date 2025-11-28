namespace System.Reflection.Metadata.Ecma335;

public readonly struct SignatureTypeEncoder
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

	public SignatureTypeEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public void Array(Action<SignatureTypeEncoder> elementType, Action<ArrayShapeEncoder> arrayShape)
	{
	}

	public void Array(out SignatureTypeEncoder elementType, out ArrayShapeEncoder arrayShape)
	{
		throw null;
	}

	public void Boolean()
	{
	}

	public void Byte()
	{
	}

	public void Char()
	{
	}

	public CustomModifiersEncoder CustomModifiers()
	{
		throw null;
	}

	public void Double()
	{
	}

	public MethodSignatureEncoder FunctionPointer(SignatureCallingConvention convention = SignatureCallingConvention.Default, FunctionPointerAttributes attributes = FunctionPointerAttributes.None, int genericParameterCount = 0)
	{
		throw null;
	}

	public GenericTypeArgumentsEncoder GenericInstantiation(EntityHandle genericType, int genericArgumentCount, bool isValueType)
	{
		throw null;
	}

	public void GenericMethodTypeParameter(int parameterIndex)
	{
	}

	public void GenericTypeParameter(int parameterIndex)
	{
	}

	public void Int16()
	{
	}

	public void Int32()
	{
	}

	public void Int64()
	{
	}

	public void IntPtr()
	{
	}

	public void Object()
	{
	}

	public SignatureTypeEncoder Pointer()
	{
		throw null;
	}

	public void PrimitiveType(PrimitiveTypeCode type)
	{
	}

	public void SByte()
	{
	}

	public void Single()
	{
	}

	public void String()
	{
	}

	public SignatureTypeEncoder SZArray()
	{
		throw null;
	}

	public void Type(EntityHandle type, bool isValueType)
	{
	}

	public void TypedReference()
	{
	}

	public void UInt16()
	{
	}

	public void UInt32()
	{
	}

	public void UInt64()
	{
	}

	public void UIntPtr()
	{
	}

	public void VoidPointer()
	{
	}
}
