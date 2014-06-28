/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes empty blocks.
	/// </summary>
	public class EmptyBlockRemovalStage : BaseMethodCompilerStage
	{
		protected BaseInstruction jumpInstruction;

		protected override void Run()
		{
			var trace = CreateTrace();

			List<Tuple<BasicBlock, BasicBlock>> worklist = new List<Tuple<BasicBlock, BasicBlock>>();

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

				RemoveBlock(block, trace);
			}
		}

		private void RemoveBlock(BasicBlock block, CompilerTrace trace)
		{
			Debug.Assert(block.NextBlocks.Count == 1);

			BasicBlock target = block.NextBlocks[0];

			if (trace.Active)
			{
				trace.Log("====== Removing: " + block.ToString() + " # " + block.Sequence);
				trace.Log("     New Target: " + target.ToString());
				foreach (var from in block.PreviousBlocks)
				{
					trace.Log("Previous Blocks: " + from.ToString());
				}
			}

			target.PreviousBlocks.Remove(block);

			foreach (var from in block.PreviousBlocks)
			{
				from.NextBlocks.Remove(block);
				from.NextBlocks.AddIfNew(target);

				if (trace.Active)
				{
					trace.Log("  Add target destinationstination NextBlock of " + from.ToString());
				}

				target.PreviousBlocks.AddIfNew(from);

				if (trace.Active)
				{
					trace.Log("  Add " + from.ToString() + " destinationstination PreviousBlock of " + target.ToString());
				}

				ReplaceBranchTargets(from, block, target);
			}

			block.NextBlocks.Clear();
			block.PreviousBlocks.Clear();

			EmptyBlockOfAllInstructions(block);
		}
	}
}