using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

public static class File
{
	public static void AppendAllLines(string path, IEnumerable<string> contents)
	{
	}

	public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
	{
	}

	public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static void AppendAllText(string path, string? contents)
	{
	}

	public static void AppendAllText(string path, string? contents, Encoding encoding)
	{
	}

	public static Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static StreamWriter AppendText(string path)
	{
		throw null;
	}

	public static void Copy(string sourceFileName, string destFileName)
	{
	}

	public static void Copy(string sourceFileName, string destFileName, bool overwrite)
	{
	}

	public static FileStream Create(string path)
	{
		throw null;
	}

	public static FileStream Create(string path, int bufferSize)
	{
		throw null;
	}

	public static FileStream Create(string path, int bufferSize, FileOptions options)
	{
		throw null;
	}

	public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
	{
		throw null;
	}

	public static StreamWriter CreateText(string path)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static void Decrypt(string path)
	{
	}

	public static void Delete(string path)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void Encrypt(string path)
	{
	}

	public static bool Exists([NotNullWhen(true)] string? path)
	{
		throw null;
	}

	public static FileAttributes GetAttributes(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static FileAttributes GetAttributes(string path)
	{
		throw null;
	}

	public static DateTime GetCreationTime(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static DateTime GetCreationTime(string path)
	{
		throw null;
	}

	public static DateTime GetCreationTimeUtc(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static DateTime GetCreationTimeUtc(string path)
	{
		throw null;
	}

	public static DateTime GetLastAccessTime(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static DateTime GetLastAccessTime(string path)
	{
		throw null;
	}

	public static DateTime GetLastAccessTimeUtc(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static DateTime GetLastAccessTimeUtc(string path)
	{
		throw null;
	}

	public static DateTime GetLastWriteTime(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static DateTime GetLastWriteTime(string path)
	{
		throw null;
	}

	public static DateTime GetLastWriteTimeUtc(SafeFileHandle fileHandle)
	{
		throw null;
	}

	public static DateTime GetLastWriteTimeUtc(string path)
	{
		throw null;
	}

	[UnsupportedOSPlatform("windows")]
	public static UnixFileMode GetUnixFileMode(SafeFileHandle fileHandle)
	{
		throw null;
	}

	[UnsupportedOSPlatform("windows")]
	public static UnixFileMode GetUnixFileMode(string path)
	{
		throw null;
	}

	public static void Move(string sourceFileName, string destFileName)
	{
	}

	public static void Move(string sourceFileName, string destFileName, bool overwrite)
	{
	}

	public static FileStream Open(string path, FileMode mode)
	{
		throw null;
	}

	public static FileStream Open(string path, FileMode mode, FileAccess access)
	{
		throw null;
	}

	public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
	{
		throw null;
	}

	public static FileStream Open(string path, FileStreamOptions options)
	{
		throw null;
	}

	public static SafeFileHandle OpenHandle(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read, FileOptions options = FileOptions.None, long preallocationSize = 0L)
	{
		throw null;
	}

	public static FileStream OpenRead(string path)
	{
		throw null;
	}

	public static StreamReader OpenText(string path)
	{
		throw null;
	}

	public static FileStream OpenWrite(string path)
	{
		throw null;
	}

	public static byte[] ReadAllBytes(string path)
	{
		throw null;
	}

	public static Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static string[] ReadAllLines(string path)
	{
		throw null;
	}

	public static string[] ReadAllLines(string path, Encoding encoding)
	{
		throw null;
	}

	public static Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static string ReadAllText(string path)
	{
		throw null;
	}

	public static string ReadAllText(string path, Encoding encoding)
	{
		throw null;
	}

	public static Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IEnumerable<string> ReadLines(string path)
	{
		throw null;
	}

	public static IEnumerable<string> ReadLines(string path, Encoding encoding)
	{
		throw null;
	}

	public static IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName)
	{
	}

	public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
	{
	}

	public static FileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget)
	{
		throw null;
	}

	public static void SetAttributes(SafeFileHandle fileHandle, FileAttributes fileAttributes)
	{
	}

	public static void SetAttributes(string path, FileAttributes fileAttributes)
	{
	}

	public static void SetCreationTime(SafeFileHandle fileHandle, DateTime creationTime)
	{
	}

	public static void SetCreationTime(string path, DateTime creationTime)
	{
	}

	public static void SetCreationTimeUtc(SafeFileHandle fileHandle, DateTime creationTimeUtc)
	{
	}

	public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
	{
	}

	public static void SetLastAccessTime(SafeFileHandle fileHandle, DateTime lastAccessTime)
	{
	}

	public static void SetLastAccessTime(string path, DateTime lastAccessTime)
	{
	}

	public static void SetLastAccessTimeUtc(SafeFileHandle fileHandle, DateTime lastAccessTimeUtc)
	{
	}

	public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
	{
	}

	public static void SetLastWriteTime(SafeFileHandle fileHandle, DateTime lastWriteTime)
	{
	}

	public static void SetLastWriteTime(string path, DateTime lastWriteTime)
	{
	}

	public static void SetLastWriteTimeUtc(SafeFileHandle fileHandle, DateTime lastWriteTimeUtc)
	{
	}

	public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
	{
	}

	[UnsupportedOSPlatform("windows")]
	public static void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode)
	{
	}

	[UnsupportedOSPlatform("windows")]
	public static void SetUnixFileMode(string path, UnixFileMode mode)
	{
	}

	public static void WriteAllBytes(string path, byte[] bytes)
	{
	}

	public static Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static void WriteAllLines(string path, IEnumerable<string> contents)
	{
	}

	public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
	{
	}

	public static void WriteAllLines(string path, string[] contents)
	{
	}

	public static void WriteAllLines(string path, string[] contents, Encoding encoding)
	{
	}

	public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static void WriteAllText(string path, string? contents)
	{
	}

	public static void WriteAllText(string path, string? contents, Encoding encoding)
	{
	}

	public static Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
