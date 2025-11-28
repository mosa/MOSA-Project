using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct EventAccessors
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public MethodDefinitionHandle Adder
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<MethodDefinitionHandle> Others
	{
		get
		{
			throw null;
		}
	}

	public MethodDefinitionHandle Raiser
	{
		get
		{
			throw null;
		}
	}

	public MethodDefinitionHandle Remover
	{
		get
		{
			throw null;
		}
	}
}
