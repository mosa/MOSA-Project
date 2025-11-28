using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct MethodSignature<TType>
{
	private readonly TType _ReturnType_k__BackingField;

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public int GenericParameterCount
	{
		get
		{
			throw null;
		}
	}

	public SignatureHeader Header
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<TType> ParameterTypes
	{
		get
		{
			throw null;
		}
	}

	public int RequiredParameterCount
	{
		get
		{
			throw null;
		}
	}

	public TType ReturnType
	{
		get
		{
			throw null;
		}
	}

	public MethodSignature(SignatureHeader header, TType returnType, int requiredParameterCount, int genericParameterCount, ImmutableArray<TType> parameterTypes)
	{
		throw null;
	}
}
