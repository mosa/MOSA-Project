using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace System.Reflection.PortableExecutable;

public readonly struct PEMemoryBlock
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public unsafe byte* Pointer
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<byte> GetContent()
	{
		throw null;
	}

	public ImmutableArray<byte> GetContent(int start, int length)
	{
		throw null;
	}

	public BlobReader GetReader()
	{
		throw null;
	}

	public BlobReader GetReader(int start, int length)
	{
		throw null;
	}
}
