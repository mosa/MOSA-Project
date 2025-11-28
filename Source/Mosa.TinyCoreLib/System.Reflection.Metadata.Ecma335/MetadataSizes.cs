using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335;

public sealed class MetadataSizes
{
	public ImmutableArray<int> ExternalRowCounts
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<int> HeapSizes
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<int> RowCounts
	{
		get
		{
			throw null;
		}
	}

	internal MetadataSizes()
	{
	}

	public int GetAlignedHeapSize(HeapIndex index)
	{
		throw null;
	}
}
