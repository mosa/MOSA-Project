// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Cleanup Stage
	/// </summary>
	public class IRCleanupStage : EmptyBlockRemovalStage
	{
		protected override void Run()
		{
			RemoveNops();
			RemoveEmptyBlocks();
			OrderBlocks();
		}

		private void OrderBlocks()
		{
			//var blockOrderAnalysis = new SimpleTraceBlockOrder();   // faster than others
			var blockOrderAnalysis = new LoopAwareBlockOrder();

			blockOrderAnalysis.PerformAnalysis(BasicBlocks);

			var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

			if (HasProtectedRegions)
			{
				newBlockOrder = AddMissingBlocks(newBlockOrder, true);
			}

			BasicBlocks.ReorderBlocks(newBlockOrder);
		}

		private void RemoveNops()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction == IRInstruction.Nop)
					{
						node.Empty();
					}
				}
			}
		}
	}
}
