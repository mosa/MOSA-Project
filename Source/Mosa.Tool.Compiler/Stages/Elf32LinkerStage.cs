/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.LinkerFormat.Elf;

namespace Mosa.Tool.Compiler.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32LinkerStage : Mosa.Compiler.Linker.Elf32.Linker, IPipelineStage, IAssemblyCompilerStage, IAssemblyLinker
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Executable and Linking Format (ELF) Linker [32 Bit]"; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage members

		public void Setup(AssemblyCompiler compiler)
		{
			this.OutputFile = compiler.CompilerOptions.OutputFile;
			this.FileAlignment = compiler.CompilerOptions.Elf32.FileAlignment;
			this.IsLittleEndian = compiler.Architecture.IsLittleEndian;
			this.Machine = (MachineType)compiler.Architecture.ElfMachineType;
		}

		#endregion // IAssemblyCompilerStage members
	}
}