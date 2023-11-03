// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class FileFinder
{
	public static string Find(string file, IList<string> searchpaths)
	{
		if (File.Exists(file))
			return file;

		foreach (var path in searchpaths)
		{
			var fullpath = Path.Combine(path, file);

			if (File.Exists(fullpath))
			{
				return fullpath;
			}
		}

		return null;
	}

	public static string SearchSubdirectories(string path, string filename)
	{
		if (!Directory.Exists(path))
			return null;

		var enumerationOptions = new EnumerationOptions()
		{
			IgnoreInaccessible = true,
			MaxRecursionDepth = 5,
			RecurseSubdirectories = true,
			MatchType = MatchType.Simple,
			MatchCasing = MatchCasing.PlatformDefault,
			ReturnSpecialDirectories = false,
			AttributesToSkip = FileAttributes.ReparsePoint | FileAttributes.Device
		};

		var result = Directory.GetFiles(path, filename, enumerationOptions);

		if (result?.Length >= 1)
		{
			return Path.GetDirectoryName(result[0]);
		}

		return null;
	}
}
