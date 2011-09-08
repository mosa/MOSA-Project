/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework
{

	public class CompilerOptions
	{

		#region Structures

		public struct MultibootStruct
		{
			public uint? VideoMode { get; set; }
			public uint? VideoWidth { get; set; }
			public uint? VideoHeight { get; set; }
			public uint? VideoDepth { get; set; }
		}

		public struct PortableExecutableStruct
		{
			public bool? SetChecksum { get; set; }
			public uint? FileAlignment { get; set; }
			public uint? SectionAlignment { get; set; }
		}

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
		public IAssemblyLinker Linker { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public IAssemblyCompilerStage BootCompilerStage { get; set; }

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
		/// Gets or sets a value indicating whether [enable SSA].
		/// </summary>
		/// <value><c>true</c> if [enable SSA]; otherwise, <c>false</c>.</value>
		public bool EnableSSA { get; set; }

		/// <summary>
		/// Gets or sets the multiboot.
		/// </summary>
		/// <value>The multiboot.</value>
		public MultibootStruct Multiboot;

		/// <summary>
		/// Gets or sets the elf32.
		/// </summary>
		/// <value>The elf32.</value>
		public Elf32Struct Elf32;

		/// <summary>
		/// Gets or sets the portable executable.
		/// </summary>
		/// <value>The portable executable.</value>
		public PortableExecutableStruct PortableExecutable;

		#endregion // Properties

		public CompilerOptions()
		{
			EnableSSA = false;
		}
	}
}
