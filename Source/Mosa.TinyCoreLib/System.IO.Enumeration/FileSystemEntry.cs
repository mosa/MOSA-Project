namespace System.IO.Enumeration;

public ref struct FileSystemEntry
{
	private object _dummy;

	private int _dummyPrimitive;

	public FileAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset CreationTimeUtc
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySpan<char> Directory
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySpan<char> FileName
	{
		get
		{
			throw null;
		}
	}

	public bool IsDirectory
	{
		get
		{
			throw null;
		}
	}

	public bool IsHidden
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset LastAccessTimeUtc
	{
		get
		{
			throw null;
		}
	}

	public DateTimeOffset LastWriteTimeUtc
	{
		get
		{
			throw null;
		}
	}

	public long Length
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySpan<char> OriginalRootDirectory
	{
		get
		{
			throw null;
		}
	}

	public readonly ReadOnlySpan<char> RootDirectory
	{
		get
		{
			throw null;
		}
	}

	public FileSystemInfo ToFileSystemInfo()
	{
		throw null;
	}

	public string ToFullPath()
	{
		throw null;
	}

	public string ToSpecifiedFullPath()
	{
		throw null;
	}
}
