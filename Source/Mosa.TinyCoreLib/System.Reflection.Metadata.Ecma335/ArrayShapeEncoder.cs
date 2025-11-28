using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335;

public readonly struct ArrayShapeEncoder
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

	public ArrayShapeEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public void Shape(int rank, ImmutableArray<int> sizes, ImmutableArray<int> lowerBounds)
	{
	}
}
