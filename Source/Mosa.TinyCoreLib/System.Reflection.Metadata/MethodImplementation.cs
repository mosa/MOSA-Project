namespace System.Reflection.Metadata;

public readonly struct MethodImplementation
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public EntityHandle MethodBody
	{
		get
		{
			throw null;
		}
	}

	public EntityHandle MethodDeclaration
	{
		get
		{
			throw null;
		}
	}

	public TypeDefinitionHandle Type
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
