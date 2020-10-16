// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// The Block Ordering Stage reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public class BlockOrderingStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var blockOrderAnalysis = new LoopAwareBlockOrder();

			blockOrderAnalysis.Analyze(BasicBlocks);

			var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

			if (HasProtectedRegions)
			{
				newBlockOrder = AddMissingBlocksIfRequired(newBlockOrder);
			}

			BasicBlocks.ReorderBlocks(newBlockOrder);

			DumpTrace(blockOrderAnalysis);
		}

		private void DumpTrace(LoopAwareBlockOrder blockOrderAnalysis)
		{
			var trace = CreateTraceLog();

			if (trace == null)
				return;

			int index = 0;

			foreach (var block in blockOrderAnalysis.NewBlockOrder)
			{
				if (block != null)
					trace.Log($"# {index.ToString()} Block {block} #{block.Sequence.ToString()}");
				else
					trace.Log($"# {index.ToString()} NONE");

				index++;
			}

			trace.Log();

			foreach (var block in BasicBlocks)
			{
				int depth = blockOrderAnalysis.GetLoopDepth(block);
				int depthindex = blockOrderAnalysis.GetLoopIndex(block);

				trace.Log($"Block {block} #{block.Sequence.ToString()} -> Depth: {depth.ToString()} Index: {depthindex.ToString()}");
			}
		}
	}
}
