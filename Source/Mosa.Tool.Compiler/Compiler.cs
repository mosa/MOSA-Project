// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Trace.BuiltIn;
using Mosa.Utility.Aot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Class containing the Compiler.
	/// </summary>
	public class Compiler
	{
		#region Data

		protected MosaCompiler compiler = new MosaCompiler();

		/// <summary>
		/// Holds a reference to the Options parsed from the arguments.
		/// </summary>
		private Options options;

		private readonly int majorVersion = 1;
		private readonly int minorVersion = 4;
		private readonly string codeName = "Neptune";

		/// <summary>
		/// A string holding a simple usage description.
		/// </summary>
		private readonly string usageString;

		#endregion Data

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Compiler class.
		/// </summary>
		public Compiler()
		{
			compiler.CompilerFactory = delegate { return new AotCompiler(); };

			usageString = "Usage: mosacl -o outputfile --Architecture=[x86|x64|ARMv6] --format=[ELF32|ELF64] {--boot=[mb0.7]} {additional options} inputfiles";
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Runs the command line parser and the compilation process.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		public void Run(string[] args)
		{
			// always print header with version information
			Console.WriteLine("MOSA AOT Compiler, Version {0}.{1} '{2}'", majorVersion, minorVersion, codeName);
			Console.WriteLine("Copyright 2015 by the MOSA Project. Licensed under the New BSD License.");

			//Console.WriteLine("Copyright 2008 by Novell. NDesk.Options is released under the MIT/X11 license.");
			Console.WriteLine();
			Console.WriteLine("Parsing options...");

			try
			{
				options = ParseOptions(args);
				if (options == null)
				{
					ShowShortHelp();
					return;
				}

				if (options.InputFiles.Count == 0)
				{
					throw new Exception("No input file(s) specified.");
				}

				compiler.CompilerOptions = options.CompilerOptions;

				// Process boot format:
				// Boot format only matters if it's an executable
				// Process this only now, because input files must be known
				if (!options.IsInputExecutable && compiler.CompilerOptions.BootStageFactory != null)
				{
					Console.WriteLine("Warning: Ignoring boot format, because target is not an executable.");
					Console.WriteLine();
				}

				if (string.IsNullOrEmpty(compiler.CompilerOptions.OutputFile))
				{
					throw new Exception("No output file specified.");
				}

				if (compiler.CompilerOptions.Architecture == null)
				{
					throw new Exception("No Architecture specified.");
				}
			}
			catch (Exception e)
			{
				ShowError(e.Message);
				return;
			}

			Console.WriteLine(ToString());

			Console.WriteLine("Compiling ...");

			DateTime start = DateTime.Now;

			try
			{
				Compile();
			}
			catch (CompilerException ce)
			{
				ShowError(ce.Message);
			}

			DateTime end = DateTime.Now;

			TimeSpan time = end - start;
			Console.WriteLine();
			Console.WriteLine("Compilation time: " + time);
		}

		private static Options ParseOptions(string[] args)
		{
			ParserResult<Options> result = new Parser(config => config.HelpWriter = Console.Out).ParseArguments<Options>(args);

			if (result.Tag == ParserResultType.NotParsed)
			{
				return null;
			}

			return ((Parsed<Options>)result).Value;
		}

		/// <summary>
		/// Returns a string representation of the current options.
		/// </summary>
		/// <returns>A string containing the options.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(" > Output file: ").AppendLine(compiler.CompilerOptions.OutputFile);
			sb.Append(" > Input file(s): ").AppendLine(string.Join(", ", new List<string>(GetInputFileNames()).ToArray()));
			sb.Append(" > Architecture: ").AppendLine(compiler.CompilerOptions.Architecture.GetType().FullName);
			sb.Append(" > Binary format: ").AppendLine(compiler.CompilerOptions.LinkerFormatType.ToString());
			sb.Append(" > Boot format: ").AppendLine((compiler.CompilerOptions.BootStageFactory == null) ? "None" : compiler.CompilerOptions.BootStageFactory().Name);
			sb.Append(" > Is executable: ").AppendLine(options.IsInputExecutable.ToString());
			return sb.ToString();
		}

		#endregion Public Methods

		#region Private Methods

		private void Compile()
		{
			compiler.CompilerTrace.TraceListener = new ConsoleEventListener();
			compiler.Load(options.InputFiles);
			compiler.Execute(Environment.ProcessorCount);
		}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		private IEnumerable<string> GetInputFileNames()
		{
			foreach (FileInfo file in options.InputFiles)
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
			Console.WriteLine("Execute 'mosacl --help' for more information.");
			Console.WriteLine();
		}

		/// <summary>
		/// Shows a short help text pointing to the '--help' option.
		/// </summary>
		private void ShowShortHelp()
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.WriteLine("Execute 'mosacl --help' for more information.");
		}

		#endregion Private Methods

		#region Internal Methods

		/// <summary>
		/// Selects the architecture.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		private static BaseArchitecture SelectArchitecture(string architecture)
		{
			switch (architecture.ToLower())
			{
				case "x86": return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException(string.Format("Unknown or unsupported Architecture {0}.", architecture));
			}
		}

		private static Func<BaseCompilerStage> GetBootStageFactory(string format)
		{
			switch (format.ToLower())
			{
				case "multibootHeader-0.7":
				case "mb0.7": return delegate { return new Mosa.Platform.x86.CompilerStages.Multiboot0695Stage(); };
				default: throw new NotImplementCompilerException(string.Format("Unknown or unsupported boot format {0}.", format));
			}
		}

		private static LinkerFormatType GetLinkerFactory(string format)
		{
			switch (format.ToLower())
			{
				case "elf": return LinkerFormatType.Elf32;
				case "elf32": return LinkerFormatType.Elf32;
				case "elf64": return LinkerFormatType.Elf64;
				default: return LinkerFormatType.Elf32;
			}
		}

		#endregion Internal Methods
	}
}
