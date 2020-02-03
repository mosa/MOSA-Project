// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public static class AppLocationsSettings
	{
		public static Settings GetAppLocations()
		{
			var settings = new Settings();

			Set(settings, "AppLocation.Qemu", FindQemu());
			Set(settings, "AppLocation.QemuBIOS", FindQemuBIOS(settings.GetValue("AppLocation.Qemu")));
			Set(settings, "AppLocation.QemuImg", FindQemuImg());
			Set(settings, "AppLocation.Bochs", FindBochs());
			Set(settings, "AppLocation.VmwarePlayer", FindVmwarePlayer());
			Set(settings, "AppLocation.Ndisasm", FindNdisasm());
			Set(settings, "AppLocation.Mkisofs", FindMkisofs());
			Set(settings, "AppLocation.GDB", FindGDB());

			return settings;
		}

		private static string FindQemu()
		{
			return TryFind(
				new string[] { "qemu-system-i386.exe", "qemu-system-i386", "qemu-system-x86_64", "qemu-system-x86_64.exe", },
				new string[] {
					CombineParameterAndDirectory("MOSA",@"Tools\QEMU"),
					CombineParameterAndDirectory("MOSA","QEMU"),
					@"..\Tools\QEMU",
					@"Tools\QEMU",
					CombineParameterAndDirectory("ProgramFiles","qemu"),
					CombineParameterAndDirectory("ProgramFiles(x86)","qemu"),
					"/bin",
					"/usr/bin"
				},
				new string[]
				{
					@"..\packages",
					@"..\..\packages",
				}
			);
		}

		private static string FindGDB()
		{
			return TryFind(
				new string[] { "gdb.exe" },
				new string[] {
					CombineParameterAndDirectory("MOSA",@"Tools\gdb"),
					CombineParameterAndDirectory("MOSA","gdb"),
					@"..\Tools\gdb",
					@"Tools\gdb",
					@"C:\cygwin64\bin",
					@"C:\mingw64\bin",
					@"C:\cygwin\bin",
					@"C:\mingw32\bin",
					@"C:\mingw\bin",
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Mosa-Project\Tools"),
					"/bin"
				},
				new string[]
				{
					@"..\packages",
					@"..\..\packages",
				}
			);
		}

		private static string FindMkisofs()
		{
			return TryFind(
				new string[] { "mkisofs.exe", "mkisofs" },
				new string[] {
					CombineParameterAndDirectory("ProgramFiles",@"VMware\VMware Player"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"VMware\VMware Player"),
					CombineParameterAndDirectory("ProgramFiles","cdrtools"),
					CombineParameterAndDirectory("ProgramFiles(x86)","cdrtools"),
					CombineParameterAndDirectory("MOSA",@"Tools\mkisofs"),
					CombineParameterAndDirectory("MOSA","mkisofs"),
					@"..\Tools\mkisofs",
					@"Tools\mkisofs",
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Mosa-Project\Tools"),
					"/bin"
				},
				new string[]
				{
					@"..\packages",
					@"..\..\packages",
				}
			);
		}

		private static string FindNdisasm()
		{
			return TryFind(
				new string[] { "ndisasm.exe", "ndisasm" },
				new string[] {
					CombineParameterAndDirectory("MOSA",@"Tools\ndisasm"),
					CombineParameterAndDirectory("MOSA","ndisasm"),
					@"..\Tools\ndisasm",
					@"Tools\ndisasm",
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Mosa-Project\Tools"),
					"/bin"
				},
				new string[]
				{
					@"..\packages",
					@"..\..\packages",
				}
			);
		}

		private static string FindVmwarePlayer()
		{
			return TryFind(
				new string[] { "vmplayer.exe", "vmplayer" },
				new string[] {
					CombineParameterAndDirectory("ProgramFiles",@"VMware\VMware Player"),
					CombineParameterAndDirectory("ProgramFiles",@"VMware\VMware Workstation"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"VMware\VMware Player"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"VMware\VMware Workstation"),
					"/bin"
				}
			);
		}

		private static string FindBochs()
		{
			return TryFind(
				new string[] { "bochs.exe", "bochs" },
				new string[] {
					CombineParameterAndDirectory("ProgramFiles","Bochs-2.6.9"),
					CombineParameterAndDirectory("ProgramFiles(x86)","Bochs-2.6.9"),
					CombineParameterAndDirectory("ProgramFiles","Bochs-2.6.8"),
					CombineParameterAndDirectory("ProgramFiles(x86)","Bochs-2.6.8"),
					CombineParameterAndDirectory("ProgramFiles","Bochs-2.6.5"),
					CombineParameterAndDirectory("ProgramFiles(x86)","Bochs-2.6.5"),
					CombineParameterAndDirectory("ProgramFiles","Bochs-2.6.2"),
					CombineParameterAndDirectory("ProgramFiles(x86)","Bochs-2.6.2"),
					CombineParameterAndDirectory("MOSA",@"Tools\Bochs"),
					CombineParameterAndDirectory("MOSA","Bochs"),
					@"..\Tools\Bochs",
					@"Tools\Bochs",
					"/bin",
					"/usr/bin"
				},
				new string[]
				{
					@"..\packages",
					@"..\..\packages",
				}
			);
		}

		private static string FindQemuImg()
		{
			return TryFind(
				new string[] { "qemu-img.exe", "qemu-img" },
				new string[]
				{
					CombineParameterAndDirectory("MOSA", @"Tools\QEMU"),
					CombineParameterAndDirectory("MOSA", "QEMU"),
					@"..\Tools\QEMU",
					@"Tools\QEMU",
					CombineParameterAndDirectory("ProgramFiles", "qemu"),
					CombineParameterAndDirectory("ProgramFiles(x86)", "qemu"),
					"/bin"
				},
				new string[]
				{
					@"..\packages",
					@"..\..\packages",
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
						"/usr/share/qemu",
						"/usr/share/seabios"
					},
					new string[]
					{
						@"..\packages",
						@"..\..\packages",
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

		private static string CombineParameterAndDirectory(string parameter, string subdirectory)
		{
			var variable = Environment.GetEnvironmentVariable(parameter);

			if (variable == null)
				return null;

			return Path.Combine(variable, subdirectory);
		}

		private static string TryFind(IList<string> files, IList<string> directories, IList<string> searchdirectories = null)
		{
			if (searchdirectories != null)
			{
				foreach (var directory in searchdirectories)
				{
					foreach (var file in files)
					{
						var location = SearchSubdirectories(directory, file);

						if (location != null)
						{
							return location;
						}
					}
				}
			}

			foreach (var directory in directories)
			{
				foreach (var file in files)
				{
					if (TryFind(file, directory, out string location))
					{
						return location;
					}
				}
			}

			return string.Empty;
		}

		private static bool TryFind(string file, string directory, out string location)
		{
			location = string.Empty;

			if (directory == null)
				return false;

			string combine = Path.Combine(directory, file);

			if (File.Exists(combine))
			{
				location = combine;
				return true;
			}

			return false;
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
}
