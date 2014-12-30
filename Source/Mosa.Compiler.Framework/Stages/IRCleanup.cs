/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class IRCleanup : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			// Remove Nops
			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.Instruction == IRInstruction.Nop)
					{
						ctx.Remove();
						continue;
					}
				}
			}

			// copied from EmptyBlockRemovalStage.cs

			// don't remove any blocks given unusual flow control
			if (HasProtectedRegions)
				return;

			//var trace = CreateTraceLog();

			foreach (var block in BasicBlocks)
			{
				// don't process other unusual blocks (header blocks, return block, etc.)
				if (block.NextBlocks.Count == 0 || block.PreviousBlocks.Count == 0)
					continue;

				// don't remove block if it jumps back to itself
				if (block.PreviousBlocks.Contains(block))
					continue;

				if (!IsEmptyBlockWithSingleJump(block))
					continue;

				//if (trace.Active)
				//{
				//	trace.Log("====== Removing: " + block.ToString() + " # " + block.Sequence);
				//	trace.Log("     New Target: " + block.NextBlocks[0].ToString());
				//	foreach (var from in block.PreviousBlocks)
				//	{
				//		trace.Log("Previous Blocks: " + from.ToString());
				//	}
				//}


				RemoveEmptyBlockWithSingleJump(block);
			}
		}
	}
}
