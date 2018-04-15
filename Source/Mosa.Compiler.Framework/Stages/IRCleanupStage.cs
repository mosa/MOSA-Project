// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

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
			MergeBlocks();
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

		private void MergeBlocks()
		{
			var changed = true;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					if (block.IsEpilogue
						|| block.IsPrologue
						|| block.IsTryHeadBlock
						|| block.IsHandlerHeadBlock
						|| !block.IsCompilerBlock)
						continue;

					if (block.NextBlocks.Count != 1)
						continue;

					var next = block.NextBlocks[0];

					if (next.PreviousBlocks.Count != 1)
						continue;

					if (next.IsEpilogue
						|| next.IsPrologue
						|| next.IsTryHeadBlock
						|| next.IsHandlerHeadBlock
						|| !next.IsCompilerBlock)
						continue;

					var insertPoint = block.BeforeLast.GoBackwardsToNonEmpty();

					var beforeInsertPoint = insertPoint.Previous.GoBackwardsToNonEmpty();

					if (beforeInsertPoint.BranchTargetsCount != 0)
					{
						Debug.Assert(beforeInsertPoint.BranchTargets[0] == next);
						beforeInsertPoint.Empty();
					}

					insertPoint.Empty();
					insertPoint.CutFrom(next.First.Next.GoForwardToNonEmpty(), next.Last.Previous.GoBackwardsToNonEmpty());
					changed = true;
				}
			}
		}
	}
}
