// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Trace.BuiltIn;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Class containing the Compiler.
	/// </summary>
	public class Compiler
	{
		#region Data

		protected MosaCompiler compiler;

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
			usageString = @"Usage: Mosa.Tool.Compiler.exe -o outputfile --achitecture=[x86|x64|ARMv6] --format=[ELF32|ELF64] {--boot=[mb0.7]} {additional options} inputfiles.

Example: Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin -a x86 --mboot v1 --x86-irq-methods --base-address 0x00500000 Mosa.HelloWorld.x86.exe mscorlib.dll Mosa.Plug.Korlib.dll Mosa.Plug.Korlib.x86.dll";
		}

		#endregion Constructors

		private List<BaseCompilerExtension> GetCompilerExtensions()
		{
			var list = new List<BaseCompilerExtension>();
			list.Add(new Mosa.Compiler.Extensions.Dwarf.DwarfCompilerExtension());
			return list;
		}

		#region Public Methods

		/// <summary>
		/// Runs the command line parser and the compilation process.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		public void Run(string[] args)
		{
			// always print header with version information
			Console.WriteLine("MOSA AOT Compiler, Version {0}.{1} '{2}'", majorVersion, minorVersion, codeName);
			Console.WriteLine("Copyright 2019 by the MOSA Project. Licensed under the New BSD License.");

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

				compiler = new MosaCompiler(options.CompilerOptions, GetCompilerExtensions());

				Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
				Debug.AutoFlush = true;

				// Process boot format:
				// Boot format only matters if it's an executable
				// Process this only now, because input files must be known
				if (!options.IsInputExecutable)
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
				Environment.Exit(1);
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
				Environment.Exit(1);
				return;
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
			var sb = new StringBuilder();
			sb.Append(" > Output file: ").AppendLine(compiler.CompilerOptions.OutputFile);
			sb.Append(" > Input file(s): ").AppendLine(string.Join(", ", new List<string>(GetInputFileNames()).ToArray()));
			sb.Append(" > Architecture: ").AppendLine(compiler.CompilerOptions.Architecture.GetType().FullName);
			sb.Append(" > Binary format: ").AppendLine(compiler.CompilerOptions.LinkerFormatType.ToString());
			sb.Append(" > Boot spec: ").AppendLine(compiler.CompilerOptions.MultibootSpecification.ToString());
			sb.Append(" > Is executable: ").AppendLine(options.IsInputExecutable.ToString());
			return sb.ToString();
		}

		#endregion Public Methods

		#region Private Methods

		private void Compile()
		{
			compiler.CompilerTrace.SetTraceListener(new ConsoleEventListener());

			compiler.CompilerOptions.AddSourceFiles(options.InputFiles);

			compiler.Load();

			compiler.ThreadedCompile();
		}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		private IEnumerable<string> GetInputFileNames()
		{
			foreach (var file in options.InputFiles)
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
			Console.WriteLine("Execute 'Mosa.Tool.Compiler.exe --help' for more information.");
			Console.WriteLine();
		}

		/// <summary>
		/// Shows a short help text pointing to the '--help' option.
		/// </summary>
		private void ShowShortHelp()
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.WriteLine("Execute 'Mosa.Tool.Compiler.exe --help' for more information.");
		}

		#endregion Private Methods
	}
}
