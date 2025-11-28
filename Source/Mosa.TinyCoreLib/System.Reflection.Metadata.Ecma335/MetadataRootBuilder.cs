namespace System.Reflection.Metadata.Ecma335;

public sealed class MetadataRootBuilder
{
	public string MetadataVersion
	{
		get
		{
			throw null;
		}
	}

	public MetadataSizes Sizes
	{
		get
		{
			throw null;
		}
	}

	public bool SuppressValidation
	{
		get
		{
			throw null;
		}
	}

	public MetadataRootBuilder(MetadataBuilder tablesAndHeaps, string? metadataVersion = null, bool suppressValidation = false)
	{
	}

	public void Serialize(BlobBuilder builder, int methodBodyStreamRva, int mappedFieldDataStreamRva)
	{
	}
}
