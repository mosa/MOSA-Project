/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using NDesk.Options;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Tools.Compiler.Boot;
using Mosa.Tools.Compiler.Linkers;
using Mosa.Runtime.Linker;
using Mosa.Tools.Compiler.Symbols.Pdb;
using Mosa.Tools.Compiler.Stages;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	/// Class containing the Compiler.
	/// </summary>
	public class Compiler
	{
		#region Fields

		/// <summary>
		/// Holds the stage responsible for the architecture.
		/// </summary>
		private ArchitectureSelector architectureSelector;

		/// <summary>
		/// Holds the stage responsible for the linker/binary format.
		/// </summary>
		private LinkerFormatSelector linkerStage;

		/// <summary>
		/// Holds the stage responsible for the boot format.
		/// </summary>
		private BootFormatSelector bootFormatStage;

		/// <summary>
		/// Holds the name of the map file to generate.
		/// </summary>
		private MapFileGeneratorWrapper mapFileWrapper;

		/// <summary>
		/// Holds a list of input files.
		/// </summary>
		private List<FileInfo> inputFiles;

		/// <summary>
		/// Determines if the file is executable.
		/// </summary>
		private bool isExecutable;

		/// <summary>
		/// Holds a reference to the OptionSet used for option parsing.
		/// </summary>
		private OptionSet optionSet;

		private readonly int majorVersion = 0;
		private readonly int minorVersion = 6;
		private readonly string codeName = @"Tanigawa";

		/// <summary>
		/// A string holding a simple usage description.
		/// </summary>
		private readonly string usageString;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Compiler class.
		/// </summary>
		public Compiler()
		{
			usageString = "Usage: mosacl -o outputfile --Architecture=[x86] --format=[ELF32|ELF64|PE] {--boot=[mb0.7]} {additional options} inputfiles";
			optionSet = new OptionSet();
			inputFiles = new List<FileInfo>();

			this.linkerStage = new LinkerFormatSelector();
			this.bootFormatStage = new BootFormatSelector();
			this.architectureSelector = new ArchitectureSelector();
			this.mapFileWrapper = new MapFileGeneratorWrapper();

			#region Setup general options
			optionSet.Add(
				"v|version",
				"Display version information.",
				delegate(string v)
				{
					if (v != null)
					{
						// only show _header and exit
						Environment.Exit(0);
					}
				});

			optionSet.Add(
				"h|?|help",
				"Display the full set of available options.",
				delegate(string v)
				{
					if (v != null)
					{
						this.ShowHelp();
						Environment.Exit(0);
					}
				});

			// default option handler for input files
			optionSet.Add(
				"<>",
				"Input files.",
				delegate(string v)
				{
					if (!File.Exists(v))
					{
						throw new OptionException(String.Format("Input file or option '{0}' doesn't exist.", v), String.Empty);
					}

					FileInfo file = new FileInfo(v);
					if (file.Extension.ToLower() == ".exe")
					{
						if (isExecutable)
						{
							// there are more than one exe files in the list
							throw new OptionException("Multiple executables aren't allowed.", String.Empty);
						}

						isExecutable = true;
					}

					inputFiles.Add(file);
				});

			#endregion

			this.linkerStage.AddOptions(optionSet);
			this.bootFormatStage.AddOptions(optionSet);
			this.architectureSelector.AddOptions(optionSet);
			this.mapFileWrapper.AddOptions(optionSet);

			StaticAllocationResolutionStageWrapper.Instance.AddOptions(optionSet);
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Runs the command line parser and the compilation process.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		public void Run(string[] args)
		{
			// always print _header with version information
			Console.WriteLine("MOSA AOT Compiler, Version {0}.{1} '{2}'", majorVersion, minorVersion, codeName);
			Console.WriteLine("Copyright 2009 by the MOSA Project. Licensed under the New BSD License.");
			Console.WriteLine("Copyright 2008 by Novell. NDesk.Options is released under the MIT/X11 license.");
			Console.WriteLine();

			try
			{
				if (args == null || args.Length == 0)
				{
					// no arguments are specified
					ShowShortHelp();
					return;
				}

				optionSet.Parse(args);

				if (inputFiles.Count == 0)
				{
					throw new OptionException("No input file(s) specified.", String.Empty);
				}

				// Process boot format:
				// Boot format only matters if it's an executable
				// Process this only now, because input files must be known
				if (isExecutable == false && bootFormatStage.IsConfigured == true)
				{
					Console.WriteLine("Warning: Ignoring boot format, because target is not an executable.");
					Console.WriteLine();
				}

				// Check for missing options
				if (!linkerStage.IsConfigured)
				{
					throw new OptionException("No binary format specified.", "Architecture");
				}

				if (String.IsNullOrEmpty(this.linkerStage.OutputFile))
				{
					throw new OptionException("No output file specified.", "o");
				}

				if (!architectureSelector.IsConfigured)
				{
					throw new OptionException("No architecture specified.", "Architecture");
				}
			}
			catch (OptionException e)
			{
				ShowError(e.Message);
				return;
			}

			Console.WriteLine(this.ToString());

			Console.WriteLine("Compiling ...");

			Compile();

			//try
			//{
			//    Compile();
			//}
			//catch (CompilationException ce)
			//{
			//    this.ShowError(ce.Message);
			//}
		}

		/// <summary>
		/// Returns a string representation of the current options.
		/// </summary>
		/// <returns>A string containing the options.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Output file: ").AppendLine(this.linkerStage.OutputFile);
			sb.Append("Input file(s): ").AppendLine(String.Join(", ", new List<string>(GetInputFileNames()).ToArray()));
			sb.Append("Architecture: ").AppendLine(architectureSelector.Architecture.GetType().FullName);
			sb.Append("Binary format: ").AppendLine(((IPipelineStage)linkerStage).Name);
			sb.Append("Boot format: ").AppendLine(((IPipelineStage)bootFormatStage).Name);
			sb.Append("Is executable: ").AppendLine(isExecutable.ToString());
			return sb.ToString();
		}

		#endregion Public Methods

		#region Private Methods

		private void Compile()
		{
			CompilationRuntime runtime = new CompilationRuntime();

			runtime.AssemblyLoader.InitializePrivatePaths(this.GetInputFileNames());
			runtime.TypeSystem.LoadModules(this.GetInputFileNames());

			// Create the compiler
			using (AotCompiler aot = new AotCompiler(this.architectureSelector.Architecture, runtime.TypeSystem))
			{
				aot.Pipeline.AddRange(new IAssemblyCompilerStage[] 
					{
						this.bootFormatStage,
						new InterruptBuilderStage(),						
						new AssemblyCompilationStage(), 
						//new FakeSystemObjectGenerationStage(),
						new MethodCompilerSchedulerStage(),
						new TypeInitializers.TypeInitializerSchedulerStage(),
						this.bootFormatStage,
						new Metadata.MetadataBuilderStage(),
						new CilHeaderBuilderStage(),
						new ObjectFileLayoutStage(),
						this.linkerStage,
						this.mapFileWrapper
					});

				aot.Run();
			}
		}

		//private IMetadataModule GetMainAssembly(IAssemblyLoader assemblyLoader)
		//{
		//    string firstAssembly = this.inputFiles[0].FullName;
		//    return assemblyLoader.Load(firstAssembly);
		//}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		private IEnumerable<string> GetInputFileNames()
		{
			foreach (FileInfo file in this.inputFiles)
				yield return file.FullName;
		}

		/// <summary>
		/// Shows an error and a short information text.
		/// </summary>
		/// <param name="message">The error message to show.</param>
		private void ShowError(string message)
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.Write("Error: ");
			Console.WriteLine(message);
			Console.WriteLine();
			Console.WriteLine("Run 'mosacl --help' for more information.");
			Console.WriteLine();
		}

		/// <summary>
		/// Shows a short help text pointing to the '--help' option.
		/// </summary>
		private void ShowShortHelp()
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.WriteLine("Run 'mosacl --help' for more information.");
		}

		/// <summary>
		/// Shows the full help containing descriptions for all possible options.
		/// </summary>
		private void ShowHelp()
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.WriteLine("Options:");
			this.optionSet.WriteOptionDescriptions(Console.Out);
		}

		#endregion Private Methods
	}
}
