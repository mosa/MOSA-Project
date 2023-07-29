// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Mosa.Utility.Configuration;

public static class AppLocationsSettings
{
	private static readonly string ProgramFiles86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
	private static readonly string ProgramFiles = Environment.GetEnvironmentVariable("ProgramFiles");
	private static readonly string UserProfile = Environment.GetEnvironmentVariable("UserProfile");
	private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
	private static readonly string AppDirectory = AppDomain.CurrentDomain.BaseDirectory;

	private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
	private static readonly bool IsOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

	private static readonly string[] LinuxDirectories = new string[] { "/bin", "/usr/bin", "/opt/homebrew/bin", "/usr/local/bin/", "/usr/local/Cellar" };

	public static void GetAppLocations(MosaSettings mosaSettings)
	{
		mosaSettings.QEMUApp = FindQemu();
		mosaSettings.QEMUBios = FindQemuBIOS();
		mosaSettings.QEMUEdk2X86 = FindQemuEDK2X86();
		mosaSettings.QEMUEdk2X64 = FindQemuEDK2X64();
		mosaSettings.QEMUEdk2ARM = FindQemuEDK2ARM();
		mosaSettings.QemuImgApp = FindQemuImg();
		mosaSettings.BochsApp = FindBochs();
		mosaSettings.VmwarePlayerApp = FindVmwarePlayer();
		mosaSettings.VmwareWorkstationApp = FindVmwareWorkstation();
		mosaSettings.VirtualBoxApp = FindVirtualBox();
		mosaSettings.NdisasmApp = FindNdisasm();
		mosaSettings.MkisofsApp = FindMkisofs();
		mosaSettings.GDBApp = FindGDB();
		mosaSettings.GraphwizApp = FindGraphwiz();
	}

	private static string FindQemu()
	{
		return
			IsWindows ? TryFind("qemu-system-i386.exe",
				new string[] {
					@"%CURRENT%\qemu",
					@"%CURRENT%\..\Tools\qemu",
					@"%CURRENT%\Tools\qemu",

					@"%APPDIR%\qemu",
					@"%APPDIR%\Tools\qemu",
					@"%APPDIR%\..\Tools\qemu",

					@"%ProgramFiles%\qemu",
					@"%ProgramFiles(x86)%\qemu",
				})
			: TryFind("qemu-system-i386", LinuxDirectories);
	}

	private static string FindGDB()
	{
		return
			IsWindows ? TryFind("gdb.exe",
				new string[] {
					@"%CURRENT%\..\Tools\gdb",
					@"%CURRENT%\Tools\gdb",

					@"%APPDIR%\Tools\gdb",
					@"%APPDIR%\..\Tools\gdb",

					@"C:\cygwin64\bin",
					@"C:\mingw64\bin",
					@"C:\cygwin\bin",
					@"C:\mingw\bin",
					@"C:\mingw32\bin",
				})
			: TryFind("gdb", LinuxDirectories);
	}

	private static string FindMkisofs()
	{
		return
			IsWindows ? TryFind("mkisofs.exe",
				new string[] {
					@"%CURRENT%\..\Tools\mkisofs",
					@"%CURRENT%\Tools\mkisofs",

					@"%APPDIR%\Tools\mkisofs",
					@"%APPDIR%\..\Tools\mkisofs",

					@"%ProgramFiles%\VMware\VMware Player",
					@"%ProgramFiles(x86)%\VMware\VMware Player",
					@"%ProgramFiles%\cdrtools",
					@"%ProgramFiles(x86)%\cdrtools",
				})
			: TryFind("mkisofs", LinuxDirectories);
	}

	private static string FindNdisasm()
	{
		return
			IsWindows ? TryFind("ndisasm.exe",
				new string[] {
					@"%CURRENT%\..\Tools\ndisasm",
					@"%CURRENT%\Tools\ndisasm",

					@"%APPDIR%\Tools\ndisasm",
					@"%APPDIR%\..\Tools\ndisasm"
				})
			: TryFind("ndisasm", LinuxDirectories);
	}

	private static string FindVmwarePlayer()
	{
		return
			IsWindows ? TryFind("vmplayer.exe",
				new string[] {
					@"%ProgramFiles%\VMware\VMware Player",
					@"%ProgramFiles(x86)%\VMware\VMware Player",
				})
			: TryFind("vmplayer", LinuxDirectories);
	}

	private static string FindVmwareWorkstation()
	{
		return
			IsWindows ? TryFind("vmware.exe",
				new string[] {
					@"%ProgramFiles%\VMware\VMware Workstation",
					@"%ProgramFiles(x86)%\VMware\VMware Workstation",
				})
			: TryFind("vmware", LinuxDirectories);
	}

	private static string FindVirtualBox()
	{
		return
			IsWindows ? TryFind("VBoxManage.exe",
				new string[] {
					@"%ProgramFiles%\Oracle",
					@"%ProgramFiles(x86)%\Oracle",
				})
			: TryFind("VBoxManage", LinuxDirectories);
	}

