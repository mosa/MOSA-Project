// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Tool.Compiler
{
	internal class Options
	{
		public List<FileInfo> InputFiles { get; }
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

					var file = new FileInfo(v);
					if (string.Equals(file.Extension, ".exe", StringComparison.OrdinalIgnoreCase))
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

		[Option('a', "architecture", HelpText = "Select one of the MOSA architectures to compile for [x86|x64|ARMv6].", Required = true)]
		public string Architecture { set { CompilerOptions.Architecture = SelectArchitecture(value); } }

		[Option("mboot", HelpText = "Select multiboot specification [v1|v2].")]
		public string Boot { set { CompilerOptions.MultibootSpecification = GetMultibootSpecification(value); } }

		[Option('f', "format", HelpText = "Select the format of the binary file to create [ELF32|ELF64].")]
		public string LinkerFormat { set { CompilerOptions.LinkerFormatType = GetLinkerFactory(value); } }

		[Option('o', "out", HelpText = "The name of the output file.", Required = true)]
		public string OutputFile { set { CompilerOptions.OutputFile = value; } }

		[Option("map", HelpText = "Generate a map file of the produced binary.")]
		public string MapFile { set { CompilerOptions.MapFile = value; } }

		[Option("output-time")]
		public string CompileTimeFile { set { CompilerOptions.CompileTimeFile = value; } }

		[Option("debug-info", HelpText = "Generate a debug info file of the produced binary.")]
		public string DebugInfoFile { set { CompilerOptions.DebugFile = value; } }

		[Option("sa", HelpText = "Performs static allocations at compile time.")]
		public bool EnableStaticAllocation { set { CompilerOptions.EnableStaticAllocations = value; } }

		[Option("enable-static-alloc", HelpText = "Performs static allocations at compile time.")]
		public bool EnableStaticAllocationTrue { set { EnableStaticAllocation = value; } }

		[Option("ssa", HelpText = "Performs single static assignments at compile time.")]
		public bool EnableSSA { set { CompilerOptions.EnableSSA = value; } }

		[Option("enable-single-static-assignment-form", HelpText = "Performs single static assignments at compile time.")]
		public bool EnableSSATrue { set { EnableSSA = value; } }

		[Option("optimize-ir", HelpText = "Performs ir-level optimizations.")]
		public bool EnableIROptimizaion { set { CompilerOptions.EnableIROptimizations = value; } }

		[Option("enable-ir-optimizations", HelpText = "Performs ir-level optimizations.")]
		public bool EnableIROptimizaionAlt { set { EnableIROptimizaion = value; } }

		[Option("emit-all-symbols", HelpText = "Emits all the symbols in the Symbol Table.")]
		public bool EmitAllSymbols { set { CompilerOptions.EmitAllSymbols = value; } }

		[Option("emit-static-relocations", HelpText = "Emits the static symbols in Relocation Table.")]
		public bool EmitStaticRelocations { set { CompilerOptions.EmitStaticRelocations = value; } }

		[Option("x86-irq-methods", HelpText = "Emits x86 interrupt methods.")]
		public bool EmitX86IRQMethods { set { CompilerOptions.SetCustomOption("x86.irq-methods", value ? "true" : "false"); } }

		[Option("base-address", HelpText = "Specify the base address.")]
		public string BaseAddress { set { CompilerOptions.BaseAddress = value.ParseHexOrInteger(); } }

		public CompilerOptions CompilerOptions { get; set; }

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
				case "x64": return Mosa.Platform.x64.Architecture.CreateArchitecture(Mosa.Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				case "armv6": return Mosa.Platform.ARMv6.Architecture.CreateArchitecture(Mosa.Platform.ARMv6.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException(string.Format("Unknown or unsupported Architecture {0}.", architecture));
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

		private static MultibootSpecification GetMultibootSpecification(string format)
		{
			switch (format.ToLower())
			{
				case "v1": return Mosa.Compiler.Framework.MultibootSpecification.V1;
				case "v2": return Mosa.Compiler.Framework.MultibootSpecification.V2;
				default: throw new NotImplementCompilerException(string.Format("Unknown or unsupported MultibootSpecification {0}.", format));
			}
		}

		#endregion Internal Methods
	}
}
