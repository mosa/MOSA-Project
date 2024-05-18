namespace System.Reflection.Metadata.Ecma335;

public readonly struct ParametersEncoder
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

	public ParametersEncoder(BlobBuilder builder, bool hasVarArgs = false)
	{
		throw null;
	}

	public ParameterTypeEncoder AddParameter()
	{
		throw null;
	}

	public ParametersEncoder StartVarArgs()
	{
		throw null;
	}
}
