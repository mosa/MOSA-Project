/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>  
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Tool.Compiler.Stages;
using NDesk.Options;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Class containing the Compiler.
	/// </summary>
	public class Compiler
	{
		#region Data

		/// <summary>
		/// Holds the compiler options
		/// </summary>
		private CompilerOptions compilerOptions = new CompilerOptions();

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

		private readonly int majorVersion = 1;
		private readonly int minorVersion = 1;
		private readonly string codeName = @"Zaphod";

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
			usageString = "Usage: mosacl -o outputfile --Architecture=[x86|avr32] --format=[ELF32|ELF64|PE] {--boot=[mb0.7]} {additional options} inputfiles";
			optionSet = new OptionSet();
			inputFiles = new List<FileInfo>();

			#region Setup general options
			optionSet.Add(
				"v|version",
				"Display version information.",
				delegate(string v)
				{
					if (v != null)
					{
						// only show header and exit
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

			#region Setup options

			optionSet.Add(
				"b|boot=",
				"Specify the bootable format of the produced binary [{mb0.7}].",
				delegate(string format)
				{
					compilerOptions.BootCompilerStage = SelectBootStage(format);
				}
			);

			optionSet.Add(
				"a|Architecture=",
				"Select one of the MOSA architectures to compile for [{x86|avr32}].",
				delegate(string arch)
				{
					compilerOptions.Architecture = SelectArchitecture(arch);
				}
			);

			optionSet.Add(
				"f|format=",
				"Select the format of the binary file to create [{ELF32|ELF64|PE}].",
				delegate(string format)
				{
					compilerOptions.Linker = SelectLinkerStage(format);
				}
			);

			optionSet.Add(
				"o|out=",
				"The name of the output {file}.",
				delegate(string file)
				{
					compilerOptions.OutputFile = file;
				}
			);

			optionSet.Add<uint>(
				"elf-file-alignment=",
				"Determines the alignment of sections within the ELF file. Must be a multiple of 512 bytes.",
				delegate(uint alignment)
				{
					try
					{
						compilerOptions.Elf32.FileAlignment = alignment;
					}
					catch (System.Exception x)
					{
						throw new OptionException(@"The specified file alignment is invalid.", @"elf-file-alignment", x);
					}
				}
			);

			optionSet.Add(
				"map=",
				"Generate a map {file} of the produced binary.",
				delegate(string file)
				{
					compilerOptions.MapFile = file;
				}
			);

			optionSet.Add(
				"method-pipeline-export-dir|mped=",
				"The method pipeline export directory {file}.",
				delegate(string dir)
				{
					compilerOptions.MethodPipelineExportDirectory = dir;
				}
			);

			optionSet.Add(
				"pe-no-checksum",
				"Causes no checksum to be written in the generated PE file. MOSA requires the checksum to be set. It is on by default, use this switch to turn it off.",
				delegate(string value)
				{
					compilerOptions.PortableExecutable.SetChecksum = false;
				}
			);

			optionSet.Add<uint>(
				"pe-file-alignment=",
				"Determines the alignment of sections within the PE file. Must be a multiple of 512 bytes.",
				delegate(uint alignment)
				{
					try
					{
						compilerOptions.PortableExecutable.FileAlignment = alignment;
					}
					catch (Exception x)
					{
						throw new OptionException(@"The specified file alignment is invalid.", @"pe-file-alignment", x);
					}
				}
			);

			optionSet.Add<uint>(
				"pe-section-alignment=",
				"Determines the alignment of sections in memory. Must be a multiple of 4096 bytes.",
				delegate(uint alignment)
				{
					try
					{
						compilerOptions.PortableExecutable.SectionAlignment = alignment;
					}
					catch (Exception x)
					{
						throw new OptionException(@"The specified section alignment is invalid.", @"pe-section-alignment", x);
					}
				}
			);

			optionSet.Add(
				@"sa|enable-static-alloc",
				@"Performs static allocations at compile time.",
				enable => compilerOptions.EnableStaticAllocations = enable != null
			);

			optionSet.Add(
				@"ssa|enable-single-static-assignment-form",
				@"Performs single static assignments at compile time.",
				enable => compilerOptions.EnableSSA = enable != null
			);

			optionSet.Add(
				"stats=",
				"Generate instruction statistics {file} of the produced binary.",
				delegate(string file)
				{
					compilerOptions.StatisticsFile = file;
				}
			);

			optionSet.Add(
				"multiboot-video-mode=",
				"Specify the video mode for multiboot [{text|graphics}].",
				delegate(string v)
				{
					switch (v.ToLower())
					{
						case "text":
							compilerOptions.Multiboot.VideoMode = 1;
							break;
						case "graphics":
							compilerOptions.Multiboot.VideoMode = 0;
							break;
						default:
							throw new OptionException("Invalid value for multiboot video mode: " + v, "multiboot-video-mode");
					}
				}
			);

			optionSet.Add(
				"multiboot-video-width=",
				"Specify the {width} for video output, in pixels for graphics mode or in characters for text mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val))
					{
						// TODO: this probably needs further validation
						compilerOptions.Multiboot.VideoWidth = val;
					}
					else
					{
						throw new OptionException("Invalid value for multiboot video width: " + v, "multiboot-video-width");
					}
				}
			);

			optionSet.Add(
				"multiboot-video-height=",
				"Specify the {height} for video output, in pixels for graphics mode or in characters for text mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val))
					{
						// TODO: this probably needs further validation
						compilerOptions.Multiboot.VideoHeight = val;
					}
					else
					{
						throw new OptionException("Invalid value for multiboot video height: " + v, "multiboot-video-height");
					}
				}
			);

			optionSet.Add(
				"multiboot-video-depth=",
				"Specify the {depth} (number of bits per pixel) for graphics mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val))
					{
						compilerOptions.Multiboot.VideoDepth = val;
					}
					else
					{
						throw new OptionException("Invalid value for multiboot video depth: " + v, "multiboot-video-depth");
					}
				}
			);

			optionSet.Add(
				"multiboot-module=",
				"Adds a {0:module} to multiboot, to be loaded at a given {1:virtualAddress} (can be used multiple times).",
				delegate(string file, string address)
				{
					// TODO: validate and add this to a list or something
					Console.WriteLine("Adding multiboot module " + file + " at virtualAddress " + address);
				}
			);

			#endregion // Setup options

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
			Console.WriteLine("Copyright 2011 by the MOSA Project. Licensed under the New BSD License.");
			Console.WriteLine("Copyright 2008 by Novell. NDesk.Options is released under the MIT/X11 license.");
			Console.WriteLine();
			Console.WriteLine("Parsing options...");

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
				if (!isExecutable && compilerOptions.BootCompilerStage != null)
				{
					Console.WriteLine("Warning: Ignoring boot format, because target is not an executable.");
					Console.WriteLine();
				}

				// Check for missing options
				if (compilerOptions.Linker == null)
				{
					throw new OptionException("No binary format specified.", "Architecture");
				}

				if (String.IsNullOrEmpty(compilerOptions.OutputFile))
				{
					throw new OptionException("No output file specified.", "o");
				}

				if (compilerOptions.Architecture == null)
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

			DateTime start = DateTime.Now;

			try
			{
				Compile();
			}
			catch (CompilationException ce)
			{
				this.ShowError(ce.Message);
			}

			DateTime end = DateTime.Now;

			TimeSpan time = end - start;
			Console.WriteLine();
			Console.WriteLine("Compilation time: " + time);
		}

		/// <summary>
		/// Returns a string representation of the current options.
		/// </summary>
		/// <returns>A string containing the options.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(" > Output file: ").AppendLine(compilerOptions.OutputFile);
			sb.Append(" > Input file(s): ").AppendLine(String.Join(", ", new List<string>(GetInputFileNames()).ToArray()));
			sb.Append(" > Architecture: ").AppendLine(compilerOptions.Architecture.GetType().FullName);
			sb.Append(" > Binary format: ").AppendLine(((IPipelineStage)compilerOptions.Linker).Name);
			sb.Append(" > Boot format: ").AppendLine((compilerOptions.BootCompilerStage == null) ? "None" : ((IPipelineStage)compilerOptions.BootCompilerStage).Name);
			sb.Append(" > Is executable: ").AppendLine(isExecutable.ToString());
			return sb.ToString();
		}

		#endregion Public Methods

		#region Private Methods

		private void Compile()
		{
			AotAssemblyCompiler.Compile(compilerOptions, inputFiles);
		}

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

		#region Internal Methods

		/// <summary>
		/// Selects the architecture.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		private static IArchitecture SelectArchitecture(string architecture)
		{
			switch (architecture.ToLower())
			{
				case "x86":
					return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);

				case "avr32":
					return Mosa.Platform.AVR32.Architecture.CreateArchitecture(Mosa.Platform.AVR32.ArchitectureFeatureFlags.AutoDetect);

				case "x64":

				default:
					throw new OptionException(String.Format("Unknown or unsupported architecture {0}.", architecture), "Architecture");
			}
		}

		/// <summary>
		/// Selects the boot stage.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		private static IAssemblyCompilerStage SelectBootStage(string format)
		{
			switch (format.ToLower())
			{
				case "multiboot-0.7":
				case "mb0.7":
					return new Multiboot0695AssemblyStage();

				default:
					throw new OptionException(String.Format("Unknown or unsupported boot format {0}.", format), "boot");
			}
		}

		/// <summary>
		/// Selects the linker implementation to use.
		/// </summary>
		/// <param name="format">The linker format.</param>
		/// <returns>The implementation of the linker.</returns>
		private static IAssemblyLinker SelectLinkerStage(string format)
		{
			switch (format.ToLower())
			{
				case "elf32":
					return new Elf32LinkerStage();

				case "elf64":
					return new Elf64LinkerStage();

				case "pe":
					return new PortableExecutableLinkerStage();

				default:
					throw new OptionException(String.Format("Unknown or unsupported binary format {0}.", format), "format");
			}
		}

		#endregion
	}
}
