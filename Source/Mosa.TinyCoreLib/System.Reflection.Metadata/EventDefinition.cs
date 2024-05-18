namespace System.Reflection.Metadata;

public readonly struct EventDefinition
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public EventAttributes Attributes
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

	public EntityHandle Type
	{
		get
		{
			throw null;
		}
	}

	public EventAccessors GetAccessors()
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}
}
