/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Stage used for investigating operand usage in MOSA (experimental)
	/// </summary>
	public class OperandUsageAnalyzerStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Create a list of end blocks
			List<BasicBlock> endBlocks = new List<BasicBlock>();

			foreach (var block in this.basicBlocks)
			{
				if (block.NextBlocks.Count == 0)
					endBlocks.Add(block);
			}

			return;

		}

	}
}
