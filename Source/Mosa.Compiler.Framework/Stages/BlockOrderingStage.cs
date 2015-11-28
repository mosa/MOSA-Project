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
			if (HasProtectedRegions)
			{
				// exception handlers may point to empty/unreferenced blocks
				return;
			}

			var blockOrderAnalysis = MethodCompiler.Compiler.CompilerOptions.BlockOrderAnalysisFactory();

			blockOrderAnalysis.PerformAnalysis(BasicBlocks);

			if (HasProtectedRegions & blockOrderAnalysis.NewBlockOrder.Count != BasicBlocks.Count)
			{
				// exception handlers may point to removed blocks
				return;
			}

			BasicBlocks.ReorderBlocks(blockOrderAnalysis.NewBlockOrder);

			DumpTrace(blockOrderAnalysis);
		}

		private void DumpTrace(IBlockOrderAnalysis blockOrderAnalysis)
		{
			var trace = CreateTraceLog();

			if (!trace.Active)
				return;

			int index = 0;

			foreach (var block in blockOrderAnalysis.NewBlockOrder)
			{
				if (block != null)
					trace.Log("# " + index.ToString() + " Block " + block.ToString() + " #" + block.Sequence.ToString());
				else
					trace.Log("# " + index.ToString() + " NONE");

				index++;
			}

			trace.Log(string.Empty);

			foreach (var block in BasicBlocks)
			{
				int depth = blockOrderAnalysis.GetLoopDepth(block);
				int depthindex = blockOrderAnalysis.GetLoopIndex(block);

				trace.Log("Block " + block.ToString() + " #" + block.Sequence.ToString() + " -> Depth: " + depth.ToString() + " Index: " + depthindex.ToString());
			}
		}
	}
}
