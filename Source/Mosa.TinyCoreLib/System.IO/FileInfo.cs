using System.Runtime.Versioning;

namespace System.IO;

public sealed class FileInfo : FileSystemInfo
{
	public DirectoryInfo? Directory
	{
		get
		{
			throw null;
		}
	}

	public string? DirectoryName
	{
		get
		{
			throw null;
		}
	}

	public override bool Exists
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
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

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public FileInfo(string fileName)
	{
	}

	public StreamWriter AppendText()
	{
		throw null;
	}

	public FileInfo CopyTo(string destFileName)
	{
		throw null;
	}

	public FileInfo CopyTo(string destFileName, bool overwrite)
	{
		throw null;
	}

	public FileStream Create()
	{
		throw null;
	}

	public StreamWriter CreateText()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public void Decrypt()
	{
	}

	public override void Delete()
	{
	}

	[SupportedOSPlatform("windows")]
	public void Encrypt()
	{
	}

	public void MoveTo(string destFileName)
	{
	}

	public void MoveTo(string destFileName, bool overwrite)
	{
	}

	public FileStream Open(FileMode mode)
	{
		throw null;
	}

	public FileStream Open(FileMode mode, FileAccess access)
	{
		throw null;
	}

	public FileStream Open(FileMode mode, FileAccess access, FileShare share)
	{
		throw null;
	}

	public FileStream Open(FileStreamOptions options)
	{
		throw null;
	}

	public FileStream OpenRead()
	{
		throw null;
	}

	public StreamReader OpenText()
	{
		throw null;
	}

	public FileStream OpenWrite()
	{
		throw null;
	}

	public FileInfo Replace(string destinationFileName, string? destinationBackupFileName)
	{
		throw null;
	}

	public FileInfo Replace(string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
	{
		throw null;
	}
}
