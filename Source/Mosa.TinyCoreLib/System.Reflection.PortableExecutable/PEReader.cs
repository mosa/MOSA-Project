using System.Collections.Immutable;
using System.IO;
using System.Reflection.Metadata;

namespace System.Reflection.PortableExecutable;

public sealed class PEReader : IDisposable
{
	public bool HasMetadata
	{
		get
		{
			throw null;
		}
	}

	public bool IsEntireImageAvailable
	{
		get
		{
			throw null;
		}
	}

	public bool IsLoadedImage
	{
		get
		{
			throw null;
		}
	}

	public PEHeaders PEHeaders
	{
		get
		{
			throw null;
		}
	}

	public unsafe PEReader(byte* peImage, int size)
	{
	}

	public unsafe PEReader(byte* peImage, int size, bool isLoadedImage)
	{
	}

	public PEReader(ImmutableArray<byte> peImage)
	{
	}

	public PEReader(Stream peStream)
	{
	}

	public PEReader(Stream peStream, PEStreamOptions options)
	{
	}

	public PEReader(Stream peStream, PEStreamOptions options, int size)
	{
	}

	public void Dispose()
	{
	}

	public PEMemoryBlock GetEntireImage()
	{
		throw null;
	}

	public PEMemoryBlock GetMetadata()
	{
		throw null;
	}

	public PEMemoryBlock GetSectionData(int relativeVirtualAddress)
	{
		throw null;
	}

	public PEMemoryBlock GetSectionData(string sectionName)
	{
		throw null;
	}

	public CodeViewDebugDirectoryData ReadCodeViewDebugDirectoryData(DebugDirectoryEntry entry)
	{
		throw null;
	}

	public ImmutableArray<DebugDirectoryEntry> ReadDebugDirectory()
	{
		throw null;
	}

	public MetadataReaderProvider ReadEmbeddedPortablePdbDebugDirectoryData(DebugDirectoryEntry entry)
	{
		throw null;
	}

	public PdbChecksumDebugDirectoryData ReadPdbChecksumDebugDirectoryData(DebugDirectoryEntry entry)
	{
		throw null;
	}

	public bool TryOpenAssociatedPortablePdb(string peImagePath, Func<string, Stream?> pdbFileStreamProvider, out MetadataReaderProvider? pdbReaderProvider, out string? pdbPath)
	{
		throw null;
	}
}
