// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework.Analysis;

/// <summary>
/// Loop Detector
/// </summary>
public sealed class LoopDetector
{
	public static List<Loop> FindLoops(BasicBlocks basicBlocks)
	{
		var dominance = new SimpleFastDominance(basicBlocks, basicBlocks.PrologueBlock);

		return FindLoops(basicBlocks, dominance);
	}

	public static List<Loop> FindLoops(BasicBlocks basicBlocks, SimpleFastDominance analysisDominance)
	{
		var loops = new List<Loop>();
		var lookup = new Dictionary<BasicBlock, Loop>();

		foreach (var block in basicBlocks)
		{
			if (block.PreviousBlocks.Count == 0)
				continue;

			foreach (var previous in block.PreviousBlocks)
			{
				// Is this a back-edge? Yes, if "block" dominates "previous"
				if (analysisDominance.IsDominator(block, previous))
				{
					if (lookup.TryGetValue(block, out Loop loop))
					{
						loop.AddBackEdge(previous);
					}
					else
					{
						loop = new Loop(block, previous);
						loops.Add(loop);
						lookup.Add(block, loop);
					}
				}
			}
		}

		foreach (var loop in loops)
		{
			PopulateLoopNodes(basicBlocks, loop);
		}

		return loops;
	}

	private static void PopulateLoopNodes(BasicBlocks basicBlocks, Loop loop)
	{
		var worklist = new Stack<BasicBlock>();
		var array = new BitArray(basicBlocks.Count);

		foreach (var backedge in loop.Backedges)
		{
			worklist.Push(backedge);
		}

		loop.AddNode(loop.Header);
		array.Set(loop.Header.Sequence, true);

		while (worklist.Count != 0)
		{
			var node = worklist.Pop();

			if (!array.Get(node.Sequence))
			{
				array.Set(node.Sequence, true);
				loop.LoopBlocks.Add(node);

				foreach (var previous in node.PreviousBlocks)
				{
					if (previous == loop.Header)
						continue;

					worklist.Push(previous);
				}
			}
		}
	}
}
