using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.IO;

public static class Directory
{
	public static DirectoryInfo CreateDirectory(string path)
	{
		throw null;
	}

	[UnsupportedOSPlatform("windows")]
	public static DirectoryInfo CreateDirectory(string path, UnixFileMode unixCreateMode)
	{
		throw null;
	}

	public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
	{
		throw null;
	}

	public static DirectoryInfo CreateTempSubdirectory(string? prefix = null)
	{
		throw null;
	}

	public static void Delete(string path)
	{
	}

	public static void Delete(string path, bool recursive)
	{
	}

	public static IEnumerable<string> EnumerateDirectories(string path)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFiles(string path)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFileSystemEntries(string path)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public static bool Exists([NotNullWhen(true)] string? path)
	{
		throw null;
	}

	public static DateTime GetCreationTime(string path)
	{
		throw null;
	}

	public static DateTime GetCreationTimeUtc(string path)
	{
		throw null;
	}

	public static string GetCurrentDirectory()
	{
		throw null;
	}

	public static string[] GetDirectories(string path)
	{
		throw null;
	}

	public static string[] GetDirectories(string path, string searchPattern)
	{
		throw null;
	}

	public static string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public static string GetDirectoryRoot(string path)
	{
		throw null;
	}

	public static string[] GetFiles(string path)
	{
		throw null;
	}

	public static string[] GetFiles(string path, string searchPattern)
	{
		throw null;
	}

	public static string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public static string[] GetFileSystemEntries(string path)
	{
		throw null;
	}

	public static string[] GetFileSystemEntries(string path, string searchPattern)
	{
		throw null;
	}

	public static string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
	{
		throw null;
	}

	public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	{
		throw null;
	}

	public static DateTime GetLastAccessTime(string path)
	{
		throw null;
	}

	public static DateTime GetLastAccessTimeUtc(string path)
	{
		throw null;
	}

	public static DateTime GetLastWriteTime(string path)
	{
		throw null;
	}

	public static DateTime GetLastWriteTimeUtc(string path)
	{
		throw null;
	}

	public static string[] GetLogicalDrives()
	{
		throw null;
	}

	public static DirectoryInfo? GetParent(string path)
	{
		throw null;
	}

	public static void Move(string sourceDirName, string destDirName)
	{
	}

	public static FileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget)
	{
		throw null;
	}

	public static void SetCreationTime(string path, DateTime creationTime)
	{
	}

	public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
	{
	}

	public static void SetCurrentDirectory(string path)
	{
	}

	public static void SetLastAccessTime(string path, DateTime lastAccessTime)
	{
	}

	public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
	{
	}

	public static void SetLastWriteTime(string path, DateTime lastWriteTime)
	{
	}

	public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
	{
	}
}
