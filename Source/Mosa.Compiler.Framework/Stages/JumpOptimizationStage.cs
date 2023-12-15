// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stages removes jumps to the next instruction
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
public sealed class JumpOptimizationStage : BaseMethodCompilerStage
{
	private readonly Counter JumpsRemovedCount = new("JumpOptimization.JumpsRemoved");

	protected override void Initialize()
	{
		Register(JumpsRemovedCount);
	}

	protected override void Run()
	{
		for (var f = 0; f < BasicBlocks.Count - 1; f++)
		{
			var from = BasicBlocks[f];
			var next = BasicBlocks[f + 1];

			if (!from.NextBlocks.Contains(next))
				continue;

			var node = from.BeforeLast.BackwardsToNonEmpty;

			//Debug.Assert(node.Instruction.FlowControl == FlowControl.UnconditionalBranch);
			Debug.Assert(node.BranchTargetsCount != 0);

			var target = node.BranchTarget1;

			if (next == target)
			{
				// insert pseudo flow instruction --- this is important when protected regions exists
				node.SetInstruction(IR.Flow, target);
				JumpsRemovedCount.Increment();
				continue;
			}
			else
			{
				if (from.NextBlocks.Count != 2)
					continue;

				// reverse the condition and swap branch and jump target, and remove jump instruction
				var jumpNode = node;
				var branchNode = node.Previous.BackwardsToNonEmpty;

				Debug.Assert(node.BranchTargetsCount == 1);

				var jumpTarget = jumpNode.BranchTarget1;

				// reverse condition
				branchNode.ConditionCode = branchNode.ConditionCode.GetOpposite();

				// insert pseudo flow instruction --- this is important when protected regions exists
				jumpNode.SetInstruction(IR.Flow, branchNode.BranchTargets[0]);
				// update branch target
				branchNode.UpdateBranchTarget(0, jumpTarget);

				JumpsRemovedCount.Increment();
				continue;
			}
		}
	}
}
