namespace System.Reflection.Metadata;

public readonly struct ReservedBlob<THandle> where THandle : struct
{
	private readonly THandle _Handle_k__BackingField;

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public Blob Content
	{
		get
		{
			throw null;
		}
	}

	public THandle Handle
	{
		get
		{
			throw null;
		}
	}

	public BlobWriter CreateWriter()
	{
		throw null;
	}
}
