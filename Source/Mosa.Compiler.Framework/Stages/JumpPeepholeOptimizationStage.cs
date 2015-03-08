/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Marcelo Caetano (marcelocaetano) <marcelo.caetano@ymail.com>
 */

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
			//if (HasProtectedRegions)
			//	return;

			var trace = CreateTraceLog();

			for (int f = 0; f < BasicBlocks.Count - 1; f++)
			{
				var from = BasicBlocks[f];
				var node = from.BeforeLast;

				while (node.IsEmpty)
				{
					node = node.Previous;
				}

				if (node.Instruction.FlowControl != FlowControl.UnconditionalBranch)
					continue;

				if (node.BranchTargetsCount == 0)
					continue;

				Debug.Assert(node.Instruction.FlowControl == FlowControl.UnconditionalBranch);

				var next = BasicBlocks[f + 1];
				var target = node.BranchTargets[0];

				if (next != target)
					continue;

				node.Empty();
			}
		}
	}
}
