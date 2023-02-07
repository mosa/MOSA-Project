// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Controls.ApplicationLifetimes;

namespace Mosa.Tool.Bootstrap;

public static class LauncherHunter
{
	private const string InstalledMosaTool = @"%ProgramFiles(x86)%\MOSA-Project\bin";
	private const string LauncherFileName = "Mosa.Tool.Launcher.dll";

	private const string GlobalPackageDirectory = @".nuget\packages";
	private const string ToolsPackage = "mosa.tools.package";

	private const string Korlib = "System.Runtime.dll";

	public static void Hunt(IClassicDesktopStyleApplicationLifetime desktop, string[] args)
	{
		if (args.Length == 0)
		{
			var window = new MainWindow();
			window.SetStatus("No arguments specified!");
			desktop.MainWindow = window;
			return;
		}

		var location = FindLauncher(args[0]);

		if (location == null)
		{
			var window = new MainWindow();
			window.SetStatus("Unable to find Mosa.Tool.Launcher.dll!");
			desktop.MainWindow = window;
			return;
		}

		Process.Start(new ProcessStartInfo
		{
			FileName = "dotnet",
			Arguments = $"{location} {string.Join(" ", args)}",
			UseShellExecute = false,
			CreateNoWindow = true,
			WorkingDirectory = Environment.CurrentDirectory
		})?.WaitForExit();

		Environment.Exit(0);
	}

	private static string? FindLauncher(string source)
	{
		var location = FindLauncherInCurrentDirectory();

		if (location != null)
			return location;

		var targetVersion = GetIdealFileVersion(Environment.CurrentDirectory) ?? GetIdealFileVersion(source);

		location = FindLauncherInGlobalCatalog(targetVersion);

		return location ?? FindInstalledLauncher();
	}

	private static string? FindInstalledLauncher()
	{
		return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? null : CheckLauncher(InstalledMosaTool.Replace("%ProgramFiles(x86)%", Environment.GetEnvironmentVariable("ProgramFiles(x86)")));
	}

	private static string? FindLauncherInCurrentDirectory()
	{
		return CheckLauncher(Environment.CurrentDirectory);
	}

	private static string? CheckLauncher(string? directory)
	{
		if (directory == null)
			return null;

		var location = Path.Combine(directory, LauncherFileName);

		return File.Exists(location) ? location : null;
	}

	private static string? FindLauncherInGlobalCatalog(FileVersionInfo? targetVersion)
	{
		var userProfile = Environment.GetEnvironmentVariable("userprofile");
		var globalPackageDirectory = Path.Combine(userProfile!, GlobalPackageDirectory, ToolsPackage);

		if (!Directory.Exists(globalPackageDirectory))
			return null;

		string? bestLocation = null;
		FileVersionInfo? bestVersion = null;

		foreach (var directory in Directory.GetDirectories(globalPackageDirectory))
		{
			var location = Path.Combine(directory, "tools", LauncherFileName);

			if (!File.Exists(location))
				continue;

			if (targetVersion == null)
				return location;

			var locationVersion = FileVersionInfo.GetVersionInfo(location);

			if (CompareTo(locationVersion, targetVersion) == 0)
				return location;

			if (bestLocation == null)
			{
				bestLocation = location;
				bestVersion = locationVersion;
			}
			else if (CompareTo(locationVersion, bestVersion) == 1)
			{
				bestLocation = location;
				bestVersion = locationVersion;
			}
		}

		return bestLocation;
	}

	private static FileVersionInfo? GetIdealFileVersion(string directory)
	{
		var location = Path.Combine(directory, Korlib);

		return !File.Exists(location) ? null : FileVersionInfo.GetVersionInfo(location);
	}

	private static int CompareTo(FileVersionInfo? older, FileVersionInfo? newer)
	{
		if (newer == null || older == null)
			return 0;

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
