using CommandLine;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Linker;

namespace Mosa.Tool.Compiler
{
	class Options
	{
		public List<FileInfo> InputFiles { get; private set; }
		public bool IsInputExecutable { get; private set; }

		[Value(0, MetaName = "Input files", HelpText = "Input files.", Min = 1, Required = true)]
		public IEnumerable<string> InputFilePaths
		{
			set
			{
				foreach (string v in value)
				{
					if (!File.Exists(v))
					{
						throw new Exception(string.Format("Input file '{0}' doesn't exist.", v));
					}

					FileInfo file = new FileInfo(v);
					if (file.Extension.ToLower() == ".exe")
					{
						if (IsInputExecutable)
						{
							// there are more than one exe files in the list
							throw new Exception("Multiple executables aren't allowed.");
						}

						IsInputExecutable = true;
					}

					InputFiles.Add(file);
				}
			}
		}

		[Option('b', "boot", HelpText = "Specify the bootable format of the produced binary [mb0.7].")]
		public string BootFormat { set { CompilerOptions.BootStageFactory = GetBootStageFactory(value); } }

		[Option('a', "architecture", HelpText = "Select one of the MOSA architectures to compile for [x86|x64|ARMv6].", Required = true)]
		public string Architecture { set { CompilerOptions.Architecture = SelectArchitecture(value); } }

		[Option('f', "format", HelpText = "Select the format of the binary file to create [ELF32|ELF64].")]
		public string LinkerFormat { set { CompilerOptions.LinkerFormatType = GetLinkerFactory(value); } }

		[Option('o', "out", HelpText = "The name of the output file.", Required = true)]
		public string OutputFile { set { CompilerOptions.OutputFile = value; } }

		[Option("map", HelpText = "Generate a map file of the produced binary.")]
		public string MapFile { set { CompilerOptions.MapFile = value; } }

		[Option("debug-info=", HelpText = "Generate a debug info file of the produced binary.")]
		public string DebugInfoFile { set { CompilerOptions.DebugFile = value; } }

		[Option("sa", HelpText = "Performs static allocations at compile time.")]
		public bool EnableStaticAllocation { set { CompilerOptions.EnableStaticAllocations = value; } }

		[Option("enable-static-alloc", HelpText = "Performs static allocations at compile time.")]
		public bool EnableStaticAllocationExtraOption
		{
			set { EnableStaticAllocation = value; }
		}

		[Option("ssa", HelpText = "Performs single static assignments at compile time.")]
		public bool EnableSSA { set { CompilerOptions.EnableSSA = value; } }

		[Option("enable-single-static-assignment-form", HelpText = "Performs single static assignments at compile time.")]
		public bool EnableSSAExtraOption
		{
			set { EnableSSA = value; }
		}

		[Option("optimize-ir", HelpText = "Performs single static assignments optimizations.")]
		public bool EnableIROptimizaion { set { CompilerOptions.EnableIROptimizations = value; } }

		[Option("enable-ir-optimizations", HelpText = "Performs single static assignments optimizations.")]
		public bool EnableIROptimizaionExtraOption
		{
			set { EnableIROptimizaion = value; }
		}

		[Option("emit-symbols", HelpText = "Emits the Symbol Table.")]
		public bool EmitSymbols { set { CompilerOptions.EmitSymbols = value; } }

		[Option("emit-relocations", HelpText = "Emits the Relocation Table.")]
		public bool EmitRelocations { set { CompilerOptions.EmitRelocations = value; } }

		[Option("x86-irq-methods", HelpText = "Emits x86 interrupt methods.")]
		public bool EmitX86IRQMethods { set { CompilerOptions.SetCustomOption("x86.irq-methods", value ? "true" : "false"); } }

		[Option("base-address", HelpText = "Specify the base address.")]
		public string BaseAddress { set { CompilerOptions.BaseAddress = value.ParseHexOrDecimal(); } }

		public CompilerOptions CompilerOptions
		{ get; set; }

		public Options()
		{
			InputFiles = new List<FileInfo>();
			CompilerOptions = new CompilerOptions();
		}

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

		private static Func<ICompilerStage> GetBootStageFactory(string format)
		{
			switch (format.ToLower())
			{
				case "multibootHeader-0.7":
				case "mb0.7": return delegate { return new Mosa.Platform.x86.Stages.Multiboot0695Stage(); };
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
