// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Cleanup Stage
	/// </summary>
	public class IRCleanupStage : DeadBlockStage
	{
		protected override void Run()
		{
			RemoveNops();
			EmptyDeadBlocks();
			SkipEmptyBlocks();
			OrderBlocks();
		}

		private void OrderBlocks()
		{
			//var blockOrderAnalysis = new LoopAwareBlockOrder();
			var blockOrderAnalysis = new SimpleTraceBlockOrder();   // faster than others

			blockOrderAnalysis.Analyze(BasicBlocks);

			var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

			if (newBlockOrder.Count != BasicBlocks.Count && HasProtectedRegions)
			{
				newBlockOrder = AddMissingBlocksIfRequired(newBlockOrder);
			}

			BasicBlocks.ReorderBlocks(newBlockOrder);
		}

		private void RemoveNops()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.IsNop)
					{
						node.Empty();
					}
				}
			}
		}
	}
}
