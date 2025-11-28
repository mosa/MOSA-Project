namespace System.Reflection.Metadata;

public readonly struct Constant
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public EntityHandle Parent
	{
		get
		{
			throw null;
		}
	}

	public ConstantTypeCode TypeCode
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
}
