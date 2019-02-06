// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class AppLocations
	{
		public string NDISASM { get; set; }

		public string QEMU { get; set; }

		public string QEMUBIOSDirectory { get; set; }

		public string QEMUImg { get; set; }

		public string BOCHS { get; set; }

		public string BOCHSBIOSDirectory { get; set; }

		public string VMwarePlayer { get; set; }

		public string Mkisofs { get; set; }

		public string MsBuild { get; set; }

		public string GDB { get; set; }

		// TODO: The following methods should be placed in another class, possibly as a class extension

		public void FindApplications()
		{
			if (string.IsNullOrEmpty(QEMU))
			{
				// find QEMU executable
				QEMU = TryFind(
					new string[] { "qemu-system-i386.exe", "qemu-system-i386" },
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

			if (!string.IsNullOrEmpty(QEMU))
			{
				if (string.IsNullOrEmpty(QEMUImg))
				{
					QEMUImg = TryFind(
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

				if (string.IsNullOrEmpty(QEMUBIOSDirectory))
				{
					QEMUBIOSDirectory = Path.GetDirectoryName(
						TryFind(
							new string[] { "bios.bin" },
							new string[] {
								Path.GetDirectoryName(QEMU),
								Path.Combine(Path.GetDirectoryName(QEMU), "bios"),
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
			}

			if (string.IsNullOrEmpty(NDISASM))
			{
				// find NDISMASM
				NDISASM = TryFind(
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
			if (string.IsNullOrEmpty(BOCHS))
			{
				// find BOCHS
				BOCHS = TryFind(
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

			if (string.IsNullOrEmpty(BOCHSBIOSDirectory) && !string.IsNullOrEmpty(BOCHS))
			{
				var dir = TryFind(
						new string[] { "BIOS-bochs-latest" },
						new string[] {
											Path.GetDirectoryName(BOCHS),
											"/usr/share/bochs/"
						},
						new string[]
						{
											@"..\packages",
											@"..\..\packages",
						}
					);

				if (!string.IsNullOrEmpty(dir))
					BOCHSBIOSDirectory = Path.GetDirectoryName(dir);
			}

			if (string.IsNullOrEmpty(VMwarePlayer))
			{
				// find vmware player
				VMwarePlayer = TryFind(
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

			if (string.IsNullOrEmpty(Mkisofs))
			{
				// find mkisofs
				Mkisofs = TryFind(
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

			if (string.IsNullOrEmpty(GDB))
			{
				// find GDB
				GDB = TryFind(
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

			if (string.IsNullOrEmpty(MsBuild))
			{
				MsBuild = TryFind(
					new string[] { "msbuild.exe", "msbuild" },
					new string[] {
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\amd64"),
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\amd64"),
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin"),
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64"),
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin"),
						CombineParameterAndDirectory("SYSTEMROOT", @"Microsoft.NET\Framework64\v4.0.30319"),
						CombineParameterAndDirectory("SYSTEMROOT", @"Microsoft.NET\Framework\v4.0.30319")
					}
				);
				if (string.IsNullOrEmpty(MsBuild))
					MsBuild = "msbuild";
			}
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
