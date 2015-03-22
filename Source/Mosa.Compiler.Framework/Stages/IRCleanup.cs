/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class IRCleanup : EmptyBlockRemovalStage
	{
		protected override void Run()
		{
			RemoveEmptyBlocks();

			// TODO: merge block A & B, where A.Next contains only B, and B.Previous contains only A.

			OrderBlocks();
			RemoveNops();
		}

		private void OrderBlocks()
		{
			// don't remove any blocks when the flow control is unusual
			if (HasProtectedRegions)
				return;

			var blockOrderAnalysis = new LoopAwareBlockOrder();

			blockOrderAnalysis.PerformAnalysis(BasicBlocks);

			BasicBlocks.ReorderBlocks(blockOrderAnalysis.NewBlockOrder);
		}

		private void RemoveNops()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					// Remove Nops
					if (node.Instruction == IRInstruction.Nop)
					{
						node.Empty();
					}
				}
			}
		}
	}
}