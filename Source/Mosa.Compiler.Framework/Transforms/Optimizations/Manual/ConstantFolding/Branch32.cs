// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

public sealed class Branch32 : BaseTransform
{
	public Branch32() : base(IR.Branch32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (context.Block.NextBlocks.Count == 1)
			return false;

		return IsNormal(context.ConditionCode);
	}

	public override void Transform(Context context, Transform transform)
	{
		var target = context.BranchTarget1;
		var block = context.Block;

		if (!Compare32(context.ConditionCode, context.Operand1, context.Operand2))
		{
			context.SetNop();

			Framework.Transform.UpdatePhiBlock(target);
		}
		else
		{
			var phiBlock = GetOtherBranchTarget(block, target);

			context.SetInstruction(IR.Jmp, target);

			RemoveRemainingInstructionInBlock(context);

			Framework.Transform.UpdatePhiBlock(phiBlock);
		}
	}
}
