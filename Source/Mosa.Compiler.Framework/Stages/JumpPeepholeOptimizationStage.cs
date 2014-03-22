/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Marcelo Caetano (marcelocaetano) <marcelo.caetano@ymail.com>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stages removes jumps to the next instruction
	/// </summary>
	public sealed class JumpPeepholeOptimizationStage : BaseCodeTransformationStage
	{
		// TODO:
		// 1. If first branch is to the next basic block,
		//       then swap branch condition, replace branch target with jump target, and remove jump instruction.
		//           part of code: ConditionCode = GetOppositeConditionCode(ConditionCode);
		// 2. If the basic block contains only a single jump instruction, rewrite all jumps to avoid it.

		protected override void Run()
		{
			var trace = CreateTrace();

			for (int f = 0; f < BasicBlocks.Count - 1; f++)
			{
				var from = BasicBlocks[f];
				var next = BasicBlocks[f + 1];

				Context context = new Context(InstructionSet, from, from.EndIndex);
				context.GotoPrevious();

				while (context.IsEmpty)
				{
					context.GotoPrevious();
				}

				if (context.Instruction.FlowControl != FlowControl.UnconditionalBranch)
					continue;

				Debug.Assert(context.Instruction.FlowControl == FlowControl.UnconditionalBranch);
				Debug.Assert(context.BranchTargets.Length == 1);

				var target = context.BranchTargets[0];

				if (next.Label != target)
					continue;

				context.Remove();
			}
		}

	}
}