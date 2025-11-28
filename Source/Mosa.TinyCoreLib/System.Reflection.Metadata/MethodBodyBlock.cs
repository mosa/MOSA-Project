using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public sealed class MethodBodyBlock
{
	public ImmutableArray<ExceptionRegion> ExceptionRegions
	{
		get
		{
			throw null;
		}
	}

	public StandaloneSignatureHandle LocalSignature
	{
		get
		{
			throw null;
		}
	}

	public bool LocalVariablesInitialized
	{
		get
		{
			throw null;
		}
	}

	public int MaxStack
	{
		get
		{
			throw null;
		}
	}

	public int Size
	{
		get
		{
			throw null;
		}
	}

	internal MethodBodyBlock()
	{
	}

	public static MethodBodyBlock Create(BlobReader reader)
	{
		throw null;
	}

	public byte[]? GetILBytes()
	{
		throw null;
	}

	public ImmutableArray<byte> GetILContent()
	{
		throw null;
	}

	public BlobReader GetILReader()
	{
		throw null;
	}
}
