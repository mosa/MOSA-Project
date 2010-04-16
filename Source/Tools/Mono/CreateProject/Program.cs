/*
* (c) 2009 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Mono.CreateProject
{
	/// <summary>
	/// Program class for Mono.CreateProject
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// Main method
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		private static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("CreateProject v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2010. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			if (args.Length < 2) {
				Console.WriteLine("Usage: CreateProject <project name> [-s source file] [-x exclude file] [-c conditions] [-ref project dependency] [-path path prefix]");
				Console.Error.WriteLine("ERROR: Missing argument");
				return -1;
			}

			try {
				Project project = new Project();

				project.Name = System.IO.Path.GetFileNameWithoutExtension(args[0]);
				project.ProjectFile = args[0];

				for (int i = 1; i < args.Length; i = i + 2) {
					switch (args[i]) {
						case "-s": project.ReadSourceFile(args[i + 1]); break;
						case "-x": project.ReadExcludeFile(args[i + 1]); break;
						case "-c": project.Conditions.Add(args[i + 1]); break;
						case "-ref": if (!string.IsNullOrEmpty(args[i + 1])) {
								foreach (string split in args[i + 1].Split(new char[] { ';' }))
									project.ProjectReferences.Add(split);
							}
							break;
						case "-path": project.PathPrefix = args[i + 1] + System.IO.Path.DirectorySeparatorChar; break;
						default:
							Console.Error.WriteLine("ERROR: Invalid argument");
							return -1;
					}
				}

				project.Write();
			}
			catch (Exception e) {
				Console.Error.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		#region Nested type: project

		/// <summary>
		/// project Class
		/// </summary>
		public sealed class Project
		{
			public string Name;
			public string ProjectFile;
			public List<string> Files = new List<string>();
			public List<string> ProjectReferences = new List<string>();
			public List<string> Conditions = new List<string>();
			public List<string> ExcludeFiles = new List<string>();
			public string PathPrefix = string.Empty;

			public Project()
			{ }

			/// <summary>
			/// Reads the source file.
			/// </summary>
			/// <param name="source">The source.</param>
			public void ReadSourceFile(string source)
			{
				foreach (string file in System.IO.File.ReadAllLines(source))
					if (!string.IsNullOrEmpty(file.Trim()))
						Files.Add(file.Trim().Replace('/', Path.DirectorySeparatorChar));
			}

			/// <summary>
			/// Reads the exclude file.
			/// </summary>
			/// <param name="source">The source.</param>
			public void ReadExcludeFile(string source)
			{
				foreach (string file in System.IO.File.ReadAllLines(source))
					ExcludeFiles.Add(file);
			}

			/// <summary>
			/// Creates the project.
			/// </summary>
			public void Write()
			{
				using (TextWriter writer = new StreamWriter(ProjectFile)) {

					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine(
						"<Project ToolsVersion=\"3.5\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
					writer.WriteLine("\t<PropertyGroup>");
					writer.WriteLine("\t\t<Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>");
					writer.WriteLine("\t\t<Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
					writer.WriteLine("\t\t<ProductVersion>9.0.30729</ProductVersion>");
					writer.WriteLine("\t\t<SchemaVersion>2.0</SchemaVersion>");
					writer.WriteLine("\t\t<ProjectGuid>{" + Guid.NewGuid().ToString() + "}</ProjectGuid>");
					writer.WriteLine("\t\t<OutputType>library</OutputType>");
					writer.WriteLine("\t\t<AppDesignerFolder>Properties</AppDesignerFolder>");
					writer.WriteLine("\t\t<RootNamespace>" + Name + "</RootNamespace>");
					writer.WriteLine("\t\t<AssemblyName>" + Name + "</AssemblyName>");
					writer.WriteLine("\t\t<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>");
					writer.WriteLine("\t\t<FileAlignment>512</FileAlignment>");
					writer.WriteLine("\t\t<NoStdLib>True</NoStdLib>");
					writer.WriteLine("\t</PropertyGroup>");
					writer.WriteLine("\t<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">");
					writer.WriteLine("\t\t<DebugSymbols>true</DebugSymbols>");
					writer.WriteLine("\t\t<DebugType>full</DebugType>");
					writer.WriteLine("\t\t<Optimize>false</Optimize>");
					writer.WriteLine("\t\t<OutputPath>bin\\Debug\\</OutputPath>");

					StringBuilder conditions = new StringBuilder();
					foreach (string file in Conditions)
						conditions.Append(file + ";");
					writer.WriteLine("\t\t<DefineConstants>TRACE;DEBUG;" + conditions + "</DefineConstants>");

					writer.WriteLine("\t\t<ErrorReport>prompt</ErrorReport>");
					writer.WriteLine("\t\t<WarningLevel>4</WarningLevel>");
					writer.WriteLine("\t\t<AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
					writer.WriteLine("\t\t<NoStdLib>True</NoStdLib>");
					writer.WriteLine("\t</PropertyGroup>");
					writer.WriteLine("\t<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
					writer.WriteLine("\t\t<DebugType>pdbonly</DebugType>");
					writer.WriteLine("\t\t<Optimize>true</Optimize>");
					writer.WriteLine("\t\t<OutputPath>bin\\Release\\</OutputPath>");
					writer.WriteLine("\t\t<DefineConstants>" + conditions + "</DefineConstants>");
					writer.WriteLine("\t\t<ErrorReport>prompt</ErrorReport>");
					writer.WriteLine("\t\t<WarningLevel>4</WarningLevel>");
					writer.WriteLine("\t\t<AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
					writer.WriteLine("\t\t<NoStdLib>True</NoStdLib>");
					writer.WriteLine("\t</PropertyGroup>");
					writer.WriteLine("\t<ItemGroup>");

					foreach (string file in Files)
						writer.WriteLine("\t\t<Compile Include=\"" + PathPrefix + file + "\" />");

					writer.WriteLine("\t</ItemGroup>");

					if (ProjectReferences.Count > 0) {
						writer.WriteLine("\t<ItemGroup>");
						foreach (string project in ProjectReferences) {
							writer.WriteLine("\t\t<ProjectReference Include=\"..\\" + System.IO.Path.GetFileNameWithoutExtension(project) + "\\" + project + ".csproj\">");
							writer.WriteLine("\t\t\t<Name>" + System.IO.Path.GetFileNameWithoutExtension(project) + "</Name>");
							writer.WriteLine("\t\t</ProjectReference>");
						}
						writer.WriteLine("\t</ItemGroup>");
					}

					writer.WriteLine("\t<Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />");
					writer.WriteLine("</Project>");
				}
			}

		}

		#endregion
	}
}