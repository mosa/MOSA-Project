/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Represents compiler generated methods.
	/// </summary>
	public sealed class LinkerFinalizationStage : BaseCompilerStage, ICompilerStage
	{

		#region ICompilerStage members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		void ICompilerStage.Run()
		{
			this.linker.Finalize();
		}

		#endregion // ICompilerStage members

	}
}
