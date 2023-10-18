// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.Transforms.Optimizations;

public sealed class Test : BaseTransform
{
	public Test() : base(IRInstruction.Phi32, TransformType.Manual | TransformType.Optimization, true)
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

		if (!(ctx.ConditionCode == ConditionCode.Equal || ctx.ConditionCode == ConditionCode.NotEqual))
			return false;

		if (!ctx.Operand2.IsResolvedConstant)
			return false;

		if (context.Node.PreviousNonEmpty.IsBlockStartInstruction)
			return false;

		if (context.Node.NextNonEmpty != ctx)
			return false;

		return false;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var ctx = context.Result.Uses[0];

		var action = EvaluateBranch(ctx.ConditionCode, context.Operand1, context.Operand2, context.Operand2.ConstantUnsigned32);
		var sourceBlock = context.PhiBlocks[action.SourceBlock];

		var targetBlock = action.BranchResult
			? ctx.BranchTargets[0]
			: GetOtherBranchTarget(sourceBlock, ctx.BranchTargets[0]);

		ReplaceBranchTarget(ctx.PhiBlocks[action.SourceBlock], ctx.Block, targetBlock);

		TransformContext.UpdatePhiBlock(ctx.Block);
	}

	private static void ReplaceBranchTarget(BasicBlock source, BasicBlock oldTarget, BasicBlock newTarget)
	{
		for (var node = source.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
		{
			if (node.IsEmptyOrNop)
				continue;

			if ((node.Instruction.IsConditionalBranch || node.Instruction.IsUnconditionalBranch) && node.BranchTargets[0] == oldTarget)
			{
				node.BranchTargets[0] = newTarget;
				continue;
			}

			break;
		}
	}

	private static (int SourceBlock, bool BranchResult) EvaluateBranch(ConditionCode condition, Operand phi1, Operand phi2, uint value)
	{
		switch (condition)
		{
			case ConditionCode.Equal when value == phi1.ConstantUnsigned32 && phi1.IsResolvedConstant: return (0, true); // operand1 -> true
			case ConditionCode.Equal when value == phi2.ConstantUnsigned32 && phi2.IsResolvedConstant: return (1, true); // operand2 -> true
			case ConditionCode.Equal when value != phi1.ConstantUnsigned32 && phi1.IsResolvedConstant: return (0, false); // operand1 -> false
			case ConditionCode.Equal when value != phi2.ConstantUnsigned32 && phi2.IsResolvedConstant: return (1, true); // operand2 -> false
			case ConditionCode.NotEqual when value == phi1.ConstantUnsigned32 && phi1.IsResolvedConstant: return (0, true); // operand1 -> false
			case ConditionCode.NotEqual when value == phi2.ConstantUnsigned32 && phi2.IsResolvedConstant: return (1, true); // operand2 -> false
			case ConditionCode.NotEqual when value != phi1.ConstantUnsigned32 && phi1.IsResolvedConstant: return (0, true); // operand1 -> true
			case ConditionCode.NotEqual when value != phi2.ConstantUnsigned32 && phi2.IsResolvedConstant: return (1, true); // operand2 -> true
		}

		throw new InvalidOperationCompilerException("Invalid State", true);
	}
}

//1001A: IR.Phi v134<2> <= 1, 0 { L_10018, L_10019}
//00038: IR.CompareIntBranch32[==] v134<2>, 9 L_0007D
//00038: IR.Jmp L_0003A
