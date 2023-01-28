// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.IO;

/// <summary>
/// Implementation of the "System.IO.Path" class
/// </summary>
public static class Path
{
	/// <summary>
	/// Directory Separator
	/// </summary>
	public static readonly char DirectorySeparatorChar = '/';

	/// <summary>
	/// Alterative Directory Separator
	/// </summary>
	public static readonly char AltDirectorySeparatorChar = '\\';

	public static string Combine(string path1, string path2)
	{
		return path1 + DirectorySeparatorChar + path2;
	}
}
