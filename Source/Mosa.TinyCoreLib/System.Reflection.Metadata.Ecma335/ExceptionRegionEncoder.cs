namespace System.Reflection.Metadata.Ecma335;

public readonly struct ExceptionRegionEncoder
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

	public bool HasSmallFormat
	{
		get
		{
			throw null;
		}
	}

	public ExceptionRegionEncoder Add(ExceptionRegionKind kind, int tryOffset, int tryLength, int handlerOffset, int handlerLength, EntityHandle catchType = default(EntityHandle), int filterOffset = 0)
	{
		throw null;
	}

	public ExceptionRegionEncoder AddCatch(int tryOffset, int tryLength, int handlerOffset, int handlerLength, EntityHandle catchType)
	{
		throw null;
	}

	public ExceptionRegionEncoder AddFault(int tryOffset, int tryLength, int handlerOffset, int handlerLength)
	{
		throw null;
	}

	public ExceptionRegionEncoder AddFilter(int tryOffset, int tryLength, int handlerOffset, int handlerLength, int filterOffset)
	{
		throw null;
	}

	public ExceptionRegionEncoder AddFinally(int tryOffset, int tryLength, int handlerOffset, int handlerLength)
	{
		throw null;
	}

	public static bool IsSmallExceptionRegion(int startOffset, int length)
	{
		throw null;
	}

	public static bool IsSmallRegionCount(int exceptionRegionCount)
	{
		throw null;
	}
}
