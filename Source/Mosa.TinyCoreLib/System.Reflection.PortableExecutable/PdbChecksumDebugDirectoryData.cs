using System.Collections.Immutable;

namespace System.Reflection.PortableExecutable;

public readonly struct PdbChecksumDebugDirectoryData
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public string AlgorithmName
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<byte> Checksum
	{
		get
		{
			throw null;
		}
	}
}
