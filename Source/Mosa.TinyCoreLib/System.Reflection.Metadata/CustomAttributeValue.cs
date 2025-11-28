using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct CustomAttributeValue<TType>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ImmutableArray<CustomAttributeTypedArgument<TType>> FixedArguments
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<CustomAttributeNamedArgument<TType>> NamedArguments
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeValue(ImmutableArray<CustomAttributeTypedArgument<TType>> fixedArguments, ImmutableArray<CustomAttributeNamedArgument<TType>> namedArguments)
	{
		throw null;
	}
}
