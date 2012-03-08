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

namespace Mosa.Tool.Compiler.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public class PortableExecutableLinkerStage : Mosa.Compiler.Linker.PE.Linker, IPipelineStage, IAssemblyCompilerStage, IAssemblyLinker
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>

		string IPipelineStage.Name { get { return @"Portable Executable File Linker"; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage members

		public void Setup(AssemblyCompiler compiler)
		{
			this.OutputFile = compiler.CompilerOptions.OutputFile;
			
			if (compiler.CompilerOptions.PortableExecutable.FileAlignment.HasValue)
				this.FileAlignment = compiler.CompilerOptions.PortableExecutable.FileAlignment.Value;

			if (compiler.CompilerOptions.PortableExecutable.SectionAlignment.HasValue)
				this.SectionAlignment = compiler.CompilerOptions.PortableExecutable.SectionAlignment.Value;

			if (compiler.CompilerOptions.PortableExecutable.SetChecksum.HasValue)
				this.SetChecksum = compiler.CompilerOptions.PortableExecutable.SetChecksum.Value;
		}

		#endregion // IAssemblyCompilerStage members
	}
}