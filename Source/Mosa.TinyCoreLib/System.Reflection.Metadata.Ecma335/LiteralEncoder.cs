namespace System.Reflection.Metadata.Ecma335;

public readonly struct LiteralEncoder
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

	public LiteralEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public ScalarEncoder Scalar()
	{
		throw null;
	}

	public void TaggedScalar(Action<CustomAttributeElementTypeEncoder> type, Action<ScalarEncoder> scalar)
	{
	}

	public void TaggedScalar(out CustomAttributeElementTypeEncoder type, out ScalarEncoder scalar)
	{
		throw null;
	}

	public void TaggedVector(Action<CustomAttributeArrayTypeEncoder> arrayType, Action<VectorEncoder> vector)
	{
	}

	public void TaggedVector(out CustomAttributeArrayTypeEncoder arrayType, out VectorEncoder vector)
	{
		throw null;
	}

	public VectorEncoder Vector()
	{
		throw null;
	}
}
