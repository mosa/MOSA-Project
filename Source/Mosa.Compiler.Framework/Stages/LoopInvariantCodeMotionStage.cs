// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Basics;
using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Value Numbering Stage
	/// </summary>
	public sealed class LoopInvariantCodeMotionStage : BaseMethodCompilerStage
	{
		private SimpleFastDominance AnalysisDominance;

		private TraceLog trace;

		private Counter codeMotionCount = new Counter("LoopInvariantCodeMotionStage.CodeMotionCount");

		protected override void Setup()
		{
			codeMotionCount.Reset();
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
		}

		protected override void Finish()
		{
			AnalysisDominance = null;
		}

		private List<Loop> FindLoops()
		{
			List<Loop> loops = null;

			foreach (var block in BasicBlocks)
			{
				if (block.PreviousBlocks.Count == 0)
					continue;

				foreach (var previous in block.PreviousBlocks)
				{
					// Is this a back-edge? Yes, if "block" dominates "previous"
					if (AnalysisDominance.IsDominator(block, previous))
					{
						var loop = new Loop(block, previous);

						(loops ?? (loops = new List<Loop>())).Add(loop);
					}
				}
			}

			return loops;
		}
	}
}
