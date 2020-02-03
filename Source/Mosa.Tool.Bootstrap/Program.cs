// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Mosa.Tool.Bootstrap
{
	/// <summary>
	/// Class containing the entry point of the program.
	/// </summary>
	internal static class Program
	{
		private static readonly string LauncherFileName = "Mosa.Tool.Launcher.exe";

		/// <summary>
		/// Main entry point for the compiler.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		internal static void Main(string[] args)
		{
			var location = FindLauncher();

			if (location == null)
			{
				var status = new StatusForm("Unable to find Mosa.Launcher.Tool.exe!");

				Application.Run(status);
			}
			else
			{
				var sb = new StringBuilder();

				foreach (var arg in args)
				{
					sb.Append(arg);
					sb.Append(' ');
				}

				var start = new ProcessStartInfo
				{
					FileName = location,
					Arguments = sb.ToString(),
					UseShellExecute = false,
					CreateNoWindow = true,
				};

				Process.Start(start);
			}

			return;
		}

		internal static string FindLauncher()
		{
			if (CheckDirectory(Environment.CurrentDirectory))
			{
				return Path.Combine(Environment.CurrentDirectory, LauncherFileName);
			}

			// check within packages directory in 1 or 2 directories back
			// this is how VS organizes projects and packages

			var result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "packages"));

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "..", "packages"));

			return result;
		}

		internal static bool CheckDirectory(string directory)
		{
			return File.Exists(Path.Combine(directory, LauncherFileName));
		}

		internal static string SearchSubdirectories(string path)
		{
			if (Directory.Exists(path))
			{
				var result = Directory.GetFiles(path, LauncherFileName, SearchOption.AllDirectories);

				if (result?.Length >= 1)
				{
					return result[0];
				}
			}

			return null;
		}
	}
}
