/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class ProtectedRegionFlowUpdateStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (!HasProtectedRegions)
				return;

			foreach (var block in BasicBlocks)
			{
				// quick check - small optimization
				if (block.NextBlocks.Count == 0)
					continue;

				for (var context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (context.Instruction != IRInstruction.TryEnd)
						continue;

					// Fix flow control
					Debug.Assert(context.BasicBlock.NextBlocks.Count == 1);

					var nextBlock = block.NextBlocks[0];

					block.NextBlocks.Clear();
					nextBlock.PreviousBlocks.Remove(block);

					if (nextBlock.PreviousBlocks.Count >= 1)
					{
						// need to create a new head
						var head = CreateNewBlockWithContext();

						head.AppendInstruction(IRInstruction.Jmp, nextBlock);
						LinkBlocks(head, nextBlock);

						BasicBlocks.AddHeaderBlock(head.BasicBlock);
					}
					else
					{
						Debug.Assert(!BasicBlocks.HeadBlocks.Contains(nextBlock));

						BasicBlocks.AddHeaderBlock(nextBlock);
					}

					break;
				}
			}
		}
	}
}
