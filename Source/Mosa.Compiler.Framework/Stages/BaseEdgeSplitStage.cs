/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes critical edges by inserting empty basic blocks. Some SSA optimizations and the flow
	///	control resolution in the register allocator require that all critical edges are removed.
	/// </summary>
	public class BaseEdgeSplitStage : BaseMethodCompilerStage
	{
		protected BaseInstruction jumpInstruction;

		public virtual void Run()
		{
			List<KeyValuePair<BasicBlock, BasicBlock>> worklist = new List<KeyValuePair<BasicBlock, BasicBlock>>();

			foreach (var from in basicBlocks)
			{
				if (from.NextBlocks.Count > 1)
				{
					foreach (var to in from.NextBlocks)
					{
						if (to.PreviousBlocks.Count > 1)
						{
							worklist.Add(new KeyValuePair<BasicBlock, BasicBlock>(from, to));
						}
					}
				}
			}

			foreach (var edge in worklist)
			{
				if (edge.Key.NextBlocks.Count > 1 || edge.Value.PreviousBlocks.Count > 1)
					SplitEdge(edge.Key, edge.Value);
			}
		}

		private void SplitEdge(BasicBlock a, BasicBlock b)
		{
			// Create new block z
			Context ctx = CreateNewBlockWithContext();
			ctx.AppendInstruction(jumpInstruction, b);
			ctx.Label = -1;

			var z = ctx.BasicBlock;

			// Unlink blocks
			a.NextBlocks.Remove(b);
			b.PreviousBlocks.Remove(a);

			// Link a to z
			a.NextBlocks.Add(z);
			z.PreviousBlocks.Add(a);

			// Link z to b
			b.PreviousBlocks.Add(z);
			z.NextBlocks.Add(b);

			// Replace any jump/branch target in block a with z
			ctx = new Context(instructionSet, a, a.EndIndex);

			Debug.Assert(ctx.IsBlockEndInstruction);

			ctx.GotoPrevious();

			// Find branch or jump to b and replace it with z
			while (ctx.BranchTargets != null)
			{
				int[] targets = ctx.BranchTargets;
				for (int index = 0; index < targets.Length; index++)
				{
					if (targets[index] == b.Label)
						targets[index] = z.Label;
				}
				ctx.GotoPrevious();
			}
		}
	}
}