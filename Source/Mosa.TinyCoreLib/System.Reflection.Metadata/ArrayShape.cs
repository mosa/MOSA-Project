using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public readonly struct ArrayShape
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ImmutableArray<int> LowerBounds
	{
		get
		{
			throw null;
		}
	}

	public int Rank
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<int> Sizes
	{
		get
		{
			throw null;
		}
	}

	public ArrayShape(int rank, ImmutableArray<int> sizes, ImmutableArray<int> lowerBounds)
	{
		throw null;
	}
}
