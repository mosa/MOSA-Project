using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct TypeDefinition
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

	public EntityHandle BaseType
	{
		get
		{
			throw null;
		}
	}

	public bool IsNested
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

	public DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes()
	{
		throw null;
	}

	public TypeDefinitionHandle GetDeclaringType()
	{
		throw null;
	}

	public EventDefinitionHandleCollection GetEvents()
	{
		throw null;
	}

	public FieldDefinitionHandleCollection GetFields()
	{
		throw null;
	}

	public GenericParameterHandleCollection GetGenericParameters()
	{
		throw null;
	}

	public InterfaceImplementationHandleCollection GetInterfaceImplementations()
	{
		throw null;
	}

	public TypeLayout GetLayout()
	{
		throw null;
	}

	public MethodImplementationHandleCollection GetMethodImplementations()
	{
		throw null;
	}

	public MethodDefinitionHandleCollection GetMethods()
	{
		throw null;
	}

	public ImmutableArray<TypeDefinitionHandle> GetNestedTypes()
	{
		throw null;
	}

	public PropertyDefinitionHandleCollection GetProperties()
	{
		throw null;
	}
}
