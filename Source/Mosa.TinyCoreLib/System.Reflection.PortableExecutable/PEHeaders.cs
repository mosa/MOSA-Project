using System.Collections.Immutable;
using System.IO;

namespace System.Reflection.PortableExecutable;

public sealed class PEHeaders
{
	public CoffHeader CoffHeader
	{
		get
		{
			throw null;
		}
	}

	public int CoffHeaderStartOffset
	{
		get
		{
			throw null;
		}
	}

	public CorHeader? CorHeader
	{
		get
		{
			throw null;
		}
	}

	public int CorHeaderStartOffset
	{
		get
		{
			throw null;
		}
	}

	public bool IsCoffOnly
	{
		get
		{
			throw null;
		}
	}

	public bool IsConsoleApplication
	{
		get
		{
			throw null;
		}
	}

	public bool IsDll
	{
		get
		{
			throw null;
		}
	}

	public bool IsExe
	{
		get
		{
			throw null;
		}
	}

	public int MetadataSize
	{
		get
		{
			throw null;
		}
	}

	public int MetadataStartOffset
	{
		get
		{
			throw null;
		}
	}

	public PEHeader? PEHeader
	{
		get
		{
			throw null;
		}
	}

	public int PEHeaderStartOffset
	{
		get
		{
			throw null;
		}
	}

	public ImmutableArray<SectionHeader> SectionHeaders
	{
		get
		{
			throw null;
		}
	}

	public PEHeaders(Stream peStream)
	{
	}

	public PEHeaders(Stream peStream, int size)
	{
	}

	public PEHeaders(Stream peStream, int size, bool isLoadedImage)
	{
	}

	public int GetContainingSectionIndex(int relativeVirtualAddress)
	{
		throw null;
	}

	public bool TryGetDirectoryOffset(DirectoryEntry directory, out int offset)
	{
		throw null;
	}
}
