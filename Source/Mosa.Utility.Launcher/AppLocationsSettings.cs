// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Common.Configuration;

namespace Mosa.Utility.Launcher;

public static class AppLocationsSettings
{
	private static readonly string MosaEnvironmentalVariable = Environment.GetEnvironmentVariable("MOSA");
	private static readonly string ProgramFiles86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
	private static readonly string ProgramFiles = Environment.GetEnvironmentVariable("ProgramFiles");
	private static readonly string UserProfile = Environment.GetEnvironmentVariable("UserProfile");
	private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
	private static readonly string AppDirectory = AppDomain.CurrentDomain.BaseDirectory;
	private static readonly string MosaTools = @"%ProgramFiles(x86)%\MOSA-Project\Tools";

	public static Settings GetAppLocations()
	{
		var settings = new Settings();

		Set(settings, "AppLocation.Qemu", FindQemu());
		Set(settings, "AppLocation.QemuBIOS", FindQemuBIOS(settings.GetValue("AppLocation.Qemu")));
		Set(settings, "AppLocation.QemuImg", FindQemuImg());
		Set(settings, "AppLocation.Bochs", FindBochs());
		Set(settings, "AppLocation.VmwarePlayer", FindVmwarePlayer());
		Set(settings, "AppLocation.VmwareWorkstation", FindVmwareWorkstation());
		Set(settings, "AppLocation.VirtualBox", FindVirtualBox());
		Set(settings, "AppLocation.Ndisasm", FindNdisasm());
		Set(settings, "AppLocation.Mkisofs", FindMkisofs());
		Set(settings, "AppLocation.GDB", FindGDB());

		return settings;
	}

	private static string FindQemu()
	{
		return TryFind(
			new string[] { /*"qemu-system-x86_64", "qemu-system-x86_64.exe", */"qemu-system-i386.exe", "qemu-system-i386" },
			new string[] {
				@"%MOSA%\Tools\QEMU",
				@"%CURRENT%\..\Tools\QEMU",
				@"%CURRENT%\Tools\QEMU",
				@"%APPDIR%\Tools\QEMU",
				@"%APPDIR%\..\Tools\QEMU",
				@"%ProgramFiles%\qemu",
				@"%ProgramFiles(x86)%\qemu",
				@"%MOSATOOLS%\QEMU",
				"/bin",
				"/usr/bin"
			}
		);
	}

	private static string FindGDB()
	{
		return TryFind(
			new string[] { "gdb.exe" },
			new string[] {
				@"%MOSA%\Tools\gdb",
				@"%CURRENT%\..\Tools\gdb",
				@"%CURRENT%\Tools\gdb",
				@"%APPDIR%\Tools\gdb",
				@"%APPDIR%\..\Tools\gdb",
				@"%MOSATOOLS%\gdb",

				@"C:\cygwin64\bin",
				@"C:\mingw64\bin",
				@"C:\cygwin\bin",
				@"C:\mingw\bin",
				@"C:\mingw32\bin",

				"/bin"
			}
		);
	}

	private static string FindMkisofs()
	{
		return TryFind(
			new string[] { "mkisofs.exe", "mkisofs" },
			new string[] {
				@"%MOSA%\Tools\mkisofs",
				@"%CURRENT%\..\Tools\mkisofs",
				@"%CURRENT%\Tools\mkisofs",
				@"%APPDIR%\Tools\mkisofs",
				@"%APPDIR%\..\Tools\mkisofs",
				@"%MOSATOOLS%\mkisofs",

				@"%ProgramFiles%\VMware\VMware Player",
				@"%ProgramFiles(x86)%\VMware\VMware Player",
				@"%ProgramFiles%\cdrtools",
				@"%ProgramFiles(x86)%\cdrtools",
				"/bin"
			}
		);
	}

	private static string FindNdisasm()
	{
		return TryFind(
			new string[] { "ndisasm.exe", "ndisasm" },
			new string[] {
				@"%MOSA%\Tools\ndisasm",
				@"%CURRENT%\..\Tools\ndisasm",
				@"%CURRENT%\Tools\ndisasm",
				@"%APPDIR%\Tools\ndisasm",
				@"%APPDIR%\..\Tools\ndisasm",
				@"%MOSATOOLS%\ndisasm",
				"/bin"
			}
		);
	}

	private static string FindVmwarePlayer()
	{
		return TryFind(
			new string[] { "vmplayer.exe", "vmplayer" },
			new string[] {
				@"%ProgramFiles%\VMware\VMware Player",
				@"%ProgramFiles(x86)%\VMware\VMware Player",
				"/bin"
			}
		);
	}

	private static string FindVmwareWorkstation()
	{
		return TryFind(
			new string[] { "vmware.exe", "vmware" },
			new string[] {
				@"%ProgramFiles%\VMware\VMware Workstation",
				@"%ProgramFiles(x86)%\VMware\VMware Workstation",
				"/bin"
			}
		);
	}

