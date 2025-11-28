using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct PropertyAccessors
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public MethodDefinitionHandle Getter
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

	public MethodDefinitionHandle Setter
	{
		get
		{
			throw null;
		}
	}
}
