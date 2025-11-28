using System.Security.AccessControl;

namespace System.IO;

public static class FileSystemAclExtensions
{
	public static void Create(this DirectoryInfo directoryInfo, DirectorySecurity directorySecurity)
	{
	}

	public static FileStream Create(this FileInfo fileInfo, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity? fileSecurity)
	{
		throw null;
	}

	public static DirectoryInfo CreateDirectory(this DirectorySecurity directorySecurity, string path)
	{
		throw null;
	}

	public static DirectorySecurity GetAccessControl(this DirectoryInfo directoryInfo)
	{
		throw null;
	}

	public static DirectorySecurity GetAccessControl(this DirectoryInfo directoryInfo, AccessControlSections includeSections)
	{
		throw null;
	}

	public static FileSecurity GetAccessControl(this FileInfo fileInfo)
	{
		throw null;
	}

	public static FileSecurity GetAccessControl(this FileInfo fileInfo, AccessControlSections includeSections)
	{
		throw null;
	}

	public static FileSecurity GetAccessControl(this FileStream fileStream)
	{
		throw null;
	}

	public static void SetAccessControl(this DirectoryInfo directoryInfo, DirectorySecurity directorySecurity)
	{
	}

	public static void SetAccessControl(this FileInfo fileInfo, FileSecurity fileSecurity)
	{
	}

	public static void SetAccessControl(this FileStream fileStream, FileSecurity fileSecurity)
	{
	}
}
