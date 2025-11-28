using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Formats.Tar;

public abstract class TarEntry
{
	public int Checksum
	{
		get
		{
			throw null;
		}
	}

	public Stream? DataStream
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TarEntryType EntryType
	{
		get
		{
			throw null;
		}
	}

	public TarEntryFormat Format
	{
		get
		{
			throw null;
		}
	}

	public int Gid
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

	public string LinkName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UnixFileMode Mode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTimeOffset ModificationTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Uid
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal TarEntry()
	{
	}

	public void ExtractToFile(string destinationFileName, bool overwrite)
	{
	}

	public Task ExtractToFileAsync(string destinationFileName, bool overwrite, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
