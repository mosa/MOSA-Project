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
				{ 
					SplitEdge(edge.Key, edge.Value); 
				}
			}
		}

		private void SplitEdge(BasicBlock from, BasicBlock to)
		{
			// Create new block z
			Context ctx = CreateNewBlockWithContext();

			architecture.InsertJumpInstruction(ctx, to);
			//ctx.AppendInstruction(jumpInstruction, to);
			ctx.Label = -1;

			var js = ctx.BasicBlock;

			// Unlink blocks
			from.NextBlocks.Remove(to);
			to.PreviousBlocks.Remove(from);

			// Link (from) to js
			from.NextBlocks.Add(js);
			js.PreviousBlocks.Add(from);

			// Link z to (to)
			to.PreviousBlocks.Add(js);
			js.NextBlocks.Add(to);

			ReplaceBranchTargets(from, to, js);
		}
	}
}