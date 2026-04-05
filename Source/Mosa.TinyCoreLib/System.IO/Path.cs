using System.Diagnostics.CodeAnalysis;

namespace System.IO;

public static class Path
{
	public static readonly char AltDirectorySeparatorChar = Internal.Impl.Path.AltDirectorySeparatorChar;

	public static readonly char DirectorySeparatorChar = Internal.Impl.Path.DirectorySeparatorChar;

	[Obsolete("Path.InvalidPathChars has been deprecated. Use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
	public static readonly char[] InvalidPathChars;

	public static readonly char PathSeparator;

	public static readonly char VolumeSeparatorChar;

	[return: NotNullIfNotNull("path")]
	public static string? ChangeExtension(string? path, string? extension) => throw new NotImplementedException();

	public static string Combine(string path1, string path2)
	{
		var path = path1;
		if (!path.EndsWith(DirectorySeparatorChar))
			path += DirectorySeparatorChar;
		return path + path2;
	}

	public static string Combine(string path1, string path2, string path3) => throw new NotImplementedException();

	public static string Combine(string path1, string path2, string path3, string path4) => throw new NotImplementedException();

	public static string Combine(params string[] paths) => throw new NotImplementedException();

	public static bool EndsInDirectorySeparator(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static bool EndsInDirectorySeparator([NotNullWhen(true)] string? path) => throw new NotImplementedException();

	public static bool Exists([NotNullWhen(true)] string? path) => throw new NotImplementedException();

	public static ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static string? GetDirectoryName(string? path) => throw new NotImplementedException();

	public static ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path) => throw new NotImplementedException();

	[return: NotNullIfNotNull("path")]
	public static string? GetExtension(string? path) => throw new NotImplementedException();

	public static ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path) => throw new NotImplementedException();

	[return: NotNullIfNotNull("path")]
	public static string? GetFileName(string? path) => throw new NotImplementedException();

	public static ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path) => throw new NotImplementedException();

	[return: NotNullIfNotNull("path")]
	public static string? GetFileNameWithoutExtension(string? path) => throw new NotImplementedException();

	public static string GetFullPath(string path) => throw new NotImplementedException();

	public static string GetFullPath(string path, string basePath) => throw new NotImplementedException();

	public static char[] GetInvalidFileNameChars() => throw new NotImplementedException();

	public static char[] GetInvalidPathChars() => throw new NotImplementedException();

	public static ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static string? GetPathRoot(string? path) => throw new NotImplementedException();

	public static string GetRandomFileName() => throw new NotImplementedException();

	public static string GetRelativePath(string relativeTo, string path) => throw new NotImplementedException();

	public static string GetTempFileName() => throw new NotImplementedException();

	public static string GetTempPath() => throw new NotImplementedException();

	public static bool HasExtension(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static bool HasExtension([NotNullWhen(true)] string? path) => throw new NotImplementedException();

	public static bool IsPathFullyQualified(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static bool IsPathFullyQualified(string path) => throw new NotImplementedException();

	public static bool IsPathRooted(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static bool IsPathRooted([NotNullWhen(true)] string? path) => throw new NotImplementedException();

	public static string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2) => throw new NotImplementedException();

	public static string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3) => throw new NotImplementedException();

	public static string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, ReadOnlySpan<char> path4) => throw new NotImplementedException();

	public static string Join(string? path1, string? path2) => throw new NotImplementedException();

	public static string Join(string? path1, string? path2, string? path3) => throw new NotImplementedException();

	public static string Join(string? path1, string? path2, string? path3, string? path4) => throw new NotImplementedException();

	public static string Join(params string?[] paths) => throw new NotImplementedException();

	public static ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path) => throw new NotImplementedException();

	public static string TrimEndingDirectorySeparator(string path) => throw new NotImplementedException();

	public static bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, Span<char> destination, out int charsWritten) => throw new NotImplementedException();

	public static bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination, out int charsWritten) => throw new NotImplementedException();
}
