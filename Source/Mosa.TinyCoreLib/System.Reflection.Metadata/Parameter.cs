namespace System.Reflection.Metadata;

public readonly struct Parameter
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ParameterAttributes Attributes
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

	public int SequenceNumber
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
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
}
