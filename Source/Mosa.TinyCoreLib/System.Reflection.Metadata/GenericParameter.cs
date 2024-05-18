namespace System.Reflection.Metadata;

public readonly struct GenericParameter
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public GenericParameterAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public int Index
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

	public EntityHandle Parent
	{
		get
		{
			throw null;
		}
	}

	public GenericParameterConstraintHandleCollection GetConstraints()
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}
}
