using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public struct NamespaceDefinition
{
	private object _dummy;

	private int _dummyPrimitive;

	public ImmutableArray<ExportedTypeHandle> ExportedTypes
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

	public ImmutableArray<NamespaceDefinitionHandle> NamespaceDefinitions
	{
		get
		{
			throw null;
		}
	}

	public NamespaceDefinitionHandle Parent
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<TypeDefinitionHandle> TypeDefinitions
	{
		get
		{
			throw null;
		}
	}
}
