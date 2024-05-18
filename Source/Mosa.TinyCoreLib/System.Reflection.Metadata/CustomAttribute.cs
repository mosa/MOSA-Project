namespace System.Reflection.Metadata;

public readonly struct CustomAttribute
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public EntityHandle Constructor
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

	public BlobHandle Value
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeValue<TType> DecodeValue<TType>(ICustomAttributeTypeProvider<TType> provider)
	{
		throw null;
	}
}
