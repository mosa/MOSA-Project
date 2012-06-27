/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This class orders blocks in reverse order. This stage is used for testing.
	/// </summary>
	public class ReverseBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{
		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			List<BasicBlock> reversed = new List<BasicBlock>(basicBlocks.Count);
			var prologueBlock = basicBlocks.PrologueBlock;
			reversed.Add(prologueBlock);

			for (int i = 0; i < basicBlocks.Count; i++)
			{
				if (basicBlocks[i] != prologueBlock)
					reversed.Add(basicBlocks[i]);
			}

			basicBlocks.ReorderBlocks(reversed);
		}
	}
}
