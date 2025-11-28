using System.Collections.Generic;

namespace System.Reflection.Metadata.Ecma335;

public sealed class MetadataAggregator
{
	public MetadataAggregator(IReadOnlyList<int>? baseTableRowCounts, IReadOnlyList<int>? baseHeapSizes, IReadOnlyList<MetadataReader>? deltaReaders)
	{
	}

	public MetadataAggregator(MetadataReader baseReader, IReadOnlyList<MetadataReader> deltaReaders)
	{
	}

	public Handle GetGenerationHandle(Handle handle, out int generation)
	{
		throw null;
	}
}
