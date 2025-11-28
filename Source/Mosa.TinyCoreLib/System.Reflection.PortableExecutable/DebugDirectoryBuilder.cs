using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace System.Reflection.PortableExecutable;

public sealed class DebugDirectoryBuilder
{
	public void AddCodeViewEntry(string pdbPath, BlobContentId pdbContentId, ushort portablePdbVersion)
	{
	}

	public void AddCodeViewEntry(string pdbPath, BlobContentId pdbContentId, ushort portablePdbVersion, int age)
	{
	}

	public void AddEmbeddedPortablePdbEntry(BlobBuilder debugMetadata, ushort portablePdbVersion)
	{
	}

	public void AddEntry(DebugDirectoryEntryType type, uint version, uint stamp)
	{
	}

	public void AddEntry<TData>(DebugDirectoryEntryType type, uint version, uint stamp, TData data, Action<BlobBuilder, TData> dataSerializer)
	{
	}

	public void AddPdbChecksumEntry(string algorithmName, ImmutableArray<byte> checksum)
	{
	}

	public void AddReproducibleEntry()
	{
	}
}
