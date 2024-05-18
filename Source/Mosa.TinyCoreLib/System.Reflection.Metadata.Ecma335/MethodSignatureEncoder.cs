namespace System.Reflection.Metadata.Ecma335;

public readonly struct MethodSignatureEncoder
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public BlobBuilder Builder
	{
		get
		{
			throw null;
		}
	}

	public bool HasVarArgs
	{
		get
		{
			throw null;
		}
	}

	public MethodSignatureEncoder(BlobBuilder builder, bool hasVarArgs)
	{
		throw null;
	}

	public void Parameters(int parameterCount, Action<ReturnTypeEncoder> returnType, Action<ParametersEncoder> parameters)
	{
	}

	public void Parameters(int parameterCount, out ReturnTypeEncoder returnType, out ParametersEncoder parameters)
	{
		throw null;
	}
}
