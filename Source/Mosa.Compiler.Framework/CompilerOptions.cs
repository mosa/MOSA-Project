/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework
{

	public class CompilerOptions
	{

		#region Structures

		/// <summary>
		/// Struct for multiboot options.
		/// </summary>
		public struct MultibootStruct
		{
			public uint? VideoMode { get; set; }
			public uint? VideoWidth { get; set; }
			public uint? VideoHeight { get; set; }
			public uint? VideoDepth { get; set; }
		}

		/// <summary>
		/// Struct for PE options.
		/// </summary>
		public struct PortableExecutableStruct
		{
			public bool? SetChecksum { get; set; }
			public uint? FileAlignment { get; set; }
			public uint? SectionAlignment { get; set; }
		}

		/// <summary>
		///  Struct for ELF32 options.
		/// </summary>
		public struct Elf32Struct
		{
			public uint FileAlignment { get; set; }
		}

		#endregion // Structures

		#region Properties

		/// <summary>
		/// Gets or sets the architecture.
		/// </summary>
		/// <value>The architecture.</value>
		public IArchitecture Architecture { get; set; }

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile { get; set; }

		/// <summary>
		/// Gets or sets the linker stage.
		/// </summary>
		/// <value>The linker stage.</value>
		public ILinker Linker { get; set; }

		/// <summary>
		/// Gets or sets the compiler stage responsible for booting.
		/// </summary>
		public ICompilerStage BootCompilerStage { get; set; }

		/// <summary>
		/// Gets or sets the statistics file.
		/// </summary>
		/// <value>The statistics file.</value>
		public string StatisticsFile { get; set; }

		/// <summary>
		/// Gets or sets the map file.
		/// </summary>
		/// <value>The map file.</value>
		public string MapFile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether SSA is enabled.
		/// </summary>
		/// <value><c>true</c> if SSA is enabled; otherwise, <c>false</c>.</value>
		public bool EnableSSA { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable SSA optimizations].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [enable SSA optimizations]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableSSAOptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether static allocations are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if static allocations are enabled; otherwise, <c>false</c>.
		/// </value>
		public bool EnableStaticAllocations { get; set; }

		/// <summary>
		/// Holds a struct with additional options for Multiboot.
		/// </summary>
		/// <value>The multiboot struct.</value>
		public MultibootStruct Multiboot;

		/// <summary>
		/// Holds a struct with additional options for ELF32.
		/// </summary>
		/// <value>The ELF32 struct.</value>
		public Elf32Struct Elf32;

		/// <summary>
		/// Holds a struct with additional options for the PE format.
		/// </summary>
		/// <value>The portable executable (PE) struct.</value>
		public PortableExecutableStruct PortableExecutable;

		/// <summary>
		/// Gets or sets the method pipeline export directory, used for debugging
		/// </summary>
		public string MethodPipelineExportDirectory { get; set; }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerOptions"/> class.
		/// </summary>
		public CompilerOptions()
		{
			EnableSSA = false;
			EnableSSAOptimizations = true;
		}
	}
}
