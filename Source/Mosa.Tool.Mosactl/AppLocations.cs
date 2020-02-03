// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Tool.Mosactl
{
	public class AppLocations
	{
		public string QEMU { get; }

		public string MsBuild { get; }

		public AppLocations()
		{
			if (string.IsNullOrEmpty(QEMU))
			{
				// find QEMU executable
				QEMU = TryFind(
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

			if (string.IsNullOrEmpty(MsBuild))
			{
				MsBuild = TryFind(
					new string[] { "msbuild.exe", "msbuild" },
					new string[] {
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\amd64"),
						CombineParameterAndDirectory("ProgramFiles(x86)", @"Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin"),
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
