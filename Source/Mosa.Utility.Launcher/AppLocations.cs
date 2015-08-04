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

		public string VMwarePlayer { get; set; }

		public string mkisofs { get; set; }

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
					CombineParameterAndDirectory("MOSA",@"QEMU"),
					@"..\Tools\QEMU",
					@"Tools\QEMU",
					CombineParameterAndDirectory("ProgramFiles",@"qemu"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"qemu"),
					@"/bin"
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
							CombineParameterAndDirectory("MOSA", @"QEMU"),
							@"..\Tools\QEMU",
							@"Tools\QEMU",
							CombineParameterAndDirectory("ProgramFiles", @"qemu"),
							CombineParameterAndDirectory("ProgramFiles(x86)", @"qemu"),
							@"/bin"
						}
					);
				}

				if (string.IsNullOrEmpty(QEMUBIOSDirectory))
				{
					QEMUBIOSDirectory = Path.GetDirectoryName(
						TryFind(
							new string[] { "bios.bin" },
							new string[]
							{
								Path.GetDirectoryName(QEMU),
								Path.Combine(Path.GetDirectoryName(QEMU), "bios"),
								"/usr/share/qemu"
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
					CombineParameterAndDirectory("MOSA",@"ndisasm"),
					@"..\Tools\ndisasm",
					@"Tools\ndisasm",
					@"/bin"
				}
				);
			}
			if (string.IsNullOrEmpty(BOCHS))
			{
				// find BOCHS
				BOCHS = TryFind(
					new string[] { "bochs.exe", "bochs" },
					new string[] {
					CombineParameterAndDirectory("ProgramFiles",@"Bochs-2.6.5"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Bochs-2.6.5"),
					CombineParameterAndDirectory("ProgramFiles",@"Bochs-2.6.2"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Bochs-2.6.2"),
					CombineParameterAndDirectory("MOSA",@"Tools\Bochs"),
					CombineParameterAndDirectory("MOSA",@"Bochs"),
					@"..\Tools\Bochs",
					@"Tools\Bochs",
					@"/bin"
					}
				);
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
					@"/bin"
				}
				);
			}

			if (string.IsNullOrEmpty(mkisofs))
			{
				// find mkisofs
				mkisofs = TryFind(
					new string[] { "mkisofs.exe", "mkisofs" },
					new string[] {
					CombineParameterAndDirectory("ProgramFiles",@"VMware\VMware Player"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"VMware\VMware Player"),
					CombineParameterAndDirectory("ProgramFiles",@"cdrtools"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"cdrtools"),
					CombineParameterAndDirectory("MOSA",@"Tools\mkisofs"),
					CombineParameterAndDirectory("MOSA",@"mkisofs"),
					@"..\Tools\mkisofs",
					@"Tools\mkisofs",
					@"/bin"
				}
				);
			}
		}

		private string CombineParameterAndDirectory(string parameter, string subdirectory)
		{
			var variable = Environment.GetEnvironmentVariable(parameter);

			if (variable == null)
				return null;

			return Path.Combine(variable, subdirectory);
		}

		private string TryFind(IList<string> files, IList<string> directories)
		{
			string location;

			foreach (var directory in directories)
				foreach (var file in files)
					if (TryFind(file, directory, out location))
						return location;

			return string.Empty;
		}

		private bool TryFind(string file, string directory, out string location)
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
	}
}
