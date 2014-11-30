/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Linker;
using System;

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

			//public uint? FileAlignment { get; set; }
			//public uint? SectionAlignment { get; set; }
		}

		/// <summary>
		///  Struct for ELF32 options.
		/// </summary>
		public struct Elf32Struct
		{
			//public uint FileAlignment { get; set; }
		}

		#endregion Structures

		#region Properties

		/// <summary>
		/// Gets or sets the base address.
		/// </summary>
		/// <value>
		/// The base address.
		/// </value>
		public ulong BaseAddress { get; set; }

		/// <summary>
		/// Gets or sets the architecture.
		/// </summary>
		/// <value>The architecture.</value>
		public BaseArchitecture Architecture { get; set; }

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile { get; set; }

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
		public bool EnableOptimizations { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable promote temporary variables optimization].
		/// </summary>
		/// <value>
		/// <c>true</c> if [enable promote temporary variables optimization]; otherwise, <c>false</c>.
		/// </value>
		public bool EnablePromoteTemporaryVariablesOptimization { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable conditional constant propagation].
		/// </summary>
		/// <value>
		/// <c>true</c> if [enable conditional constant propagation]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableSparseConditionalConstantPropagation { get; set; }

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
		/// Gets or sets the dominance analysis factory.
		/// </summary>
		/// <value>
		/// The dominance analysis factory.
		/// </value>
		public Func<IDominanceAnalysis> DominanceAnalysisFactory { get; set; }

		/// <summary>
		/// Gets or sets the block order analysis.
		/// </summary>
		/// <value>
		/// The block order analysis.
		/// </value>
		public Func<IBlockOrderAnalysis> BlockOrderAnalysisFactory { get; set; }

		/// <summary>
		/// Gets or sets the linker factory.
		/// </summary>
		/// <value>
		/// The linker factory.
		/// </value>
		public Func<BaseLinker> LinkerFactory { get; set; }

		/// <summary>
		/// Gets or sets the compiler stage responsible for booting.
		/// </summary>
		public Func<ICompilerStage> BootStageFactory { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [emit binary].
		/// </summary>
		/// <value>
		///   <c>true</c> if [emit binary]; otherwise, <c>false</c>.
		/// </value>
		public bool EmitBinary { get; set; }

		#endregion Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerOptions"/> class.
		/// </summary>
		public CompilerOptions()
		{
			EnableSSA = true;
			EnableOptimizations = true;
			EnablePromoteTemporaryVariablesOptimization = true;
			EnableSparseConditionalConstantPropagation = true;
			BaseAddress = 0x00400000;
			DominanceAnalysisFactory = delegate { return new SimpleFastDominance(); };
			BlockOrderAnalysisFactory = delegate { return new LoopAwareBlockOrder(); };
			EmitBinary = true;
		}
	}
}