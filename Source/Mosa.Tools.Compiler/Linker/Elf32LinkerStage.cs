/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using Mosa.Compiler.Linker;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.Linker
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf64LinkerStage : Elf64Linker, IPipelineStage, IAssemblyCompilerStage, IAssemblyLinker
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Executable and Linking Format (ELF) Linker [64 Bit]"; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage members

		public void Setup(AssemblyCompiler compiler)
		{
			// Nothing
		}

		#endregion // IAssemblyCompilerStage members
	}
}