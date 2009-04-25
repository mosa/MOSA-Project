#region

using System;
using System.IO;

#endregion

namespace Mosa.Tools.Mono.CreateProjects
{
	internal class Program
	{
		private static int Main(string[] args)
		{
			Console.WriteLine("CreateProjects v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: CreateProjects <source directory> <destination directory>");
			Console.WriteLine();

			if (args.Length < 2) {
				Console.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			try {
				List<string> files = new List<string>();

				FindFiles(args[0], string.Empty, ref files);

				foreach (string file in files)
					Process(args[0], args[1], file);
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		private static void FindFiles(string root, string directory, ref List<string> files)
		{
			foreach (
				string file in Directory.GetFiles(Path.Combine(root, directory), "*.dll.sources", SearchOption.TopDirectoryOnly)) {
				files.Add(Path.Combine(directory, Path.GetFileName(file)));
			}

			foreach (string dir in Directory.GetDirectories(Path.Combine(root, directory), "*.*", SearchOption.TopDirectoryOnly)) {
				if (dir.Contains(".svn"))
					continue;
				FindFiles(root, Path.Combine(directory, Path.GetFileName(dir)), ref files);
			}
		}

		private static void Process(string root, string dest, string full)
		{
			Library library = CreateLibrary(root, full);

			Console.WriteLine(library.Name);

			AddPartialFiles(library, dest);

			List<string> project = CreateProject(library);

			string projectfile = Path.Combine(dest, Path.Combine(dest, library.Name + ".csproj"));

			using (TextWriter writer = new StreamWriter(projectfile)) {
				foreach (string line in project)
					if (line != null)
						writer.WriteLine(line);

				writer.Close();
			}
		}

		private static Library CreateLibrary(string root, string full)
		{
			string filename = Path.GetFileName(full);

			string dll = filename.Substring(0, filename.Length - 8);
			string name = dll.Substring(0, dll.Length - 4);

			string path = Path.GetDirectoryName(full);

			Library library = new Library(name, dll, path);

			string[] lines = File.ReadAllLines(Path.Combine(root, full));

			foreach (string line in lines)
				if (!line.StartsWith("#"))
					library.Files.Add(Path.Combine(path, line.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)));

			return library;
		}

		private static void AddPartialFiles(Library library, string dest)
		{
			for (int i = library.Files.Count - 1; i > 0; i--) {
				string partial = library.Files[i].Insert(library.Files[i].Length - 2, "Partial.");

				if (File.Exists(Path.Combine(dest, partial)))
					library.Files.Add(partial);
			}
		}

		private static List<string> CreateProject(Library library)
		{
			List<string> project = new List<string>();

			project.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			project.Add(
				"<Project ToolsVersion=\"3.5\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
			project.Add("\t<PropertyGroup>");
			project.Add("\t\t<Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>");
			project.Add("\t\t<Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
			project.Add("\t\t<ProductVersion>9.0.30729</ProductVersion>");
			project.Add("\t\t<SchemaVersion>2.0</SchemaVersion>");
			project.Add("\t\t<ProjectGuid>{" + Guid.NewGuid().ToString() + "}</ProjectGuid>");
			project.Add("\t\t<OutputType>Library</OutputType>");
			project.Add("\t\t<AppDesignerFolder>Properties</AppDesignerFolder>");
			project.Add("\t\t<RootNamespace>Mono</RootNamespace>");
			project.Add("\t\t<AssemblyName>Mono</AssemblyName>");
			project.Add("\t\t<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>");
			project.Add("\t\t<FileAlignment>512</FileAlignment>");
			project.Add("\t\t<NoStdLib>True</NoStdLib>");
			project.Add("\t</PropertyGroup>");
			project.Add("\t<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
			project.Add("\t\t<DebugSymbols>true</DebugSymbols>");
			project.Add("\t\t<DebugType>full</DebugType>");
			project.Add("\t\t<Optimize>false</Optimize>");
			project.Add("\t\t<OutputPath>bin\\Debug\\</OutputPath>");
			project.Add("\t\t<DefineConstants>TRACE;DEBUG;NET_2_0 NET_1_1 INSIDE_CORLIB</DefineConstants>");
			project.Add("\t\t<ErrorReport>prompt</ErrorReport>");
			project.Add("\t\t<WarningLevel>4</WarningLevel>");
			project.Add("\t\t<AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
			project.Add("\t\t<NoStdLib>True</NoStdLib>");
			project.Add("\t</PropertyGroup>");
			project.Add("\t<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
			project.Add("\t\t<DebugType>pdbonly</DebugType>");
			project.Add("\t\t<Optimize>true</Optimize>");
			project.Add("\t\t<OutputPath>bin\\Release\\</OutputPath>");
			project.Add("\t\t<DefineConstants>TRACE;NET_2_0 NET_1_1 INSIDE_CORLIB</DefineConstants>");
			project.Add("\t\t<ErrorReport>prompt</ErrorReport>");
			project.Add("\t\t<WarningLevel>4</WarningLevel>");
			project.Add("\t\t<AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
			project.Add("\t\t<NoStdLib>True</NoStdLib>");
			project.Add("\t</PropertyGroup>");
			project.Add("\t<ItemGroup>");

			foreach (string file in library.Files) {
				project.Add("\t\t<Compile Include=\"" + file + "\" />");
			}

			project.Add("\t</ItemGroup>");
			project.Add("\t<Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />");
			project.Add("</Project>");

			return project;
		}

		#region Nested type: Library

		public class Library
		{
			public string DLL;
			public List<string> Files = new List<string>();
			public string Name;
			public string Path;

			public Library(string name, string dll, string path)
			{
				Name = name;
				DLL = dll;
				Path = path;
			}
		}

		#endregion
	}
}