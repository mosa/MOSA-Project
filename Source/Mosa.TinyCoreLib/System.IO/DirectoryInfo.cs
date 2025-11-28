using System.Collections.Generic;

namespace System.IO;

public sealed class DirectoryInfo : FileSystemInfo
{
	public override bool Exists
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

	public DirectoryInfo? Parent
	{
		get
		{
			throw null;
		}
	}

	public DirectoryInfo Root
	{
		get
		{
			throw null;
		}
	}

	public DirectoryInfo(string path)
	{
	}

	public void Create()
	{
	}

	public DirectoryInfo CreateSubdirectory(string path)
	{
		throw null;
	}

	public override void Delete()
	{
	}

	public void Delete(bool recursive)
	{
	}

	public IEnumerable<DirectoryInfo> EnumerateDirectories()
	{
		throw null;
	}

	public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern)
	{
		throw null;
	}

	public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public IEnumerable<FileInfo> EnumerateFiles()
	{
		throw null;
	}

	public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
	{
		throw null;
	}

	public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
	{
		throw null;
	}

	public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
	{
		throw null;
	}

	public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public DirectoryInfo[] GetDirectories()
	{
		throw null;
	}

	public DirectoryInfo[] GetDirectories(string searchPattern)
	{
		throw null;
	}

	public DirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public FileInfo[] GetFiles()
	{
		throw null;
	}

	public FileInfo[] GetFiles(string searchPattern)
	{
		throw null;
	}

	public FileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public FileSystemInfo[] GetFileSystemInfos()
	{
		throw null;
	}

	public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
	{
		throw null;
	}

	public FileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public void MoveTo(string destDirName)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