	private static string FindVirtualBox()
	{
		return TryFind(
			new string[] { "VBoxManage.exe", "VBoxManage" },
			new string[] {
				@"%ProgramFiles%\Oracle",
				@"%ProgramFiles(x86)%\Oracle",
				"/bin"
			}
		);
	}

	private static string FindBochs()
	{
		return TryFind(
			new string[] { "bochs.exe", "bochs" },
			new string[] {
				@"%ProgramFiles%\Bochs-2.6.9",
				@"%ProgramFiles(x86)%\Bochs-2.6.9",
				@"%ProgramFiles%\Bochs-2.6.8",
				@"%ProgramFiles(x86)%\Bochs-2.6.8",
				@"%ProgramFiles%\Bochs-2.6.5",
				@"%ProgramFiles(x86)%\Bochs-2.6.5",
				@"%ProgramFiles%\Bochs-2.6.2",
				@"%ProgramFiles(x86)%\Bochs-2.6.2",
				@"%ProgramFiles%\Bochs-2.7",
				@"%ProgramFiles(x86)%\Bochs-2.7",

				@"%MOSA%\Tools\Bochs",
				@"%CURRENT%\..\Tools\Bochs",
				@"%CURRENT%\Tools\Bochs",
				@"%APPDIR%\Tools\Bochs",
				@"%APPDIR%\..\Tools\Bochs",
				@"%MOSATOOLS%\Bochs",

				"/bin",
				"/usr/bin"
			}
		);
	}

	private static string FindQemuImg()
	{
		return TryFind(
			new string[] { "qemu-img.exe", "qemu-img" },
			new string[]
			{
				@"%MOSA%\Tools\QEMU",
				@"%CURRENT%\..\Tools\QEMU",
				@"%CURRENT%\Tools\QEMU",
				@"%APPDIR%\Tools\QEMU",
				@"%APPDIR%\..\Tools\QEMU",
				@"%MOSATOOLS%\QEMU",

				@"%ProgramFiles%\qemu",
				@"%ProgramFiles(x86)%\qemu",
				@"%ProgramFiles(x86)%\Mosa-Project\Tools\qemu",
				"/bin"
			}
		);
	}

	private static string FindQemuBIOS(string qemu)
	{
		if (qemu == null)
			return null;

		return Path.GetDirectoryName(
			TryFind(
				new string[] { "bios.bin" },
				new string[] {
					Path.GetDirectoryName(qemu),
					Path.Combine(Path.GetDirectoryName(qemu), "bios"),

					@"%MOSATOOLS%\qemu",
					@"%MOSATOOLS%\qemu\bios",

					"/usr/share/qemu",
					"/usr/share/seabios"
				}
			)
		);
	}

	private static void Set(Settings settings, string name, string value)
	{
		if (value == null)
			return;

		var property = settings.CreateProperty(name);
		property.Value = value;
	}

	public static string ReplaceWithParameters(string directory)
	{
		if (!string.IsNullOrWhiteSpace(MosaEnvironmentalVariable))
			directory = directory.Replace("%MOSA%", MosaEnvironmentalVariable);

		if (!string.IsNullOrWhiteSpace(MosaTools))
			directory = directory.Replace("%MOSATOOLS%", MosaTools);

		if (!string.IsNullOrWhiteSpace(ProgramFiles))
			directory = directory.Replace("%PROGRAMFILES%", ProgramFiles);

		if (!string.IsNullOrWhiteSpace(ProgramFiles86))
			directory = directory.Replace("%PROGRAMFILES(x86)%", ProgramFiles86);

		if (!string.IsNullOrWhiteSpace(ProgramFiles))
			directory = directory.Replace("%ProgramFiles%", ProgramFiles);

		if (!string.IsNullOrWhiteSpace(ProgramFiles86))
			directory = directory.Replace("%ProgramFiles(x86)%", ProgramFiles86);

		if (!string.IsNullOrWhiteSpace(UserProfile))
			directory = directory.Replace("%USERPROFILE%", UserProfile);

		if (!string.IsNullOrWhiteSpace(CurrentDirectory))
			directory = directory.Replace("%CURRENT%", CurrentDirectory);

		if (!string.IsNullOrWhiteSpace(AppDirectory))
			directory = directory.Replace("%APPDIR%", AppDirectory);

		if (directory.Contains("%"))
			return null;

		return directory;
	}

	private static string TryFind(IList<string> files, IList<string> searchdirectories = null)
	{
		if (searchdirectories != null)
		{
			foreach (var directory in searchdirectories)
			{
				var dir = ReplaceWithParameters(directory);

				if (dir == null)
					continue;

				foreach (var file in files)
				{
					var location = SearchSubdirectories(dir, file);

					if (location != null)
					{
						return location;
					}
				}
			}
		}

		return null;
	}

	private static string SearchSubdirectories(string path, string filename)
	{
		if (Directory.Exists(path))
		{
			var result = Directory.GetFiles(path, filename, SearchOption.AllDirectories);

			if (result?.Length >= 1)
			{
				return result[0];
			}
		}

		return null;
	}
}
