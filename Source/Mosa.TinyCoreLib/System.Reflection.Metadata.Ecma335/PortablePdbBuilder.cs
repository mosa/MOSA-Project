using System.Collections.Generic;
using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335;

public sealed class PortablePdbBuilder
{
	public ushort FormatVersion
	{
		get
		{
			throw null;
		}
	}

	public Func<IEnumerable<Blob>, BlobContentId> IdProvider
	{
		get
		{
			throw null;
		}
	}

	public string MetadataVersion
	{
		get
		{
			throw null;
		}
	}

	public PortablePdbBuilder(MetadataBuilder tablesAndHeaps, ImmutableArray<int> typeSystemRowCounts, MethodDefinitionHandle entryPoint, Func<IEnumerable<Blob>, BlobContentId>? idProvider = null)
	{
	}

	public BlobContentId Serialize(BlobBuilder builder)
	{
		throw null;
	}
}
