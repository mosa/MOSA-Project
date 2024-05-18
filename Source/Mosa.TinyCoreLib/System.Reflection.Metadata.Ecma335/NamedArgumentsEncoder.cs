namespace System.Reflection.Metadata.Ecma335;

public readonly struct NamedArgumentsEncoder
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

	public NamedArgumentsEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public void AddArgument(bool isField, Action<NamedArgumentTypeEncoder> type, Action<NameEncoder> name, Action<LiteralEncoder> literal)
	{
	}

	public void AddArgument(bool isField, out NamedArgumentTypeEncoder type, out NameEncoder name, out LiteralEncoder literal)
	{
		throw null;
	}
}
