/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
			linker.Commit();
		}

		#endregion ICompilerStage members
	}
}