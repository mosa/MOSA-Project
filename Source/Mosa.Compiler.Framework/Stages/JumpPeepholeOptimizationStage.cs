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
    public sealed class JumpPeepholeOptimizationStage : BaseCodeTransformationStage, IMethodCompilerStage
	{
		// TODO:
		// 1. If first branch is to the next basic block,
		//       then swap branch condition, replace branch target with jump target, and remove jump instruction.
		//           part of code: ConditionCode = GetOppositeConditionCode(ConditionCode);
		// 2. If the basic block contains only a single jump instruction, rewrite all jumps to avoid it.

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			var trace = CreateTrace();

			for (int f = 0; f < basicBlocks.Count - 1; f++)
			{
				var from = basicBlocks[f];
				var next = basicBlocks[f + 1];

				Context context = new Context(instructionSet, from, from.EndIndex);
				context.GotoPrevious();

				while (context.IsEmpty)
				{
					context.GotoPrevious();
				}

				if (!(context.Instruction is Jmp))
					continue;

				Debug.Assert(context.Instruction.FlowControl == FlowControl.Branch);
				Debug.Assert(context.BranchTargets.Length == 1);

				var target = context.BranchTargets[0];

				if (next.Label != target)
					continue;

				context.Remove();
			}
		}

		#endregion IMethodCompilerStage Members
	}
}