// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Common;
using Mosa.Compiler.Framework.Trace;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Value Numbering Stage
	/// </summary>
	public sealed class LoopInvariantCodeMotionStage : BaseMethodCompilerStage
	{
		private Counter CodeMotionCount = new Counter("LoopInvariantCodeMotionStage.CodeMotionCount");

		private SimpleFastDominance AnalysisDominance;

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(CodeMotionCount);
		}

		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog(5);

			AnalysisDominance = new SimpleFastDominance(BasicBlocks, BasicBlocks.PrologueBlock);

			var loops = FindLoops();

			if (trace.Active) DumpLoops(loops);
		}

		protected override void Finish()
		{
			AnalysisDominance = null;
		}

		private List<Loop> FindLoops()
		{
			var loops = new List<Loop>();
			var lookup = new Dictionary<BasicBlock, Loop>();

			foreach (var block in BasicBlocks)
			{
				if (block.PreviousBlocks.Count == 0)
					continue;

				foreach (var previous in block.PreviousBlocks)
				{
					// Is this a back-edge? Yes, if "block" dominates "previous"
					if (AnalysisDominance.IsDominator(block, previous))
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
				PopulateLoopNodes(loop);
			}

			return loops;
		}

		private void PopulateLoopNodes(Loop loop)
		{
			var worklist = new Stack<BasicBlock>();
			var array = new BitArray(BasicBlocks.Count);

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

		public void DumpLoops(List<Loop> loops)
		{
			foreach (var loop in loops)
			{
				trace.Log($"Header: {loop.Header}");
				foreach (var backedge in loop.Backedges)
				{
					trace.Log($"   Backedge: {backedge}");
				}

				var sb = new StringBuilder();

				foreach (var block in loop.LoopBlocks)
				{
					sb.Append(block);
					sb.Append(", ");
				}

				sb.Length -= 2;

				trace.Log($"   Members: {sb}");
			}
		}
	}
}
