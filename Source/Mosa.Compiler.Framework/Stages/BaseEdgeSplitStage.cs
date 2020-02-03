// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes critical edges by inserting empty basic blocks. Some SSA optimizations and the flow
	///	control resolution in the register allocator require that all critical edges are removed.
	/// </summary>
	public class BaseEdgeSplitStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var worklist = new List<Tuple<BasicBlock, BasicBlock>>();

			foreach (var from in BasicBlocks)
			{
				if (from.NextBlocks.Count > 1)
				{
					foreach (var to in from.NextBlocks)
					{
						if (to.PreviousBlocks.Count > 1)
						{
							worklist.Add(new Tuple<BasicBlock, BasicBlock>(from, to));
						}
					}
				}
			}

			foreach (var edge in worklist)
			{
				if (edge.Item1.NextBlocks.Count > 1 || edge.Item2.PreviousBlocks.Count > 1)
				{
					SplitEdge(edge.Item1, edge.Item2);
				}
			}
		}

		protected virtual void InsertJumpInstruction(Context context, BasicBlock block)
		{
			context.AppendInstruction(IRInstruction.Jmp, block);
		}

		private void SplitEdge(BasicBlock from, BasicBlock to)
		{
			// Create new block z
			var ctx = CreateNewBlockContext(to.First.Label);

			InsertJumpInstruction(ctx, to);

			ctx.Label = -1;

			ReplaceBranchTargets(from, to, ctx.Block);
		}
	}
}
