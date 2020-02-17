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
		private static readonly string InstalledMosaTool = @"%programfiles(x86)%\MOSA-Project\tools";
		private static readonly string LauncherFileName = "Mosa.Tool.Launcher.exe";

		private static readonly string GlobalPackageDirectory = @"%userprofile%\.nuget\packages";
		private static readonly string ToolsPackage = "mosa.tools.package";

		private static readonly string Korlib = "mscorlib.dll";

		internal static void Main(string[] args)
		{
			var location = FindLauncher(args);

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
					Arguments = sb.ToString().Trim(),
					UseShellExecute = false,
					CreateNoWindow = true,
				};

				Process.Start(start);
			}

			return;
		}

		internal static string FindLauncher(string[] args)
		{
			var location = FindLauncherInCurrentDirectory();

			if (location != null)
				return null;

			var targetVersion = GetIdealFileVersion(Environment.CurrentDirectory);

			if (targetVersion == null)
			{
				targetVersion = GetIdealFileVersion(args[args.Length - 1]);
			}

			location = FindLauncherInGlobalCatalog(targetVersion);

			if (location != null)
				return location;

			location = FindInstalledLauncher();

			return location;
		}

		internal static string FindLauncherInCurrentDirectory()
		{
			return FindLauncher(InstalledMosaTool);
		}

		internal static string FindInstalledLauncher()
		{
			return FindLauncher(Environment.CurrentDirectory);
		}

		internal static string FindLauncherExecutionPath()
		{
			return FindLauncher(Application.ExecutablePath);
		}

		internal static string FindLauncher(string directory)
		{
			if (directory == null)
				return null;

			var location = Path.Combine(directory, LauncherFileName);

			if (File.Exists(location))
				return location;

			return null;
		}

		internal static string FindLauncherInGlobalCatalog(FileVersionInfo targetVersion)
		{
			var globalPackageDirectory = Path.Combine(GlobalPackageDirectory, ToolsPackage);

			if (!Directory.Exists(globalPackageDirectory))
				return null;

			string bestLocation = null;
			FileVersionInfo bestVerion = null;

			foreach (var directory in Directory.GetDirectories(globalPackageDirectory))
			{
				var location = Path.Combine(directory, "tools", LauncherFileName);

				if (File.Exists(location))
				{
					if (targetVersion == null)
						return location;

					var locationVersion = FileVersionInfo.GetVersionInfo(location);

					if (CompareTo(locationVersion, targetVersion) == 0)
						return location;

					if (bestLocation == null)
					{
						bestLocation = location;
						bestVerion = locationVersion;
					}
					else if (CompareTo(locationVersion, bestVerion) == 1)
					{
						bestLocation = location;
						bestVerion = locationVersion;
					}
				}
			}

			return bestLocation;
		}

		internal static FileVersionInfo GetIdealFileVersion(string directory)
		{
			var location = Path.Combine(directory, Korlib);

			if (!File.Exists(location))
				return null;

			return FileVersionInfo.GetVersionInfo(location);
		}

		private static int CompareTo(FileVersionInfo older, FileVersionInfo newer)
		{
			if (newer.ProductMajorPart > older.ProductMajorPart) return 1;
			if (newer.ProductMajorPart < older.ProductMajorPart) return -1;

			if (newer.ProductMinorPart > older.ProductMinorPart) return 1;
			if (newer.ProductMinorPart < older.ProductMinorPart) return -1;

			if (newer.ProductBuildPart > older.ProductBuildPart) return 1;
			if (newer.ProductBuildPart < older.ProductBuildPart) return -1;

			if (newer.ProductPrivatePart == older.ProductPrivatePart)
				return 0;

			return newer.ProductPrivatePart > older.ProductPrivatePart ? 1 : -1;
		}
	}
}
