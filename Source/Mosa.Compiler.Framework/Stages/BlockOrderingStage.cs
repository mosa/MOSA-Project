// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
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
			var blockOrderAnalysis = MethodCompiler.Compiler.CompilerOptions.BlockOrderAnalysisFactory();

			blockOrderAnalysis.PerformAnalysis(BasicBlocks);

			var newBlockOrder = blockOrderAnalysis.NewBlockOrder;

			if (HasProtectedRegions)
			{
				newBlockOrder = AddMissingBlocks(newBlockOrder);
			}

			BasicBlocks.ReorderBlocks(newBlockOrder);

			DumpTrace(blockOrderAnalysis);
		}

		private IList<BasicBlock> AddMissingBlocks(IList<BasicBlock> blocks)
		{
			if (blocks.Contains(null))
				return blocks;

			var list = new List<BasicBlock>();

			foreach (var block in blocks)
			{
				if (block != null)
				{
					list.Add(block);
				}
			}

			foreach (var block in BasicBlocks)
			{
				if (!blocks.Contains(block))
				{
					list.Add(block);
				}
			}

			return list;
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