	private static string FindBochs()
	{
		return
			IsWindows ? TryFind("bochs.exe",
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

					@"%CURRENT%\..\Tools\Bochs",
					@"%CURRENT%\Tools\Bochs",

					@"%APPDIR%\Tools\Bochs",
					@"%APPDIR%\..\Tools\Bochs"
				 })
			: TryFind("bochs", LinuxDirectories);
	}

	private static string FindQemuImg()
	{
		return
			IsWindows ? TryFind("qemu-img.exe",
				new string[]
				{
					@"%CURRENT%\..\Tools\qemu",
					@"%CURRENT%\Tools\qemu",

					@"%APPDIR%\Tools\qemu",
					@"%APPDIR%\..\Tools\qemu",

					@"%ProgramFiles%\qemu",
					@"%ProgramFiles(x86)%\qemu",
					@"%ProgramFiles(x86)%\Mosa-Project\Tools\qemu",
				})
			: TryFind("qemu-img", LinuxDirectories);
	}

	private static string FindQemuBIOS()
	{
		return Path.GetDirectoryName(
			IsWindows ? TryFind("bios.bin",
					new string[] {
						@"%CURRENT%\..\Tools\qemu\share",
						@"%CURRENT%\Tools\qemu\share",

						@"%APPDIR%\Tools\qemu\share",
						@"%APPDIR%\..\Tools\qemu\share",

						@"%ProgramFiles%\qemu",
						@"%ProgramFiles%\qemu\share",
						@"%ProgramFiles(x86)%\qemu",
						@"%ProgramFiles(x86)%\qemu\share",
					})
				: TryFind("bios.bin",
					new string[] {
						"/usr/share/qemu",
						"/usr/share/seabios",
						"/opt/homebrew/share/qemu/"
					})
		);
	}

	private static string FindQemuEDK2X86()
	{
		return
			IsWindows ? TryFind("edk2-i386-code.fd",
				new string[] {
					@"%CURRENT%\..\Tools\qemu\share",
					@"%CURRENT%\Tools\qemu\share",

					@"%APPDIR%\Tools\qemu\share",
					@"%APPDIR%\..\Tools\qemu\share",

					@"%ProgramFiles%\qemu",
					@"%ProgramFiles%\qemu\share",
					@"%ProgramFiles(x86)%\qemu",
					@"%ProgramFiles(x86)%\qemu\share",
				})
			: TryFind("edk2-i386-code.fd",
				new string[] {
					"/usr/share/qemu",
					"/usr/share/ovmf",
					"/usr/share/OVMF",
					"/opt/homebrew/share/qemu/"
				});
	}

	private static string FindQemuEDK2X64()
	{
		return
			IsWindows ? TryFind("edk2-x86_64-code.fd",
				new string[] {
					@"%CURRENT%\..\Tools\qemu\share",
					@"%CURRENT%\Tools\qemu\share",

					@"%APPDIR%\Tools\qemu\share",
					@"%APPDIR%\..\Tools\qemu\share",

					@"%ProgramFiles%\qemu",
					@"%ProgramFiles%\qemu\share",
					@"%ProgramFiles(x86)%\qemu",
					@"%ProgramFiles(x86)%\qemu\share"
				})
			: TryFind("edk2-x86_64-code.fd",
				new string[] {
					"/usr/share/qemu",
					"/usr/share/ovmf",
					"/usr/share/OVMF",
					"/opt/homebrew/share/qemu/"
				});
	}

	private static string FindQemuEDK2ARM()
	{
		return
			IsWindows ? TryFind("edk2-arm-code.fd",
				new string[] {
					@"%CURRENT%\..\Tools\qemu\share",
					@"%CURRENT%\Tools\qemu\share",

					@"%APPDIR%\Tools\qemu\share",
					@"%APPDIR%\..\Tools\qemu\share",

					@"%ProgramFiles%\qemu",
					@"%ProgramFiles%\qemu\share",
					@"%ProgramFiles(x86)%\qemu",
					@"%ProgramFiles(x86)%\qemu\share"
				})
			: IsOSX ? TryFind("edk2-x86_64-code.fd", "/opt/homebrew/bin/qemu-system-i386")
			: TryFind("edk2-x86_64-code.fd",
				new string[] {
					"/usr/share/qemu",
					"/usr/share/ovmf",
					"/usr/share/OVMF",
					"/opt/homebrew/share/qemu/"
				});
	}

	private static string FindGraphwiz()
	{
		return
			IsWindows ? TryFind("dot.exe",
				new string[] {
					@"%ProgramFiles%\Graphviz\bin",
					@"%ProgramFiles(x86)%\Graphviz\bin",
				})
			: TryFind("dot", LinuxDirectories);
	}

	public static string ReplaceWithParameters(string directory)
	{
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

		if (directory.Contains('%'))
			return null;

		return directory;
	}

	private static string TryFind(string file, IList<string> searchdirectories = null)
	{
		if (searchdirectories != null)
		{
			foreach (var directory in searchdirectories)
			{
				var dir = ReplaceWithParameters(directory);

				if (dir == null)
					continue;

				var location = SearchSubdirectories(dir, file);

				if (location != null)
					return location;
			}
		}

		return null;
	}

	private static string TryFind(string file, string searchdirectory)
	{
		var dir = ReplaceWithParameters(searchdirectory);

		if (dir == null)
			return null;

		var location = SearchSubdirectories(dir, file);

		if (location != null)
			return location;

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
