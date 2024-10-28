// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Compiler.Framework.Common;

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

	public static List<Loop> FindLoops(BasicBlocks basicBlocks, SimpleFastDominance dominance)
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
				if (dominance.IsDominator(block, previous))
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
		var visited = new BlockBitSet(basicBlocks);

		foreach (var backedge in loop.Backedges)
		{
			worklist.Push(backedge);
		}

		loop.AddNode(loop.Header);
		visited.Add(loop.Header);

		while (worklist.Count != 0)
		{
			var node = worklist.Pop();

			if (visited.Contains(node))
				continue;

			visited.Add(node);
			loop.AddNode(node);

			foreach (var previous in node.PreviousBlocks)
			{
				if (previous == loop.Header)
					continue;

				worklist.Push(previous);
			}
		}
	}

	public static void DumpLoops(TraceLog loopTrace, List<Loop> loops)
	{
		if (loopTrace == null)
			return;

		foreach (var loop in loops)
		{
			loopTrace.Log($"Header: {loop.Header}");
			foreach (var backedge in loop.Backedges)
			{
				loopTrace.Log($"   Backedge: {backedge}");
			}

			var sb = new StringBuilder();

			foreach (var block in loop.LoopBlocks)
			{
				sb.Append(block);
				sb.Append(", ");
			}

			sb.Length -= 2;

			loopTrace.Log($"   Members: {sb}");
		}
	}
}
