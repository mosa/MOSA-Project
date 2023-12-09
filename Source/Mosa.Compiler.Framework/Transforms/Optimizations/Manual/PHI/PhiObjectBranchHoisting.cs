// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi;

public sealed class PhiObjectBranchHoisting : BasePhiTransform
{
	public PhiObjectBranchHoisting() : base(IR.PhiObject, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
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

		if (ctx.Instruction != IR.BranchObject)
			return false;

		if (!ctx.Operand2.IsResolvedConstant)
			return false;

		if (!context.Node.PreviousNonEmpty.IsBlockStartInstruction)
			return false;

		if (context.Node.NextNonEmpty != ctx)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var phiValue = context.Operand1.IsResolvedConstant ? context.Operand1 : context.Operand2;
		var incomingBlock = context.PhiBlocks[context.Operand1.IsResolvedConstant ? 0 : 1];

		var ctx = context.Result.Uses[0];

		var result = Compare64(ctx.ConditionCode, phiValue, ctx.Operand2);

		var targetBlock = result
			? ctx.BranchTargets[0]
			: ctx.NextNonEmpty.BranchTarget1;

		ReplaceBranchTarget(incomingBlock, ctx.Block, targetBlock);

		UpdatePhiBlock(context.Block);
	}
}
