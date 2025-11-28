namespace System.Reflection.Metadata;

public readonly struct ExportedType
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public TypeAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public EntityHandle Implementation
	{
		get
		{
			throw null;
		}
	}

	public bool IsForwarder
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

	public StringHandle Namespace
	{
		get
		{
			throw null;
		}
	}

	public NamespaceDefinitionHandle NamespaceDefinition
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
}
