using System.Diagnostics.CodeAnalysis;

namespace System.IO.Compression;

public class ZipArchiveEntry
{
	public ZipArchive Archive
	{
		get
		{
			throw null;
		}
	}

	public string Comment
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public long CompressedLength
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public uint Crc32
	{
		get
		{
			throw null;
		}
	}

	public int ExternalAttributes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string FullName
	{
		get
		{
			throw null;
		}
	}

	public bool IsEncrypted
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset LastWriteTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long Length
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	internal ZipArchiveEntry()
	{
	}

	public void Delete()
	{
	}

	public Stream Open()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
