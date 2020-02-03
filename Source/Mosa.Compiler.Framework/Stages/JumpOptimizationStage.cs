// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stages removes jumps to the next instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class JumpOptimizationStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var trace = CreateTraceLog();

			for (int f = 0; f < BasicBlocks.Count - 1; f++)
			{
				var from = BasicBlocks[f];
				var next = BasicBlocks[f + 1];

				if (!from.NextBlocks.Contains(next))
					continue;

				var node = from.BeforeLast.GoBackwardsToNonEmpty();

				Debug.Assert(node.Instruction.FlowControl == FlowControl.UnconditionalBranch);
				Debug.Assert(node.BranchTargetsCount != 0);

				var target = node.BranchTargets[0];

				if (next == target)
				{
					// insert pseudo flow instruction --- this is important when protected regions exists
					node.SetInstruction(IRInstruction.Flow, target);
					continue;
				}
				else
				{
					if (from.NextBlocks.Count != 2)
						continue;

					// reverse the condition and swap branch and jump target, and remove jump instruction
					var jumpNode = node;
					var branchNode = node.Previous.GoBackwardsToNonEmpty();

					Debug.Assert(node.BranchTargetsCount == 1);

					var jumpTarget = jumpNode.BranchTargets[0];

					// reverse condition
					branchNode.ConditionCode = branchNode.ConditionCode.GetOpposite();

					// insert pseudo flow instruction --- this is important when protected regions exists
					jumpNode.SetInstruction(IRInstruction.Flow, branchNode.BranchTargets[0]);

					// update branch target
					branchNode.UpdateBranchTarget(0, jumpTarget);

					continue;
				}
			}
		}
	}
}
