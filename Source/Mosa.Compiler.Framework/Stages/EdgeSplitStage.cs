/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes critical edges by inserting empty basic blocks. Some SSA optimizations and the flow
	///	control resolution in the register allocator require that all critical edges are removed.
	/// </summary>
	public class EdgeSplitStage : BaseEdgeSplitStage, IMethodCompilerStage, IPipelineStage
	{

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Setup(BaseMethodCompiler methodCompiler)
		{
			base.Setup(methodCompiler);

			jumpInstruction = IRInstruction.Jmp;
		}

	}
}