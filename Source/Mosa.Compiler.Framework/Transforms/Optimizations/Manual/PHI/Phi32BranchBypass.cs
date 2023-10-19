// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi;

public sealed class Phi32BranchBypass : BasePhiTransform
{
	public Phi32BranchBypass() : base(IRInstruction.Phi32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.Block.PreviousBlocks.Count != 2 || context.Block.NextBlocks.Count != 2)
			return false;

		if (!context.Result.IsUsedOnce)
			return false;

		if (!context.Result.IsDefinedOnce)
			return false;

		if (!(context.Operand1.IsResolvedConstant || context.Operand2.IsResolvedConstant))
			return false;

		var ctx = context.Result.Uses[0];

		if (ctx.Instruction != IRInstruction.Branch32)
			return false;

		if (!ctx.Operand2.IsResolvedConstant)
			return false;

		if (!context.Node.PreviousNonEmpty.IsBlockStartInstruction)
			return false;

		if (context.Node.NextNonEmpty != ctx)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var phiValue = context.Operand1.IsResolvedConstant ? context.Operand1 : context.Operand2;
		var incomingBlock = context.PhiBlocks[context.Operand1.IsResolvedConstant ? 0 : 1];

		var ctx = context.Result.Uses[0];

		var result = Compare32(ctx.ConditionCode, phiValue, ctx.Operand2);

		var targetBlock = result
			? ctx.BranchTargets[0]
			: ctx.NextNonEmpty.BranchTargets[0];

		ReplaceBranchTarget(incomingBlock, ctx.Block, targetBlock);

		UpdatePhiBlock(context.Block);
	}

	private static void ReplaceBranchTarget(BasicBlock source, BasicBlock oldTarget, BasicBlock newTarget)
	{
		for (var node = source.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (node.Instruction.IsConditionalBranch || node.Instruction.IsUnconditionalBranch)
			{
				if (node.BranchTargets[0] == oldTarget)
				{
					node.UpdateBranchTarget(0, newTarget);
				}
				continue;
			}

			break;
		}
	}
}

//Block #4 - Label L_00015
//  Prev: L_00009, L_00014
//00015: IR.Phi32 v23 <= v36, 1 {L_00009, L_00014}
//00017: IR.Branch32[>=] v23, 0 L_0001D
//00017: IR.Jmp L_00019
//  Next: L_0001D, L_00019
