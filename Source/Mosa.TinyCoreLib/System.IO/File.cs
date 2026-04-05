using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Internal;
using Microsoft.Win32.SafeHandles;

namespace System.IO;

public static class File
{
	public static void AppendAllLines(string path, IEnumerable<string> contents) => throw new NotImplementedException();

	public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		=> throw new NotImplementedException();

	public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static void AppendAllText(string path, string? contents) => throw new NotImplementedException();

	public static void AppendAllText(string path, string? contents, Encoding encoding)
		=> throw new NotImplementedException();

	public static Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static StreamWriter AppendText(string path) => throw new NotImplementedException();

	public static void Copy(string sourceFileName, string destFileName) => throw new NotImplementedException();

	public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		=> throw new NotImplementedException();

	// TODO: Make Impl.File.Create() return a FileStream
	public static FileStream Create(string path) => throw new NotImplementedException();

	public static FileStream Create(string path, int bufferSize) => throw new NotImplementedException();

	public static FileStream Create(string path, int bufferSize, FileOptions options)
		=> throw new NotImplementedException();

	public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
		=> throw new NotImplementedException();

	public static StreamWriter CreateText(string path) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static void Decrypt(string path) => throw new NotImplementedException();

	public static void Delete(string path) => throw new NotImplementedException();

	[SupportedOSPlatform("windows")]
	public static void Encrypt(string path) => throw new NotImplementedException();

	public static bool Exists([NotNullWhen(true)] string? path) => Impl.File.Exists(path);

	public static FileAttributes GetAttributes(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static FileAttributes GetAttributes(string path) => throw new NotImplementedException();

	public static DateTime GetCreationTime(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static DateTime GetCreationTime(string path) => throw new NotImplementedException();

	public static DateTime GetCreationTimeUtc(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static DateTime GetCreationTimeUtc(string path) => throw new NotImplementedException();

	public static DateTime GetLastAccessTime(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static DateTime GetLastAccessTime(string path) => throw new NotImplementedException();

	public static DateTime GetLastAccessTimeUtc(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static DateTime GetLastAccessTimeUtc(string path) => throw new NotImplementedException();

	public static DateTime GetLastWriteTime(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static DateTime GetLastWriteTime(string path) => throw new NotImplementedException();

	public static DateTime GetLastWriteTimeUtc(SafeFileHandle fileHandle) => throw new NotImplementedException();

	public static DateTime GetLastWriteTimeUtc(string path) => throw new NotImplementedException();

	[UnsupportedOSPlatform("windows")]
	public static UnixFileMode GetUnixFileMode(SafeFileHandle fileHandle) => throw new NotImplementedException();

	[UnsupportedOSPlatform("windows")]
	public static UnixFileMode GetUnixFileMode(string path) => throw new NotImplementedException();

	public static void Move(string sourceFileName, string destFileName) => throw new NotImplementedException();

	public static void Move(string sourceFileName, string destFileName, bool overwrite)
		=> throw new NotImplementedException();

	public static FileStream Open(string path, FileMode mode) => throw new NotImplementedException();

	public static FileStream Open(string path, FileMode mode, FileAccess access) => throw new NotImplementedException();

	public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
		=> throw new NotImplementedException();

	public static FileStream Open(string path, FileStreamOptions options) => throw new NotImplementedException();

	public static SafeFileHandle OpenHandle(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read, FileOptions options = FileOptions.None, long preallocationSize = 0L)
		=> throw new NotImplementedException();

	public static FileStream OpenRead(string path) => throw new NotImplementedException();

	public static StreamReader OpenText(string path) => throw new NotImplementedException();

	public static FileStream OpenWrite(string path) => throw new NotImplementedException();

	public static byte[] ReadAllBytes(string path) => Impl.File.ReadAllBytes(path);

	public static Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static string[] ReadAllLines(string path) => Impl.File.ReadAllLines(path);

	public static string[] ReadAllLines(string path, Encoding encoding) => throw new NotImplementedException();

	public static Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static string ReadAllText(string path) => Impl.File.ReadAllText(path);

	public static string ReadAllText(string path, Encoding encoding) => throw new NotImplementedException();

	public static Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static IEnumerable<string> ReadLines(string path) => throw new NotImplementedException();

	public static IEnumerable<string> ReadLines(string path, Encoding encoding) => throw new NotImplementedException();

	public static IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName)
		=> throw new NotImplementedException();

	public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
		=> throw new NotImplementedException();

	public static FileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget)
		=> throw new NotImplementedException();

	public static void SetAttributes(SafeFileHandle fileHandle, FileAttributes fileAttributes)
		=> throw new NotImplementedException();

	public static void SetAttributes(string path, FileAttributes fileAttributes)
		=> throw new NotImplementedException();

	public static void SetCreationTime(SafeFileHandle fileHandle, DateTime creationTime)
		=> throw new NotImplementedException();

	public static void SetCreationTime(string path, DateTime creationTime) => throw new NotImplementedException();

	public static void SetCreationTimeUtc(SafeFileHandle fileHandle, DateTime creationTimeUtc)
		=> throw new NotImplementedException();

	public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc) => throw new NotImplementedException();

	public static void SetLastAccessTime(SafeFileHandle fileHandle, DateTime lastAccessTime)
		=> throw new NotImplementedException();

	public static void SetLastAccessTime(string path, DateTime lastAccessTime) => throw new NotImplementedException();

	public static void SetLastAccessTimeUtc(SafeFileHandle fileHandle, DateTime lastAccessTimeUtc)
		=> throw new NotImplementedException();

	public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		=> throw new NotImplementedException();

	public static void SetLastWriteTime(SafeFileHandle fileHandle, DateTime lastWriteTime)
		=> throw new NotImplementedException();

	public static void SetLastWriteTime(string path, DateTime lastWriteTime) => throw new NotImplementedException();

	public static void SetLastWriteTimeUtc(SafeFileHandle fileHandle, DateTime lastWriteTimeUtc)
		=> throw new NotImplementedException();

	public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		=> throw new NotImplementedException();

	[UnsupportedOSPlatform("windows")]
	public static void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode)
		=> throw new NotImplementedException();

	[UnsupportedOSPlatform("windows")]
	public static void SetUnixFileMode(string path, UnixFileMode mode) => throw new NotImplementedException();

	public static void WriteAllBytes(string path, byte[] bytes) => Impl.File.WriteAllBytes(path, bytes);

	public static Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static void WriteAllLines(string path, IEnumerable<string> contents) => throw new NotImplementedException();

	public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
		=> throw new NotImplementedException();

	public static void WriteAllLines(string path, string[] contents) => Impl.File.WriteAllLines(path, contents);

	public static void WriteAllLines(string path, string[] contents, Encoding encoding)
		=> throw new NotImplementedException();

	public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static void WriteAllText(string path, string? contents) => Impl.File.WriteAllText(path, contents);

	public static void WriteAllText(string path, string? contents, Encoding encoding)
		=> throw new NotImplementedException();

	public static Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();

	public static Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
		=> throw new NotImplementedException();
}
