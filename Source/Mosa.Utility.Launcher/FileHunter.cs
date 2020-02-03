// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class FileHunter
	{
		public string SourceDirectory { get; set; }

		public FileHunter(string sourceDirectory)
		{
			SourceDirectory = sourceDirectory;
		}

		public FileInfo HuntFile(string filename)
		{
			var directory = HuntDirectory(filename);

			if (directory == null)
				return null;

			var full = Path.Combine(directory, filename);

			var file = new FileInfo(full);

			return file;
		}

		private bool Check(string directory, string filename)
		{
			return File.Exists(Path.GetFullPath(Path.Combine(directory, filename)));
		}

		public string HuntDirectory(string filename)
		{
			// Only hunt if it's not in any of the path directories already, or the source directory

			// let's check the source directory

			if (Check(SourceDirectory, filename))
				return SourceDirectory;

			// check current directory
			if (Check(Environment.CurrentDirectory, filename))
			{
				return Environment.CurrentDirectory;
			}

			// check within packages directory in 1 or 2 directories back
			// this is how VS organizes projects and packages

			var result = SearchSubdirectories(Path.Combine(SourceDirectory, "..", "packages"), filename);

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(SourceDirectory, "..", "..", "packages"), filename);

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "packages"), filename);

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "..", "packages"), filename);

			return result;
		}

		private static string SearchSubdirectories(string path, string filename)
		{
			if (Directory.Exists(path))
			{
				var result = Directory.GetFiles(path, filename, SearchOption.AllDirectories);

				if (result?.Length >= 1)
				{
					return Path.GetDirectoryName(result[0]);
				}
			}

			return null;
		}
	}
}
