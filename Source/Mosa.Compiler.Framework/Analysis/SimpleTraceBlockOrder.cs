// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Common;

namespace Mosa.Compiler.Framework.Analysis;

/// <summary>
/// The Simple Trace Block Order quickly reorders blocks to optimize loops and reduce the distance of jumps and branches.
/// </summary>
public class SimpleTraceBlockOrder : BaseBlockOrder
{
	public override int GetLoopDepth(BasicBlock block)
	{
		return 0;
	}

	public override int GetLoopIndex(BasicBlock block)
	{
		return 0;
	}

	public override void Analyze(BasicBlocks basicBlocks)
	{
		// Create dictionary of referenced blocks
		var referenced = new BlockBitSet(basicBlocks);

		// Allocate list of ordered Blocks
		NewBlockOrder = new List<BasicBlock>(basicBlocks.Count);

		// Create sorted worklist
		var workList = new Stack<BasicBlock>();

		foreach (var head in basicBlocks.HeadBlocks)
		{
			workList.Push(head);

			while (workList.Count != 0)
			{
				var block = workList.Pop();

				if (referenced.Contains(block))
					continue;

				referenced.Add(block);
				NewBlockOrder.Add(block);

				if (block.NextBlocks.Count == 0)
					continue;

				var nextBlocks = (block.NextBlocks.Count == 2 && block.NextBlocks[0].Label < block.NextBlocks[1].Label)
					? new List<BasicBlock>() { block.NextBlocks[1], block.NextBlocks[0] }
					: block.NextBlocks;

				foreach (var successor in nextBlocks)
				{
					if (!referenced.Contains(successor))
					{
						workList.Push(successor);
					}
				}
			}
		}
	}
}
